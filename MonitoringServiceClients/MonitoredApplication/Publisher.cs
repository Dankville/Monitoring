using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

using MonitoredApplication.MonitoringService;

namespace MonitoredApplication
{
    public class Publisher
    {
        [CallbackBehavior(UseSynchronizationContext = false)]
        public class MonitoredAppCalls : IPubSubMonitoringServiceCallback
        {
            public void PublishMonitorMessageRan(string message)
            {
                // does nothing since we dont get Monitoring messages here
            }
        }


        // should be a singleton
        private static Publisher _Instance = null;
        private Publisher() { }

        public static Publisher Instance()
        {
            if (_Instance == null)
            {
                _Instance = new Publisher(); 
            }
            return _Instance;
        }

        public static bool MonitoringEnabled { get; private set; } = false;

        static InstanceContext _context = null;
        static PubSubMonitoringServiceClient _client = null;

        public static void PublishMessage(string message)
        {
            _context = new InstanceContext(new MonitoredAppCalls());
            _client = new PubSubMonitoringServiceClient(_context, "NetTcpBinding_IPubSubMonitoringService");
            _client.PublishMonitorMessage(message);
            _client.Close();
        }
    }
}
