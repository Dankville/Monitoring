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
			Host host = Host.Instance;

			Console.WriteLine("Press [enter] to start mock items worker");
			Console.ReadLine();
			host.StartMockItemsWorker();
		}
	}
}
