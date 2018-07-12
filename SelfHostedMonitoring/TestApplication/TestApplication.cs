using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfHostedMonitoring;

namespace TestApplication
{
    public class TestApp
    {
        Host host = Host.Instance();

        public void TestMethod()
        {
            while (true)
            {
                string message = Console.ReadLine();

                host.MonitorService.PublishMonitorMessage(message);
            }
        }
    }
}
