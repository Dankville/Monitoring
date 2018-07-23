using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TcpMonitoring.QueueingItems;

namespace TcpMonitoring.MessagingObjects
{
	public class MonitoringInitializationMessage : IMessage
	{
		public MonitoringInitializationMessage(List<QueueItem> queueItems, string heartbeatPublisherIp, int heartbeatPublisherPort)
		{
			QueueItems = queueItems;
			HeartbeatPublisherIpAdress = heartbeatPublisherIp;
			HeartbeatPublisherPort = heartbeatPublisherPort;
		}

		public string Data { get; set; }
		public string HeartbeatPublisherIpAdress { get; set; }
		public int HeartbeatPublisherPort { get; set; }

		public List<QueueItem> QueueItems { get; set; }
	}
}
