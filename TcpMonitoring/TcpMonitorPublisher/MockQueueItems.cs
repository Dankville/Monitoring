using System;
using System.Collections.Concurrent;
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
		public static Dictionary<int, QueueItem> items = new Dictionary<int, QueueItem>();
		public static ConcurrentQueue<QueueItem> itemsQueue = new ConcurrentQueue<QueueItem>();

		public static void fillMockItems(int amountMockItems)
		{
			for (int x = 0; x < amountMockItems; x++)
			{
				StateType type;

				if (x < amountMockItems / 2)
					type = StateType.Waiting;
				else 
					type = StateType.Queued;

				items.Add(x, new QueueItem(Guid.NewGuid(), $"QueueItem{x}", type));
			}
		}
	}
}
