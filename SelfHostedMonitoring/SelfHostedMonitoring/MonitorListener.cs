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
        private int _HeartBeatCounter = 0;
        private bool _HeartBeatReceived = false;

        private MonitorListener(){  }

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
        // called via RPC by monitor
        public void Subscribe()
        {
            if (_monitorMessageCalls == null)
            {
                Console.WriteLine("Subscribed");
                _monitorMessageCalls = OperationContext.Current.GetCallbackChannel<IMonitoringContract>();
                _subscribedMonitorHandler = new MethodRanEventHandler(PublishMethodRanHandler);
                MonitoringMessageEvent = _subscribedMonitorHandler;

                HeartBeatTask();
            }
        }

        // called via RPC by monitor
        public void UnSubscribe()
        {
            Console.WriteLine("UnSubscribed");
            MonitoringMessageEvent = null;   
            _CancellationToken = new CancellationToken(true);
            _monitorMessageCalls = null;
        }

        // called via RPC by monitor.
        // Callback from BeginHeartBeat on client.
        public void EndHeartBeat()
        {
            Console.WriteLine("[x] EndHeartBeat");
            _HeartBeatReceived = true;
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
                _CancellationToken = new CancellationToken(false);
                await Task.Run(async () =>
                {
                    while (true)
                    {
                        Task.Run(() => BeginHeartBeat()).Wait();

                        await Task.Delay(1000, _CancellationToken);
                        if (_CancellationToken.IsCancellationRequested || _HeartBeatCounter >= 5)
                        {
                            ConnectionLost();
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

        private void BeginHeartBeat()
        {
            _HeartBeatCounter++;
            _HeartBeatReceived = false;
            if (_monitorMessageCalls != null)
            {
                Console.WriteLine("[o]BeginHeartbeat sent");
                try
                {
                    _monitorMessageCalls.BeginHeartBeat();
                }
                catch (Exception ex)
                {
                    // Connectionlost
                }
            }
        }

        private void ConnectionLost()
        {
            Console.WriteLine("Connection lost.");
            MonitoringMessageEvent = null;
            _monitorMessageCalls = null;
        }

        // Event handlers
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
