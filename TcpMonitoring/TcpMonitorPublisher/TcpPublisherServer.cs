using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;


using TcpMonitoring;
using TcpMonitoring.MessagingObjects;

namespace TcpMonitorPublisher
{
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public class TcpPublisherServer : IMonitoringPublisher
    {
        private static readonly IMonitoringPublisher _Instance = new TcpPublisherServer();
        
        private TcpListener _MonitorServer;
        private Socket _workSocket = null;
        private Task _heartbeatTask;
        
        
        private TcpPublisherServer() { }

        public static IMonitoringPublisher Instance => _Instance;

        public void StartServer(string ipStr, int port)
        {
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
                _MonitorServer = new TcpListener(endpoint);
                _MonitorServer.Start();
                Console.WriteLine("Publisher Server running and waiting for clients.");
                WaitForClients();
            }
            catch (Exception ex)
            {

            }
        }

        public void WaitForClients()
        {
            Socket monitorClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _MonitorServer.BeginAcceptSocket(new AsyncCallback(OnClientConnected), monitorClient);
        }

        public void OnClientConnected(IAsyncResult result)
        {
            Socket handler = _MonitorServer.EndAcceptSocket(result);

            StateObject state = new StateObject();
            state.workSocket = handler;
            _workSocket = handler;

            _heartbeatTask = NewHeartBeatTask();
            _heartbeatTask.Start();

            Receive();
        }

        private void SendAsync(string data)
        {
            if (_workSocket != null)
            {
                byte[] byteArr = Encoding.ASCII.GetBytes(data);
                _workSocket.BeginSend(byteArr, 0, byteArr.Length, 0, new AsyncCallback(SendCallback), _workSocket);
            }
        }


        private void Send(string data)
        {
            if (_workSocket != null)
            {
                byte[] dataBytes = Encoding.ASCII.GetBytes(data);
                _workSocket.Send(dataBytes);
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            try
            {
                Socket client = (Socket)result.AsyncState;
                int bytesSent = client.EndSend(result);
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
                state.workSocket = _workSocket;

                _workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception ex)
            {

            }
        }

        public void ReceiveCallback(IAsyncResult result)
        {
            string content = string.Empty;

            StateObject state = (StateObject)result.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = handler.EndReceive(result);
            Receive();

            state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
            string msg = state.sb.ToString();
            HandleMessage(msg);
        }

        private void HandleMessage(string jsonMsg)
        {
            try
            {
                IMessage message = JsonConvert.DeserializeObject<IMessage>(jsonMsg, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });

                switch (message)
                {
                    case SubscribeMessageObject S:
                        HandleSubscriptionMessage(S);
                        break;
                    case UnsubscribeMessageObject U:
                        HandleUnsubscribeMessage(U);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void HandleSubscriptionMessage(IMessage msg)
        {
            Console.WriteLine("\nsubscribed");
        }

        private void HandleUnsubscribeMessage(IMessage msg)
        {
            Console.WriteLine("\nunsubscribed");
            Close();
        }

        private Task NewHeartBeatTask()
        {
            return new Task(async () =>
            {
                while (true)
                {
                    SendHeartBeat();
                    await Task.Delay(1000);
                }
            });
        }

        private string SerializeObjectMessage(IMessage obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
        }

        private string SerializeHeartbeat()
        {
            try
            {
                HeartbeatObject heartbeat = new HeartbeatObject();
                heartbeat.Data = "Heartbeat data";
                return JsonConvert.SerializeObject(heartbeat, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public void SendErrorMessage(IMessage errorMessage)
        {
            SendAsync(SerializeMonitorMessage(errorMessage));
        }

        public void SendInterface<T>()
        {
            SendAsync(SerializeInterface<T>());
        }

        public void SendObject(IMessage message)
        {
            SendAsync(SerializeObjectMessage(message));
        }

        public void SendHeartBeat()
        {
            Send(SerializeHeartbeat());
        }

        private string SerializeInterface<T>()
        {
            try
            {
                if (!typeof(T).IsInterface)
                    throw new ArgumentException($"Type {typeof(T).Name} must be an interface.");

                Type iFaceType = (typeof(T));

                Dictionary<string, string> messageDict = new Dictionary<string, string>();
                messageDict.Add(nameof(MessageType), nameof(MessageType.Interface));
                messageDict.Add(nameof(MessageType.InterfaceName), iFaceType.Name.ToString());

                List<string> interfaceProps = new List<string>();
                List<string> interfaceMethods = new List<string>();

                foreach (var prop in iFaceType.GetProperties())
                    interfaceProps.Add(prop.ToString());
                foreach (var method in iFaceType.GetMethods())
                {
                    ParameterInfo[] paraInfo = method.GetParameters();

                    if (paraInfo.Length > 0)
                    {
                        string methodString = method.ToString();
                        string noParas = methodString.Split('(')[0];
                        noParas += '(';
                        int counter = 1;
                        foreach (var p in paraInfo)
                        {
                            noParas += p.ToString();

                            if (counter != paraInfo.Length)
                                noParas += ", ";
                            counter++;
                        }
                        noParas += ')';
                        interfaceMethods.Add(noParas);
                    }
                    else
                    {
                        interfaceMethods.Add(method.ToString());
                    }
                }

                messageDict.Add(nameof(MessageType.InterfaceProperties), JsonConvert.SerializeObject(interfaceProps));
                messageDict.Add(nameof(MessageType.InterfaceMethods), JsonConvert.SerializeObject(interfaceMethods));

                return JsonConvert.SerializeObject(messageDict);
            }
            catch (Exception AEx)
            {
                throw new Exception();
            }
        }

        private string SerializeMonitorMessage(IMessage message)
        {
            return JsonConvert.SerializeObject(message, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
        }

        public void Close()
        {
            _workSocket = null;
            _heartbeatTask.Dispose();
            _workSocket.Close();
        }

    }
}
