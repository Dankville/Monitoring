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
            }

            public void HeartBeat()
            {
                throw new NotImplementedException();
            }

            public void PublishMonitorMessageRan(string message)
            {
                TestMonitor.MonitoredEventOccured(message);
            }
        }

        public delegate void MonitoredEventHandler(string message);
        public static event MonitoredEventHandler MonitoredEventOccured;

        private InstanceContext _context = null;
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
                _context = new InstanceContext(new MonitoringServiceCall());
                _client = new MonitoringListenerClient(_context, "NetTcpBinding_IMonitoringListener");
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
