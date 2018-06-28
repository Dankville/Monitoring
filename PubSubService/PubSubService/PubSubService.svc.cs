using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PubSubService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode =InstanceContextMode.Single)]
    public class PubSubService : IPubSubService
    {
        public delegate void MethodRanEventHandler(string message);
        public static event MethodRanEventHandler methodRanEvent;

        IPubSubContract _serviceCallback = null;
        MethodRanEventHandler _methodHandler = null;

        public void Subscribe()
        {
            _serviceCallback = OperationContext.Current.GetCallbackChannel<IPubSubContract>();
            _methodHandler = new MethodRanEventHandler(PublishMethodRanHandler);
            methodRanEvent += _methodHandler;

            Console.WriteLine(OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri.OriginalString);
        }

        public void UnSubscribe()
        {
            methodRanEvent -= _methodHandler;

            Console.WriteLine(OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri.OriginalString);
        }

        public void PublishMethodRan(string message)
        {
            methodRanEvent(message);
        }

        public void PublishMethodRanHandler(string message)
        {
            _serviceCallback.MethodRan(message);

            Console.WriteLine(OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri.OriginalString);
        }
    }
}
