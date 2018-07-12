using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpMonitoring;

namespace TcpMonitorPublisher
{
    class Host
    {
        public IMonitoringPublisher PublisherServer { get; set; }

        private static readonly Host _Instance = new Host();

        private Host() { }

        public static Host Instance => _Instance;


    }
}
