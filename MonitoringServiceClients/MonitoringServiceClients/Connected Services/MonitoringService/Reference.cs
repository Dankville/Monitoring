﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonitoringServiceClients.MonitoringService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MonitoringService.IPubSubMonitoringService", CallbackContract=typeof(MonitoringServiceClients.MonitoringService.IPubSubMonitoringServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IPubSubMonitoringService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPubSubMonitoringService/Subscribe", ReplyAction="http://tempuri.org/IPubSubMonitoringService/SubscribeResponse")]
        void Subscribe();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPubSubMonitoringService/Subscribe", ReplyAction="http://tempuri.org/IPubSubMonitoringService/SubscribeResponse")]
        System.Threading.Tasks.Task SubscribeAsync();
        
        [System.ServiceModel.OperationContractAttribute(IsTerminating=true, Action="http://tempuri.org/IPubSubMonitoringService/UnSubscribe", ReplyAction="http://tempuri.org/IPubSubMonitoringService/UnSubscribeResponse")]
        void UnSubscribe();
        
        [System.ServiceModel.OperationContractAttribute(IsTerminating=true, Action="http://tempuri.org/IPubSubMonitoringService/UnSubscribe", ReplyAction="http://tempuri.org/IPubSubMonitoringService/UnSubscribeResponse")]
        System.Threading.Tasks.Task UnSubscribeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPubSubMonitoringService/PublishMonitorMessage", ReplyAction="http://tempuri.org/IPubSubMonitoringService/PublishMonitorMessageResponse")]
        void PublishMonitorMessage(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPubSubMonitoringService/PublishMonitorMessage", ReplyAction="http://tempuri.org/IPubSubMonitoringService/PublishMonitorMessageResponse")]
        System.Threading.Tasks.Task PublishMonitorMessageAsync(string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPubSubMonitoringServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPubSubMonitoringService/PublishMonitorMessageRan")]
        void PublishMonitorMessageRan(string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPubSubMonitoringServiceChannel : MonitoringServiceClients.MonitoringService.IPubSubMonitoringService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PubSubMonitoringServiceClient : System.ServiceModel.DuplexClientBase<MonitoringServiceClients.MonitoringService.IPubSubMonitoringService>, MonitoringServiceClients.MonitoringService.IPubSubMonitoringService {
        
        public PubSubMonitoringServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public PubSubMonitoringServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public PubSubMonitoringServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PubSubMonitoringServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PubSubMonitoringServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void Subscribe() {
            base.Channel.Subscribe();
        }
        
        public System.Threading.Tasks.Task SubscribeAsync() {
            return base.Channel.SubscribeAsync();
        }
        
        public void UnSubscribe() {
            base.Channel.UnSubscribe();
        }
        
        public System.Threading.Tasks.Task UnSubscribeAsync() {
            return base.Channel.UnSubscribeAsync();
        }
        
        public void PublishMonitorMessage(string message) {
            base.Channel.PublishMonitorMessage(message);
        }
        
        public System.Threading.Tasks.Task PublishMonitorMessageAsync(string message) {
            return base.Channel.PublishMonitorMessageAsync(message);
        }
    }
}
