using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PubSubMonitoringService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PubSubMonitoringService : IPubSubMonitoringService
    {
        IPubSubMonitoringContract _serviceCallback = null;
        IPubSubMonitoringContract _monitoredAppHelloCallback = null;

        // Monitoring Messages
        public delegate void MonitoringMessageEventHandler(string message);
        public static event MonitoringMessageEventHandler MonitoringMessageEvent;
        MonitoringMessageEventHandler _monitorMessageHandler = null;
        
        private static PubSubMonitoringService _Instance = null;

        private PubSubMonitoringService() { }

        public static PubSubMonitoringService Instance()
        {
            if (_Instance == null)
            {
                _Instance =new PubSubMonitoringService();
            }
            return _Instance;
        }
        
        // Contract functions
        void IPubSubMonitoringService.Subscribe()
        {
            _serviceCallback = OperationContext.Current.GetCallbackChannel<IPubSubMonitoringContract>();

            Console.WriteLine("Subscribed");
            // Set eventhandler for monitoring messages in direction of Monitored application -> Monitor
            _monitorMessageHandler = new MonitoringMessageEventHandler(PublishMonitoringEventHandler);
            MonitoringMessageEvent += _monitorMessageHandler;

            // notify Application that it is being monitored.
            _monitoredAppHelloCallback.PublishSubscribeMessage();
        }

        void IPubSubMonitoringService.UnSubscribe()
        {
            Console.WriteLine("UnSubscribed");

            // notify Application that it is no longer monitored.
            _monitoredAppHelloCallback.PublishUnsubscribeMessage();
        }

        void IPubSubMonitoringService.PublishMonitorMessage(string message)
        {
            try
            {
                MonitoringMessageEvent(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message not send because no subscriptions.");
            }
        }

        void IPubSubMonitoringService.MonitoredApplicationHello()
        {
            Console.WriteLine("Monitored App says hello");
            // Application which can be monitored presents itself to the monitoring service,
            // and a callback channel is created so calls can be made from the service to publisher of monitored applications 
            _monitoredAppHelloCallback = OperationContext.Current.GetCallbackChannel<IPubSubMonitoringContract>();
        }


        // Monitoring Message Event handler
        void PublishMonitoringEventHandler(string message)
        {
            _serviceCallback.PublishMonitorMessageRan(message);
        }
    }
}
