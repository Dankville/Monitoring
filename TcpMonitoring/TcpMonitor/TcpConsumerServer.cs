using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpMonitor
{
    class TcpConsumerServer
    {
        private TcpListener _MonitorConsumerServer;

        private TcpConsumerServer() { }

        private static TcpConsumerServer _Instance;

        public static TcpConsumerServer Instance()
        {
            if (_Instance == null)
            {
                _Instance = new TcpConsumerServer();
            }
            return _Instance;
        }

        public  void StartServer(string ipStr, int port)
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
            _MonitorConsumerServer = new TcpListener(endpoint);
            _MonitorConsumerServer.Start();
            Console.WriteLine("Consumer Server running and waiting for clients.");
            WaitForClients();
        }

        public  void WaitForClients()
        {
            _MonitorConsumerServer.BeginAcceptTcpClient(new AsyncCallback(OnClientConnected), null);
        }

        public void OnClientConnected(IAsyncResult ar)
        {
            try
            {
                TcpClient client = _MonitorConsumerServer.EndAcceptTcpClient(ar);
                if (client != null)
                {
                    Console.WriteLine("Publisher Connected");
                    HandleConsumerClientRequest(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            WaitForClients();
        }

        public void HandleConsumerClientRequest(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            Int32 responseBytes = stream.Read(buffer, 0, buffer.Length);
            string responseData = Encoding.ASCII.GetString(buffer, 0, responseBytes);

            Console.WriteLine(responseData);
        }

        public  void HeartBeat()
        {
            throw new NotImplementedException();
        }
    }
}
