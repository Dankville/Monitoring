using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

using MonitoringServiceClients.MonitoringService;

namespace MonitoringServiceClients
{
    public class TestMonitor : IDisposable
    {
        [CallbackBehavior(UseSynchronizationContext = true)]
        public class MonitoringServiceCall : IMonitoringListenerCallback
        {
            public void ErrorOccured(string exceptionMessage)
            {
                Console.WriteLine(exceptionMessage);
                // log error
            }

            public void BeginHeartBeat()
            {
                TestMonitor.HeartBeatReceived();
            }

            public void PublishMonitorMessageRan(string message)
            {
                TestMonitor.MonitoredEventOccured(message);
            }
        }

        public delegate void MonitoredEventHandler(string message);
        public static event MonitoredEventHandler MonitoredEventOccured;
        
        private MonitoringListenerClient _client = null;

        public bool Subscribed { get; private set; } = false;
        
        public TestMonitor()
        {
            bool keepGoing = true;
            Console.WriteLine("Press [s] to subscribe.");
            while (keepGoing)
            {
                string answer = Console.ReadLine();
                switch (answer)
                {
                    case "u":
                        UnSubscribe();
                        Console.WriteLine("Press [s] to subscribe.");
                        break;
                    case "s":
                        Subscribe();
                        Console.WriteLine("Press [u] to unsubscribe.");
                        break;
                    default:
                        break;
                }
            }
        }

        private void Subscribe()
        {
            try
            {
                InstanceContext context = new InstanceContext(new MonitoringServiceCall());
                _client = new MonitoringListenerClient(context, "NetTcpBinding_IMonitoringListener");
                _client.Subscribe();

                MonitoredEventHandler callFromMonitoredAppHandler = new MonitoredEventHandler(ShowMessage);
                MonitoredEventOccured += callFromMonitoredAppHandler;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void UnSubscribe()
        {
            try
            {
                _client.UnSubscribe();
                MonitoredEventOccured = null;
                // Contingently add functionalities to unsubscribe from specifik monitored applications, based on name.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void HeartBeatReceived()
        {
            Console.WriteLine("Heartbeat received");
            InstanceContext context = new InstanceContext(new MonitoringServiceCall());
            MonitoringListenerClient client = new MonitoringListenerClient(context, "NetTcpBinding_IMonitoringListener");
            client.EndHeartBeat();
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
        
        public void Dispose()
        {
            if (Subscribed)
                _client.UnSubscribe();
        }
    }
}
