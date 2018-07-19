using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TcpMonitoring;
using TcpMonitoring.MessagingObjects;
using TcpMonitoring.QueueingItems;

namespace TcpMonitorPublisher
{
	class MockQueueItems
	{
		public List<QueueItem> mockItems { get; set; }

		public MockQueueItems()
		{
			mockItems = new List<QueueItem>()
			{
				new QueueItem(){ID = 1, Data = $"QueueItem", QueueItemState = StateType.Waiting},
				new QueueItem(){ID = 2, Data = $"QueueItem", QueueItemState = StateType.Waiting},
				new QueueItem(){ID = 3, Data = $"QueueItem", QueueItemState = StateType.Waiting},
				new QueueItem(){ID = 4, Data = $"QueueItem", QueueItemState = StateType.Waiting},
				new QueueItem(){ID = 5, Data = $"QueueItem", QueueItemState = StateType.Waiting},
			};
		}
	}
}
