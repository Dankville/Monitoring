using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TcpMonitoring.QueueingItems;

namespace TcpMonitoring.MessagingObjects
{
	public class QueueItemStateChangeMessage : IMessage
	{
		public string Data { get; set; }
		public int QueueItemId { get; set; }
		public StateType NewState { get; set; }	
	}
}
