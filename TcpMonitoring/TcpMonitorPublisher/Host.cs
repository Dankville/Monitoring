using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpMonitoring;

namespace TcpMonitorPublisher
{
	class Host
	{
		public TcpPublisherServer PublisherServer { get; }
		public HeartBeatPublisher HeartBeatPublisher { get; }

		private static readonly Host _Instance = new Host();
		public static Host Instance => _Instance;

		private Host()
		{
			PublisherServer = TcpPublisherServer.Instance;
			HeartBeatPublisher = HeartBeatPublisher.Instance;
			Task.Run(() => PublisherServer.StartServer("127.0.0.1", 9000));
			Task.Run(() => HeartBeatPublisher.StartServer("127.0.0.1", 9001));
		}

		public void StartMockItemsWorker()
		{
			WorkerThreads.MockQueueItemsWorkers.Start();
		}

	}
}
