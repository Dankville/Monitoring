using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpMonitoring;
using TcpMonitoring.MessagingObjects;

namespace TcpMonitoring
{
    public interface IMonitoringPublisher
    {
        void StartServer(string ipStr, int port);
        void WaitForClients();
        void OnClientConnected(IAsyncResult result);

        void SendHeartBeat();
        void SendErrorMessage(IMessage errorMessage);
        void SendObject(IMessage obj);

        void ReceiveCallback(IAsyncResult result);
        void Close();
    }
}
