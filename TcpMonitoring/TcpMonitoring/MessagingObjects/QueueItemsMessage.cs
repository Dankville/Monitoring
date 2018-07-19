using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TcpMonitoring.QueueingItems;

namespace TcpMonitoring.MessagingObjects
{
	public class QueueItemsMessage : IMessage
	{
		public string Data { get; set; }

		public List<QueueItem> QueueItems { get; set; }
	}
}
