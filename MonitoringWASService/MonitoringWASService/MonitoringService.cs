using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace MonitoringWASService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class MonitoringService : IPubSubMonitoringService
    {
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
