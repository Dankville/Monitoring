using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceProcess;
using System.Configuration.Install;
using System.ServiceModel.Description;
using System.ComponentModel;

namespace MonitoringWindowsService
{
    public class MonitoringWindowsService : ServiceBase
    {
        public ServiceHost serviceHost = null;
        

        public MonitoringWindowsService()
        {
            ServiceName = "PubSubMonitoringService";
        }

        public static void Main()
        {
            ServiceBase.Run(new MonitoringWindowsService());
        }

        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            Uri subscriptionAddress = new Uri("http://localhost:9000/PubSubMonitoringService/");
            Uri publishingAddress = new Uri("net.tcp://localhost:9001/PubSubMonitoringService/");

            serviceHost = new ServiceHost(typeof(MonitoringService));
            // Binding for handling subscriptions the subscriptions.
            WSDualHttpBinding subscriptionBinding = new WSDualHttpBinding();
            serviceHost.AddServiceEndpoint(typeof(IPubSubMonitoringService), subscriptionBinding, subscriptionAddress);

            // Binding for handling messages. We use NetTcpBinding because it's way faster.
            NetTcpBinding publishBinding = new NetTcpBinding(SecurityMode.None, false);
            serviceHost.AddServiceEndpoint(typeof(IPubSubMonitoringService), publishBinding, publishingAddress);


            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.HttpGetUrl = subscriptionAddress;
            serviceHost.Description.Behaviors.Add(smb);
            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }

    [RunInstaller(true)]
    public class MonitoringWindowsServiceInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public MonitoringWindowsServiceInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;

            service = new ServiceInstaller();
            service.ServiceName = "PubSubMonitoringService";

            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
