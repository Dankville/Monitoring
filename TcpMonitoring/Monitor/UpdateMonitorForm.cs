using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcpMonitoring.QueueingItems;

namespace Monitor
{
	public delegate void ConnectionEventHandler(bool connected);
	public delegate void InitializingQueueItemsEventHandler(List<QueueItem> queueItems);
	public delegate void QueueItemStateChangedEventHandler(Guid itemId, StateType oldState, StateType newState);

	public static class UpdateMonitorForm
	{
		public static MonitorForm MonitorForm;

		public static event ConnectionEventHandler OnConnectionStateChange;
		public static event InitializingQueueItemsEventHandler OnInitializingQueueItemsInListView;
		public static event QueueItemStateChangedEventHandler OnQueueItemChanged;

		public static void ConnectionStateChange(bool connected)
		{
			ThreadSafeConnectionStateChange(connected);
		}

		private static void ThreadSafeConnectionStateChange(bool connected)
		{
			if (MonitorForm != null && MonitorForm.InvokeRequired) MonitorForm.Invoke(new ConnectionEventHandler(ThreadSafeConnectionStateChange), new object[] { connected });
			else OnConnectionStateChange(connected);
		}

		public static void InitializeQueueListView(List<QueueItem> queueItems)
		{
			ThreadSafeInitializeListView(queueItems);
		}

		private static void ThreadSafeInitializeListView(List<QueueItem> queueItems)
		{
			if (MonitorForm != null && MonitorForm.InvokeRequired) MonitorForm.Invoke(new InitializingQueueItemsEventHandler(ThreadSafeInitializeListView), new object[] { queueItems });
			else OnInitializingQueueItemsInListView(queueItems);
		}

		public static void UpdateQueueListViewItem(Guid ItemID, StateType oldState, StateType newState)
		{
			ThreadSafeUpdateListView(ItemID, oldState, newState);
		}

		private static void ThreadSafeUpdateListView(Guid itemID, StateType oldState, StateType newState)
		{
			if (MonitorForm != null && MonitorForm.InvokeRequired) MonitorForm.Invoke(new QueueItemStateChangedEventHandler(ThreadSafeUpdateListView), new object[] { itemID, oldState, newState });
			else OnQueueItemChanged(itemID, oldState, newState);
		}

	}
}
