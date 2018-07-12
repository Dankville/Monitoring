using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpMonitoring
{
    public interface IMonitoringPublisher
    {
        void StartServer(string ipStr, int port);
        void WaitForClients();
        void OnClientConnected(IAsyncResult result);

        void SendHeartBeat(string heartBeatData);
        void SendMessage(string message);
        void SendErrorMessage(string errorMessage);
        void SendInterface<T>();


        void ReceiveCallback(IAsyncResult result);
        void Close();
    }
}
