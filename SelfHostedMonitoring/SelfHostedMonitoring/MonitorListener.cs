using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SelfHostedMonitoring
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MonitorListener : IMonitoringListener
    {
        private MonitorListener() { }

        private static MonitorListener _Instance = null;

        public static MonitorListener Instance()
        {
            if (_Instance == null)
            {
                _Instance = new MonitorListener();
            }
            return _Instance;
        }

        // calls to Monitor.
        IMonitoringContract _monitorMessageCalls = null;

        public delegate void MethodRanEventHandler(string message);
        public static event MethodRanEventHandler MonitoringMessageEvent = null;
        MethodRanEventHandler _subscribedMonitorHandler = null;

        public void Subscribe()
        {
            Console.WriteLine("Subscribed");
            _monitorMessageCalls = OperationContext.Current.GetCallbackChannel<IMonitoringContract>();
            _subscribedMonitorHandler = new MethodRanEventHandler(PublishMethodRanHandler);
            MonitoringMessageEvent = _subscribedMonitorHandler;
        }

        public void UnSubscribe()
        {
            MonitoringMessageEvent = null;
        }

        public void PublishMonitorMessage(string message)
        {
            MonitoringMessageEvent?.Invoke(message);
        }

        private void PublishMethodRanHandler(string message)
        {
            _monitorMessageCalls.PublishMonitorMessageRan(message);
        }
    }
}
