using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace MonitoringWASService
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IPubSubMonitoringContract))]
    public interface IPubSubMonitoringService
    {
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Subscribe();

        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void UnSubscribe();

        [OperationContract(IsOneWay = false)]
        void PublishMonitorMessage(string message);

        [OperationContract(IsOneWay = true)]
        void MonitoredApplicationHello();
    }

    public interface IPubSubMonitoringContract
    {
        [OperationContract(IsOneWay = true)]
        void PublishMonitorMessageRan(string message);

        [OperationContract(IsOneWay = true)]
        void PublishUnsubscribeMessage();

        [OperationContract(IsOneWay = true)]
        void PublishSubscribeMessage();

        [OperationContract(IsOneWay = true)]
        void ErrorOccured(string exceptionMessage);
    }
}
