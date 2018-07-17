using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TcpMonitoring.MessagingObjects;

namespace TcpMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Ip localIp = "127.0.0.1";
            //int port = 9000;

            //TcpPublisherClient client = TcpPublisherClient.Instance();

            //while (true)
            //{
            //    Thread.Sleep(100);
            //    if (!client.IsConnected)
            //    {
            //        Console.WriteLine("Press [enter] to connect.");
            //        ConsoleKey key = Console.ReadKey().Key;
            //        if (key == ConsoleKey.Enter)
            //        {
            //            client.BeginConnect(localIp, port);
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("Press [escape] to disconnect.");
            //        ConsoleKey key = Console.ReadKey().Key;
            //        if (key == ConsoleKey.Escape)
            //        {
            //            client.Unsubscribe();
            //        }
            //    }               
            //}
        }
    }
}
