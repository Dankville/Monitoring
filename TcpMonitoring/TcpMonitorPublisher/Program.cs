using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

			Console.WriteLine("Press [enter] to start mockqueueitems worker");
			Console.ReadLine();

			WorkerThreads.MockQueueItemsWorkers.Start();

			Console.ReadLine();

		}

		static Thread MockQueueItemsWorker(TcpPublisherServer server)
		{
			return new Thread(() =>
			{
				
			});

		} 

	}
}
