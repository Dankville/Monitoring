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
        static InstanceContext _Context = null;
        static PubSubServiceClient _Client = null;
        static Publisher _instance = null;

        private Publisher()
        {

        }

        public static Publisher PublisherInstance()
        {
            if (_instance == null)
            {
                
                _instance = new Publisher();
            }
            return _instance;
        }

        public static void PublishMessage(string message)
        {
            _Context = new InstanceContext(new MethodRanPublishMessage());
            _Client = new PubSubServiceClient(_Context, "NetTcpBinding_IPubSubService");
            _Client.PublishMethodRan(message);
            _Client.Close();
        }
    }
}
