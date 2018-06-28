using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PubSubService
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IPubSubContract))]
    public interface IPubSubService
    {
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Subscribe();

        [OperationContract(IsOneWay = false, IsTerminating = true)]
        void UnSubscribe();

        [OperationContract(IsOneWay = false)]
        void PublishMethodRan(string message);
    }
    
    public interface IPubSubContract
    {
        [OperationContract(IsOneWay = true)]
        void MethodRan(string message);
    }
}
