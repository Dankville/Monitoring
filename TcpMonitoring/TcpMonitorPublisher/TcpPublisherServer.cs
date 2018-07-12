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
        private Socket _WorkSocket;

        private CancellationToken _CancelationToken;

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
            _WorkSocket = handler;

            HeartBeatTask();
        }

        public void SendMessage(string message)
        {
            SendAsync(SerializeMonitorMessage(message));
        }

        public void SendErrorMessage(string errorMessage)
        {
            SendAsync(SerializeMonitorMessage(errorMessage));
        }

        public void SendInterface<T>()
        {
            SendAsync(SerializeInterface<T>());
        }

        public void SendHeartBeat(string heartBeatData)
        {
            Send(SerializeHeartbeat(heartBeatData));
        }

        private void SendAsync(string data)
        {
            byte[] byteArr = Encoding.ASCII.GetBytes(data);
            _WorkSocket.BeginSend(byteArr, 0, byteArr.Length, 0, new AsyncCallback(SendCallback), _WorkSocket);
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

        private void Send(string data)
        {
            byte[] dataBytes = Encoding.ASCII.GetBytes(data);
            _WorkSocket.Send(dataBytes);
        }

        public void ReceiveCallback(IAsyncResult result)
        {
            string content = string.Empty;

            StateObject state = (StateObject)result.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead = handler.EndReceive(result);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                content = state.sb.ToString();
                Console.WriteLine(content);
            }
        }

        private void HeartBeatTask()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    SendHeartBeat(JsonSerializer(MessageType.HeartBeat, "Heartbeat data"));
                    await Task.Delay(5000);
                }
            }).Wait();
        }

        private string JsonSerializer(MessageType messageType, string data)
        {
            switch (messageType)
            {
                case (MessageType.HeartBeat):
                    return SerializeHeartbeat(data);
                case (MessageType.Interface):
                    return SerializeInterface<IMonitoringPublisherClient>();
                case (MessageType.MonitorMessage):
                case (MessageType.ErrorMessage):
                    return SerializeMonitorMessage(data);
                default:
                    return "";
            }
        }

        private string SerializeHeartbeat(string data)
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>();
            messageDict.Add(nameof(MessageType), nameof(MessageType.HeartBeat));
            return JsonConvert.SerializeObject(messageDict);
        }

        private string SerializeInterface<T>()
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>();
            messageDict.Add(nameof(MessageType), nameof(MessageType.Interface));


            List<string> interfaceProps = new List<string>();
            List<string> interfaceMethods = new List<string>();

            Type myType = (typeof(T));

            foreach (var prop in myType.GetProperties())
                interfaceProps.Add(prop.ToString());
            foreach (var method in myType.GetMethods())
                interfaceMethods.Add(method.ToString());

            messageDict.Add(nameof(MessageType.InterfaceProperties), JsonConvert.SerializeObject(interfaceProps));
            messageDict.Add(nameof(MessageType.InterfaceMethods), JsonConvert.SerializeObject(interfaceMethods));

            return JsonConvert.SerializeObject(messageDict);
        }

        private string SerializeMonitorMessage(string data)
        {
            Dictionary<string, string> monitorMessageDict = new Dictionary<string, string>();
            monitorMessageDict.Add(nameof(MessageType), nameof(MessageType.MonitorMessage));
            monitorMessageDict.Add(nameof(MessageType.MessageData), data);
            return JsonConvert.SerializeObject(monitorMessageDict);
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

    }
}
