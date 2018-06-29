using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;

namespace MonitoringWindowsService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MonitoringService : IPubSubMonitoringService
    {
        public delegate void MethodRanEventHandler(string message);
        public static event MethodRanEventHandler MonitoringMessageEvent;

        IPubSubMonitoringContract _serviceCallback = null;
        MethodRanEventHandler _subscribedMonitorHandler = null;

        public void Subscribe()
        {
            _serviceCallback = OperationContext.Current.GetCallbackChannel<IPubSubMonitoringContract>();
            _subscribedMonitorHandler = new MethodRanEventHandler(PublishMethodRanHandler);
            MonitoringMessageEvent += _subscribedMonitorHandler;
        }

        public void UnSubscribe()
        {
            MonitoringMessageEvent -= _subscribedMonitorHandler;
        }

        public void PublishMonitorMessage(string message)
        {
            MonitoringMessageEvent(message);
        }

        public void PublishMethodRanHandler(string message)
        {
            _serviceCallback.PublishMonitorMessageRan(message);
        }
    }
}
