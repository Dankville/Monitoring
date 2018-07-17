using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TcpMonitoring;
using TcpMonitoring.MessagingObjects;

namespace TcpMonitorPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            IMonitoringPublisher server = TcpPublisherServer.Instance;

            Host host = Host.Instance;
            host.PublisherServer = server;
            Task.Run(() => host.PublisherServer.StartServer("127.0.0.1", 9000));
            
            while (true)
            {
                string keyStr = Console.ReadKey().Key.ToString();

                IMessage message = new MessageObject();
                message.Data = keyStr;

                host.PublisherServer.SendObject(message);
            }
        }
    }
}
