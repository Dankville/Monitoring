using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpMonitoring
{
    public interface IMonitoringPublisherClient
    {
        string TestString { get; set; }

        void TestMethod();
        void TestMethodTwo(int TestPara);
    }
}
