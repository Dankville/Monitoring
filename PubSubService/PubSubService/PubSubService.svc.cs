using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PubSubMonitoringService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PubSubMonitoringService : IPubSubMonitoringService
    {
        IPubSubMonitoringContract _serviceCallback = null;

        // Monitoring Messages
        public delegate void MonitoringMessageEventHandler(string message);
        public static event MonitoringMessageEventHandler MonitoringMessageEvent;
        MonitoringMessageEventHandler _monitorMessageHandler = null;

        public bool MonitoringEnabled { get; private set; } = false;
        
        // Contract functions
        void IPubSubMonitoringService.Subscribe()
        {
            _serviceCallback = OperationContext.Current.GetCallbackChannel<IPubSubMonitoringContract>();

            Console.WriteLine("Subscribed");
            // Set eventhandler for monitoring messages in direction of Monitored application -> Monitor
            _monitorMessageHandler = new MonitoringMessageEventHandler(PublishMonitoringEventHandler);
            MonitoringMessageEvent = _monitorMessageHandler;
        }

        void IPubSubMonitoringService.UnSubscribe()
        {
            Console.WriteLine("UnSubscribed");

            MonitoringMessageEvent -= _monitorMessageHandler;
        }

        void IPubSubMonitoringService.PublishMonitorMessage(string message)
        {
            MonitoringMessageEvent(message);
        }
        

        // Monitoring Message Event handler
        void PublishMonitoringEventHandler(string message)
        {
            _serviceCallback.PublishMonitorMessageRan(message);
        }
    }
}
