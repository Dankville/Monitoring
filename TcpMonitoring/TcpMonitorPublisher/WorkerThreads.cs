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
		public static Thread MockQueueItemsWorkers = new Thread(() =>
		{
			while (true)
			{
				int wait = new Random().Next(5);
				Thread.Sleep(wait * 1000);

				int indexItemToChange;
				QueueItem itemToChange; 

				do
				{
					indexItemToChange = new Random().Next(MockQueueItems.items.Count - 1);
					itemToChange = MockQueueItems.items[indexItemToChange];
				} while (itemToChange.QueueItemState == StateType.InProgress);

				
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

				MockQueueItems.items[indexItemToChange] = new QueueItem()
				{
					ID = itemToChange.ID,
					Data = itemToChange.Data,
					QueueItemState = newState
				};

				string messageData = $"{itemToChange.Data} from state {oldState} => {newState}";
				Console.WriteLine(messageData);

			if (Host.Instance.PublisherServer.clientSocket.Connected)
					Host.Instance.PublisherServer.SendQueueItemStateChange(new QueueItemStateChangeMessage(itemToChange.ID, oldState, newState, messageData));
			}

			Console.WriteLine("Worker is done, no more queue items in waiting or queued state.");
			Console.ReadLine();
		});
	}
}
