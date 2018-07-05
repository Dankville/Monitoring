using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace PubSubMonitoringService
{
    public class Host : IDisposable
    {
        private ServiceHost _SelfHost;
        public MonitoringService MonitorService => MonitoringService.Instance();

        private Host()
        {
            Uri publishingAddress = new Uri("net.tcp://localhost:9001/PubSubMonitoringService/");

            _SelfHost = new ServiceHost(MonitorService, publishingAddress);
            NetTcpBinding publishBinding = new NetTcpBinding(SecurityMode.None, false);
            _SelfHost.AddServiceEndpoint(typeof(IPubSubMonitoringService), publishBinding, publishingAddress);

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();

            //_SelfHost.Description.Behaviors.Add(smb);
            _SelfHost.Open();

            Console.WriteLine($"The service is ready.");
            Console.WriteLine("Press [enter] to terminate.");
        }

        private static Host _Instance = null;

        public static Host Instance()
        {
            if (_Instance == null)
            {
                _Instance = new Host();
            }
            return _Instance;
        }

        public void Dispose()
        {
            if (_Instance != null)
            {
                _Instance = null;
                _SelfHost.Close();
            }
        }
    }
}