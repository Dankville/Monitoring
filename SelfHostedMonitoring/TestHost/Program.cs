using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfHostedMonitoring;
using TestApplication;

namespace TestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            TestApp testapp = new TestApp();
            testapp.TestMethod();
        }
    }
}
