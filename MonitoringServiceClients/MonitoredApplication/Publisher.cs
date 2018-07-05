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
        [CallbackBehavior(UseSynchronizationContext = true)]
        public class MonitoredAppCalls : IPubSubMonitoringServiceCallback
        {
            public void ErrorOccured(string exceptionMessage)
            {
                // ??
            }

            public void PublishMonitorMessageRan(string message)
            {
                // does nothing since we dont get Monitoring messages here
            }

            public void PublishSubscribeMessage()
            {
                Console.WriteLine("A monitor subscribed.");
                MonitoringEnabled = true;
            }

            public void PublishUnsubscribeMessage()
            {
                Console.WriteLine("A subscribed Monitor unsubscribed.");
                MonitoringEnabled = false;
            }
        }

        public static bool MonitoringEnabled { get; private set; } = false;

        // should be a singleton
        private static Publisher _Instance = null;
        private Publisher()
        {
            InstanceContext context = new InstanceContext(new MonitoredAppCalls());
            PubSubMonitoringServiceClient client = new PubSubMonitoringServiceClient(context, "WSDualHttpBinding_IPubSubMonitoringService");
            client.MonitoredApplicationHello();
            Console.WriteLine("Monitoring app said hello.");
        }

        public static Publisher Instance()
        {
            if (_Instance == null)
            {
                _Instance = new Publisher();
            }
            return _Instance;
        }
        
        public static void PublishMessage(string message)
        {
            InstanceContext context = new InstanceContext(new MonitoredAppCalls());
            PubSubMonitoringServiceClient client = new PubSubMonitoringServiceClient(context, "NetTcpBinding_IPubSubMonitoringService");
            client.PublishMonitorMessage(message);
            client.Close();
        }
    }
}
