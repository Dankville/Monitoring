using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

using MonitoringServiceClients.MonitoringService;

namespace MonitoringServiceClients
{
    public class Monitor
    {
        [CallbackBehavior(UseSynchronizationContext = true)]
        public class MonitoringServiceCall : IPubSubMonitoringServiceCallback
        {
            public void PublishMonitorMessageRan(string message)
            {
                Monitor.MonitoredEventOccured(message);
            }

            public void PublishSubscribeMessage()
            {
                // Doesnt do anything because this app can't be subscribed too.
            }

            public void PublishUnsubscribeMessage()
            {
                // Doesnt do anything because this app can't be subscribed too.
            }
        }

        public delegate void MonitoredEventHandler(string message);
        public static event MonitoredEventHandler MonitoredEventOccured;
        
        public Monitor()
        {
            InstanceContext context = new InstanceContext(new MonitoringServiceCall());
            PubSubMonitoringServiceClient client = new PubSubMonitoringServiceClient(context, "WSDualHttpBinding_IPubSubMonitoringService");
            MonitoredEventHandler callbackHandler = new MonitoredEventHandler(ShowMessage);
            MonitoredEventOccured += callbackHandler;

            Console.WriteLine("Monitor started, press [enter] to subscribe.");
            Console.ReadLine();
            client.Subscribe();
            Console.WriteLine("Monitor subscribed, press [enter] to unsubscribe.");
            Console.ReadLine();
            client.UnSubscribe();
            Console.ReadLine();
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
