using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonitoredApplication.MonitoringService;

namespace MonitoredApplication
{
    class TestApplication
    {
        private string _appName;

        public TestApplication(string appName)
        {
            _appName = appName;
            Console.WriteLine($"{_appName} is ready to start publishing.");
        }

        public void TestMethod(int count)
        {
            Console.WriteLine($"{_appName} says 'Hello World'");
            Publisher.PublishMessage($"{count.ToString()}: {_appName} ran method {nameof(TestMethod)}");           
        }
    }
}
