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
	class Program
	{
		static void Main(string[] args)
		{
			TcpPublisherServer server = TcpPublisherServer.Instance;

			Host host = Host.Instance;
			host.PublisherServer = server;
			Task.Run(() => host.PublisherServer.StartServer("127.0.0.1", 9000));
			
			foreach(var item in server.mockQueueItems.mockItems)
			{
				Console.WriteLine($"Press enter to change state of queueItem {item.ID}");	
				Console.ReadLine();
				server.SendQueueItemStateChange(new QueueItemStateChangeMessage() { Data = $"{item.ID} got a new ID", QueueItemId = item.ID, NewState = StateType.Queued });
			}
			Console.WriteLine("No more queueitems");
		}
	}
}
