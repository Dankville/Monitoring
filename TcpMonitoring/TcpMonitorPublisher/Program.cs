using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpMonitoring;

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
                //ConsoleKeyInfo keystroke = Console.ReadKey();
                //host.PublisherServer.SendMessage(keystroke.Key.ToString());

                Console.WriteLine("press [enter] to send interface");
                Console.ReadLine();
                host.PublisherServer.SendInterface<IMonitoringPublisherClient>();
            }
        }
    }
}
