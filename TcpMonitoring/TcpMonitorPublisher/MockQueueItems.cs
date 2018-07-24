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
		public static void fillMockItems(int amountMockItems)
		{
			for (int x = 0; x < amountMockItems; x++)
			{
				StateType[] types = new StateType[] { StateType.Waiting, StateType.Queued, StateType.InProgress };

				StateType type;

				if (x < amountMockItems / 3)
					type = StateType.Waiting;
				else if (x < (2 / 3) * amountMockItems)
					type = StateType.Queued;
				else
					type = StateType.InProgress;

				items.Add(new QueueItem(Guid.NewGuid(), $"QueueItem{x}", type));
			}
		}

		public static List<QueueItem> items = new List<QueueItem>();
	}
}
