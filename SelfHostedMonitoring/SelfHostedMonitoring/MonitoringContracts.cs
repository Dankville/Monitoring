using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SelfHostedMonitoring
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IMonitoringContract))]
    public interface IMonitoringListener
    {
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Subscribe();

        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void UnSubscribe();

        [OperationContract(IsOneWay = true)]
        void EndHeartBeat();
    }

    public interface IMonitoringContract
    {
        [OperationContract(IsOneWay = true)]
        void PublishMonitorMessageRan(string message);

        [OperationContract(IsOneWay = true)]
        void ErrorOccured(string exceptionMessage);

        [OperationContract(IsOneWay = false, AsyncPattern = true)]
        IAsyncResult BeginHeartBeat(AsyncCallback callback, object stateObject);
        void EndHeartBeat(IAsyncResult result);
    }
}
