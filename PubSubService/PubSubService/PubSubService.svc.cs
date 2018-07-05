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
    public class MonitoringService : IPubSubMonitoringService
    {
        public Dictionary<string, IPubSubMonitoringContract> MonitoredApplications = new Dictionary<string, IPubSubMonitoringContract>();

        private MonitoringService() { }

        private static MonitoringService _Instance = null;

        public static MonitoringService Instance()
        {
            if (_Instance == null)
            {
                _Instance = new MonitoringService();
            }
            return _Instance;
        }


        // calls to Monitor.
        IPubSubMonitoringContract _monitorMessageCalls = null;
        // calls to monitored application.
        IPubSubMonitoringContract _monitoredAppMessageCalls = null;

        public delegate void MethodRanEventHandler(string message);
        public static event MethodRanEventHandler MonitoringMessageEvent = null;
        MethodRanEventHandler _subscribedMonitorHandler = null;

        public void Subscribe()
        {
            _monitorMessageCalls = OperationContext.Current.GetCallbackChannel<IPubSubMonitoringContract>();
            _subscribedMonitorHandler = new MethodRanEventHandler(PublishMethodRanHandler);
            MonitoringMessageEvent = _subscribedMonitorHandler;

            try
            {
                _monitoredAppMessageCalls.PublishSubscribeMessage();
            }
            catch (Exception ex)
            {
                try
                {
                    _monitorMessageCalls.ErrorOccured($"An error occured in the MonitoringWindowsService:\n{ex.Message}");
                }
                catch (Exception exc)
                {
                    // log the exception.
                }
            }
        }

        public void UnSubscribe()
        {
            MonitoringMessageEvent = null;

            try
            {
                _monitoredAppMessageCalls.PublishUnsubscribeMessage();
            }
            catch (Exception ex)
            {
                try
                {
                    _monitorMessageCalls.ErrorOccured($"An error occured in the MonitoringWindowsService:\n{ex.Message}");
                }
                catch (Exception exc)
                {
                    // log the exception.
                }
            }
        }

        public void PublishMonitorMessage(string message)
        {
            MonitoringMessageEvent?.Invoke(message);
        }

        public void PublishMethodRanHandler(string message)
        {
            _monitorMessageCalls.PublishMonitorMessageRan(message);
        }

        public void MonitoredApplicationHello()
        {
            // setup a channel to communicate from monitoring service to monitoredApplication.
            _monitoredAppMessageCalls = OperationContext.Current.GetCallbackChannel<IPubSubMonitoringContract>();
        }
    }
}
