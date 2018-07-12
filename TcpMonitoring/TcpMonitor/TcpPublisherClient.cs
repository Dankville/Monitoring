using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

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


        private static TcpPublisherClient _Instance = null;

        public Socket PublisherTcpClient;

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
            try
            {
                Console.WriteLine("Publisher Client Started");
                PublisherTcpClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception ex)
            {

            }
        }

        public void BeginConnect(string ipAddress, int port)
        {
            PublisherTcpClient.BeginConnect(ipAddress, port, new AsyncCallback(ConnectCallback), null);
            _ConnectDone.WaitOne();
        }

        public void ConnectCallback(IAsyncResult result)
        {
            PublisherTcpClient.EndConnect(result);
            _ConnectDone.Set();
            Receive();
        }

        public void Send(string message)
        {
            try
            {
                byte[] byteData = Encoding.ASCII.GetBytes(message);

                PublisherTcpClient.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), PublisherTcpClient);
            }
            catch (Exception ex)
            {

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

        public void Receive()
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = PublisherTcpClient;

                PublisherTcpClient.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
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
                Dictionary<string, string> messageDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonMessage);

                switch (messageDict["MessageType"])
                {
                    case "HeartBeat":
                        Console.WriteLine("Hearbeat received");
                        break;
                    case "MonitorMessage":
                        HandleMonitorMessage(messageDict["MessageData"]);
                        break;
                    case "ErrorMessage":
                        HandleErrorMessage(messageDict["MessageData"]);
                        break;
                    case "Interface":
                        HandleInterfaceMessage(messageDict);
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

        private void HandleErrorMessage(string data)
        {
            Console.WriteLine(data);
        }

        private void HandleMonitorMessage(string data)
        {
            Console.WriteLine(data);
        }

        private void HandleInterfaceMessage(Dictionary<string, string> interfaceDictionary)
        {
            List<string> methodStrings = JsonConvert.DeserializeObject<List<string>>(interfaceDictionary["InterfaceMethods"]);
            List<string> propertieStrings = JsonConvert.DeserializeObject<List<string>>(interfaceDictionary["InterfaceProperties"]);


        }

        public void Close()
        {
            PublisherTcpClient.Close();
        }
    }
}
