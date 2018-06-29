using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace MonitoringWindowsService
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IPubSubMonitoringContract))]
    public interface IPubSubMonitoringService
    {
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Subscribe();

        [OperationContract(IsOneWay = false, IsTerminating = true)]
        void UnSubscribe();

        [OperationContract(IsOneWay = false)]
        void PublishMonitorMessage(string message);
    }

    public interface IPubSubMonitoringContract
    {
        [OperationContract(IsOneWay = true)]
        void PublishMonitorMessageRan(string message);
    }
}
