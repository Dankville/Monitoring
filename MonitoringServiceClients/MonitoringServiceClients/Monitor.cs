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
        public delegate void MonitoringEventHandler(string message);
        public static event MonitoringEventHandler MonitoringCallbackEvent;
        
        [CallbackBehavior(UseSynchronizationContext = false)]
        public class MonitoringServiceCallback : IPubSubServiceCallback
        {
            public void MethodRan(string message)
            {
                Monitor.MonitoringCallbackEvent(message);
            }
        }

        public Monitor()
        {
            InstanceContext context = new InstanceContext(new MonitoringServiceCallback());
            PubSubServiceClient client = new PubSubServiceClient(context, "WSDualHttpBinding_IPubSubService");
            MonitoringEventHandler callbackHandler = new MonitoringEventHandler(ShowMessage);
            MonitoringCallbackEvent += callbackHandler;
            client.Subscribe();

            Console.WriteLine("Monitoring is listening to subscribed application press [enter] to unsubscribe.");

            Console.ReadLine();

            client.UnSubscribe();
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
