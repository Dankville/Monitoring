﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpMonitoring.QueueingItems;

namespace TcpMonitoring.MessagingObjects
{
	public class ChangedItemsWhileInitializingMessage : IMessage
	{
		public List<QueueItemStateChangeMessage> items;
	}
}
