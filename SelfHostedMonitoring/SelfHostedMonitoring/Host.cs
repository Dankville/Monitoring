using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace SelfHostedMonitoring
{
    public class Host : IDisposable
    {
        private ServiceHost _SelfHost = null;
        public MonitorListener MonitorService => MonitorListener.Instance();

        private Host()
        {
            if (_SelfHost != null)
            {
                _SelfHost.Close();
            }

            Uri MetadataAddress = new Uri("http://localhost:9000/PubSubMonitoringServiceMetaData/");
            Uri PublishingAddress = new Uri("net.tcp://localhost:9001/PubSubMonitoringService/");

            _SelfHost = new ServiceHost(MonitorService, PublishingAddress);

            NetTcpBinding PublishBinding = new NetTcpBinding(SecurityMode.None, false);
            WSDualHttpBinding MetadataBinding = new WSDualHttpBinding();
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.HttpGetUrl = MetadataAddress;
            _SelfHost.Description.Behaviors.Add(smb);

            _SelfHost.AddServiceEndpoint(typeof(IMonitoringListener), PublishBinding, PublishingAddress);

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
