using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoredApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TestApplication> apps = new List<TestApplication>()
            {
                new TestApplication("Test app one")
            };
            Console.WriteLine("Applications are ready, press [enter] to send messages from each test application.");
            Console.ReadLine();
            int count = 0;
            while (count < 10000)
            {
                foreach (var app in apps)
                {
                    app.TestMethod(count);
                }
                count++;
                //Console.ReadLine();
            }
            Console.WriteLine("Test methods are done press [enter] to terminate.");
            Console.ReadLine();
        }
    }
}
