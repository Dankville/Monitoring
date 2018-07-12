using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            string localIp = "127.0.0.1";
            Console.WriteLine("press [enter] to connect.");
            Console.ReadLine();

            TcpPublisherClient client = TcpPublisherClient.Instance();
            client.BeginConnect(localIp, 9000);



            Console.ReadLine();
        }
    }
}
