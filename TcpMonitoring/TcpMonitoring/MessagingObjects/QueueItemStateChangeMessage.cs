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
		public QueueItemStateChangeMessage(Guid queueItemId, StateType oldState, StateType newState, string data)
		{
			Data = data;
			QueueItemId = queueItemId;
			OldState = oldState;
			NewState = newState;
		}

		public string Data { get; set; }
		public Guid QueueItemId { get; private set; }
		public StateType OldState { get; private set; }
		public StateType NewState { get; private set; }	
	}
}
