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
	public class MockQueueItems
	{
		public static List<QueueItem> items = new List<QueueItem>()
		{
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem1", QueueItemState = StateType.Waiting},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem2", QueueItemState = StateType.Waiting},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem3", QueueItemState = StateType.Waiting},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem4", QueueItemState = StateType.Waiting},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem5", QueueItemState = StateType.Waiting},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem6", QueueItemState = StateType.Waiting},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem7", QueueItemState = StateType.Waiting},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem8", QueueItemState = StateType.Waiting},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem9", QueueItemState = StateType.Waiting},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem10", QueueItemState = StateType.Waiting},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem11", QueueItemState = StateType.Queued},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem12", QueueItemState = StateType.Queued},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem13", QueueItemState = StateType.Queued},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem14", QueueItemState = StateType.Queued},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem15", QueueItemState = StateType.Queued},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem16", QueueItemState = StateType.Queued},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem17", QueueItemState = StateType.Queued},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem18", QueueItemState = StateType.Queued},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem19", QueueItemState = StateType.Queued},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem20", QueueItemState = StateType.Queued},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem21", QueueItemState = StateType.InProgress},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem22", QueueItemState = StateType.InProgress},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem23", QueueItemState = StateType.InProgress},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem24", QueueItemState = StateType.InProgress},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem25", QueueItemState = StateType.InProgress},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem26", QueueItemState = StateType.InProgress},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem27", QueueItemState = StateType.InProgress},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem28", QueueItemState = StateType.InProgress},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem29", QueueItemState = StateType.InProgress},
			new QueueItem(){ID = Guid.NewGuid(), Data = $"QueueItem30", QueueItemState = StateType.InProgress},
		};
	}
}
