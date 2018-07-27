using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TcpMonitoring.MessagingObjects;
using TcpMonitoring.QueueingItems;

namespace TcpMonitorPublisher
{
	public class WorkerThreads
	{
		static object _lock = new object();

		public static Thread MockQueueItemsWorkers = new Thread(() =>
		{
			MockQueueItems.fillMockItems(3000);
			while (!MockQueueItems.items.All(x => x.Value.QueueItemState == StateType.InProgress))
			{
				try
				{
					int indexItemToChange = new Random().Next(MockQueueItems.items.Count);
					QueueItem itemToChange = MockQueueItems.items[indexItemToChange];

					if (itemToChange.QueueItemState != StateType.InProgress)
					{
						lock (_lock)
						{
							int wait = new Random().Next(5);
							Thread.Sleep(wait * 10);


							StateType oldState = itemToChange.QueueItemState;
							StateType newState;

							if (itemToChange.QueueItemState == StateType.Waiting)
							{
								newState = StateType.Queued;
							}
							else if (itemToChange.QueueItemState == StateType.Queued)
							{
								newState = StateType.InProgress;
							}
							else
								break;

							MockQueueItems.items[indexItemToChange] = new QueueItem(itemToChange.ID, itemToChange.Data, newState);

							string messageData = $"{itemToChange.Data} from state {oldState} => {newState}";
							Console.WriteLine(messageData);

							var changeMessage = new QueueItemStateChangeMessage(itemToChange.ID, oldState, newState, messageData);

							if (Host.Instance.PublisherServer.clientState == PublisherClientState.Initializing)
								Host.Instance.PublisherServer.itemsChangedInInitialization.Add(changeMessage);

							if (Host.Instance.PublisherServer.clientSocket != null && Host.Instance.PublisherServer.clientSocket.Connected && Host.Instance.PublisherServer.clientState == PublisherClientState.Connected)
								Host.Instance.PublisherServer.SendQueueItemStateChange(changeMessage);
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					Console.ReadLine();
				}
			}
			Console.WriteLine("Worker is done, no more queue items in waiting or queued state.");
			Console.ReadLine();
		});
	}
}
