using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using TcpMonitoring;
using TcpMonitoring.MessagingObjects;

namespace TcpMonitor
{
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    class TcpPublisherClient
    {
        // Events
        private ManualResetEvent _ConnectDone = new ManualResetEvent(false);
        private ManualResetEvent _SendDone = new ManualResetEvent(false);
        private ManualResetEvent _ReceiveDone = new ManualResetEvent(false);

        private Task _HeartBeatChecker;
        private Task _ReceiveLoopTask;
        private CancellationToken _cancelToken = new CancellationToken();

        public int _MissedHeartBeats = 0;

        private static TcpPublisherClient _Instance = null;

        public Socket _publisherTcpClient;

        public static TcpPublisherClient Instance()
        {
            if (_Instance == null)
            {
                _Instance = new TcpPublisherClient();
            }
            return _Instance;
        }

        private TcpPublisherClient()
        {
            Console.WriteLine("Publisher Client Started");
            _publisherTcpClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public void BeginConnect(string ipAddress, int port)
        {
            _publisherTcpClient.BeginConnect(ipAddress, port, new AsyncCallback(ConnectCallback), null);
            _ConnectDone.WaitOne();
        }

        private void ConnectCallback(IAsyncResult result)
        {
            _publisherTcpClient.EndConnect(result);
            _ConnectDone.Set();

            Console.WriteLine("Connected, press [enter] to subscribe.");
        }

        public void Subscribe()
        {
            _cancelToken = new CancellationToken(false);
            Send(new SubscribeMessageObject() { Data = "Subscribe" });
        }

        public void Unsubscribe()
        {
            Send(new UnsubscribeMessageObject() { Data = "Unsubscribe" });
        }

        public void Send(IMessage message)
        {
            try
            {
                string msgJson = JsonConvert.SerializeObject(message, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
                byte[] byteData = Encoding.ASCII.GetBytes(msgJson);

                switch (message)
                {
                    case SubscribeMessageObject S:
                        _publisherTcpClient.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SubscribeCallback), _publisherTcpClient);
                        break;
                    case UnsubscribeMessageObject U:
                        _publisherTcpClient.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(UnSubscribeCallback), _publisherTcpClient);
                        break;
                }
                
            }
            catch (Exception ex)
            {

            }
        }

        private void SubscribeCallback(IAsyncResult result)
        {
            try
            {
                Socket client = (Socket)result.AsyncState;

                int bytesSent = client.EndSend(result);

                if (result.IsCompleted)
                {
                    _cancelToken = new CancellationToken(false);
                    _ReceiveLoopTask = NewReceiveLoop();
                    _ReceiveLoopTask.Start();
                    _HeartBeatChecker = HeartbeatChecker();
                    _HeartBeatChecker.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void UnSubscribeCallback(IAsyncResult result)
        {
            try
            {
                Socket client = (Socket)result.AsyncState;

                int bytesSent = client.EndSend(result);

                if (result.IsCompleted)
                {
                    _cancelToken = new CancellationToken(true);
                    _ReceiveLoopTask.Dispose();
                    _HeartBeatChecker.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void Receive()
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = _publisherTcpClient;

                _publisherTcpClient.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (SocketException ex)
            {
                // Publisher was closed.
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                StateObject state = (StateObject)result.AsyncState;
                Socket client = state.workSocket;
                int bytesRead = client.EndReceive(result);
                Receive();

                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                string msg = state.sb.ToString();

                HandleMessage(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleMessage(string jsonMessage)
        {
            try
            {
                IMessage message = JsonConvert.DeserializeObject<IMessage>(jsonMessage, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All});
                
                switch (message)
                {
                    case HeartbeatObject H:
                        HandleHeartBeatMessage(H);
                        break;
                    case MessageObject M:
                        HandleObjectMessage(M);
                        break;
                    case ErrorMessageObject E:
                        HandleErrorMessage(E);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleHeartBeatMessage(HeartbeatObject message)
        {
            // do something idk what yet, depends on XIMEx.
            _MissedHeartBeats = 0;
        }

        private void HandleObjectMessage(MessageObject message)
        {
            try
            {
                Console.WriteLine(message.Data);
            }
            catch (Exception)
            {

            }
        }

        private void HandleErrorMessage(ErrorMessageObject error)
        {
            Console.WriteLine(error.Data);
        }


        // Opdracht verkeerd begrepen.
        private void HandleInterfaceMessage(Dictionary<string, string> interfaceDictionary)
        {
            List<string> methodStrings = JsonConvert.DeserializeObject<List<string>>(interfaceDictionary["InterfaceMethods"]);
            List<string> propertieStrings = JsonConvert.DeserializeObject<List<string>>(interfaceDictionary["InterfaceProperties"]);

            InterfaceCreator interfaceCreator = new InterfaceCreator(interfaceDictionary["InterfaceName"], methodStrings, propertieStrings);
            interfaceCreator.CreateInterfaceAssembly();
        }

        private Task HeartbeatChecker()
        {
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(1000);

                        if (_cancelToken.IsCancellationRequested)
                        {
                            _cancelToken.ThrowIfCancellationRequested();
                        }

                        if (_MissedHeartBeats >= 5)
                        {
                            Close();
                            Console.WriteLine("5 heartbeats missed, closed connection");
                            break;
                        }
                        _MissedHeartBeats++;
                    }
                    catch (TaskCanceledException ex)
                    {
                        
                        _HeartBeatChecker.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }, _cancelToken);
            return task;
        }


        private Task NewReceiveLoop()
        {
            Task receiveLoopTask = new Task(() =>
            {
                try
                {
                    if (_cancelToken.IsCancellationRequested)
                    {
                        _cancelToken.ThrowIfCancellationRequested();
                    }
                    Receive();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }, _cancelToken);
            return receiveLoopTask;
        }

        public void Close()
        {
            _publisherTcpClient.Close();
        }
    }
}
