using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

using PubSubMonitoringService;

namespace PubSubMonitoringServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Host serviceHost = Host.Instance();
            Console.ReadLine();
        }
    }
}
