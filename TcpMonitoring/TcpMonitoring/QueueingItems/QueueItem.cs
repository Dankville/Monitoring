using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpMonitoring.QueueingItems
{
	public class QueueItem
	{
		public int ID { get; set; }
		public string Data { get; set; }

		public StateType QueueItemState { get; set; }
	}
}
