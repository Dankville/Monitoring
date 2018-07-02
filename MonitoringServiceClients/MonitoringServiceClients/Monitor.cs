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

            bool keepGoing = true;
            Console.WriteLine("Press [s] to subscribe.");
            while (keepGoing)
            {
                string answer = Console.ReadLine();
                switch (answer)
                {
                    case "u":
                        client.UnSubscribe();
                        Console.WriteLine("Press [s] to subscribe.");
                        break;
                    case "s":
                        client.Subscribe();
                        Console.WriteLine("Press [u] to unsubscribe.");
                        break;
                    default:
                        break;
                }
            }
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
