using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpMonitoring.QueueingItems
{
	public enum StateType
	{
		Waiting = 0,
		Queued = 1,

		InProgress = 3
	}
}
