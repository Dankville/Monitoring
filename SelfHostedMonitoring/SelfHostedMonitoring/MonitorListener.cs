using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SelfHostedMonitoring
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public sealed class MonitorListener : IMonitoringListener
    {
        private static MonitorListener _Instance = null;

        private CancellationToken _CancellationToken;
        private int _HeartBeatCounter;

        private MonitorListener()
        {
        }

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

        // Contract methods
        // called by via RPC by monitor
        public void Subscribe()
        {
            _monitorMessageCalls = OperationContext.Current.GetCallbackChannel<IMonitoringContract>();
            _subscribedMonitorHandler = new MethodRanEventHandler(PublishMethodRanHandler);
            MonitoringMessageEvent = _subscribedMonitorHandler;

            _HeartBeatCounter = 0;
            _CancellationToken = new CancellationToken(false);
            HeartBeatTask();
        }

        // called by via RPC by monitor
        public void UnSubscribe()
        {
            MonitoringMessageEvent = null;
            _CancellationToken = new CancellationToken(true);
        }

        // called by via RPC by monitor
        public void EndHeartBeat()
        {
            _HeartBeatCounter = 0;
        }

        // MonitorListener methods
        public void PublishMonitorMessage(string message)
        {
            MonitoringMessageEvent?.Invoke(message);
        }

        private async void HeartBeatTask()
        {
            try
            {
                await Task.Run(async () =>
                {
                    while (true)
                    {
                        _monitorMessageCalls.BeginHeartBeat(delegate (IAsyncResult ar)
                        {
                            _HeartBeatCounter++;
                            Console.WriteLine(_HeartBeatCounter);
                            _monitorMessageCalls.EndHeartBeat(ar);
                        }, null);

                        await Task.Delay(5000, _CancellationToken);
                        if (_CancellationToken.IsCancellationRequested || _HeartBeatCounter >= 5)
                        {
                            UnSubscribe();
                            break;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                try
                {
                    _monitorMessageCalls.ErrorOccured(ex.Message);
                }
                catch (Exception exc)
                {
                    // log exception
                }
            }
        }

        // Even handlers
        private void PublishMethodRanHandler(string message)
        {
            try
            {
                _monitorMessageCalls.PublishMonitorMessageRan(message);
            }
            catch(Exception ex)
            {
                try
                {
                    _monitorMessageCalls.ErrorOccured(ex.Message);
                }
                catch(Exception exc)
                {
                    // log exception
                }
            }
        }
    }
}
