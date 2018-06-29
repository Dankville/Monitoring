﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PubSubMonitoringService
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
    }
    
    public interface IPubSubMonitoringContract
    {
        [OperationContract(IsOneWay = true)]
        void PublishMonitorMessageRan(string message);
    }
}
