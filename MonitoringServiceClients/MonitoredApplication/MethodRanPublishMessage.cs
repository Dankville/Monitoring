using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonitoredApplication.MonitoringService;

namespace MonitoredApplication
{
    public class MethodRanPublishMessage : IPubSubServiceCallback
    {
        string _messageToSend;

        public void MethodRan(string message)
        {
            Console.Write("Message to send: ");
            _messageToSend = Console.ReadLine();
        }
    }
}
