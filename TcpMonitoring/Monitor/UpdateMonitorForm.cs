using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcpMonitoring.MessagingObjects;
using TcpMonitoring.QueueingItems;

namespace Monitor
{
	public delegate void ConnectionEventHandler(bool connected);
	public delegate void InitializingQueueItemsEventHandler(List<QueueItem> queueItems);

	public delegate void QueueItemStateChangedEventHandler(Guid itemID, StateType oldState, StateType newState);

	public static class UpdateMonitorForm
	{
		public static MonitorForm MonitorForm;

		public static event ConnectionEventHandler OnConnectionStateChange;
		public static event InitializingQueueItemsEventHandler OnInitializingQueueItemsInListView;
		public static event QueueItemStateChangedEventHandler OnQueueItemChanged;

		private static object _formUpdateLock = new object();

		public static void ConnectionStateChange(bool connected)
		{
			ThreadSafeConnectionStateChange(connected);
		}

		private static void ThreadSafeConnectionStateChange(bool connected)
		{
			if (MonitorForm != null && MonitorForm.InvokeRequired)
			{
				lock (_formUpdateLock)
					MonitorForm.Invoke(new ConnectionEventHandler(ThreadSafeConnectionStateChange), new object[] { connected });
			}
			else OnConnectionStateChange?.Invoke(connected);
		}

		public static void InitializeQueueListView(List<QueueItem> queueItems)
		{
			if (TcpPublisherClient.Instance.publisherTcpClient.Connected)
			{
				ThreadSafeInitializeListView(queueItems);
			}
		}

		private static void ThreadSafeInitializeListView(List<QueueItem> queueItems)
		{
			if (MonitorForm != null && MonitorForm.InvokeRequired)
				lock (_formUpdateLock)
					MonitorForm.Invoke(new InitializingQueueItemsEventHandler(ThreadSafeInitializeListView), new object[] { queueItems });
			else OnInitializingQueueItemsInListView?.Invoke(queueItems);
		}

		public static void UpdateQueueListViewItem(Guid itemID, StateType oldState, StateType newState)
		{
			if (TcpPublisherClient.Instance.publisherTcpClient.Connected)
				ThreadSafeUpdateListView(itemID, oldState, newState);
		}

		private static void ThreadSafeUpdateListView(Guid itemID, StateType oldState, StateType newState)
		{
			if (MonitorForm != null && MonitorForm.InvokeRequired)
			{
				lock (_formUpdateLock)
					MonitorForm.Invoke(new QueueItemStateChangedEventHandler(ThreadSafeUpdateListView), new object[] { itemID, oldState, newState });
			}
			else OnQueueItemChanged?.Invoke(itemID, oldState, newState);
		}

	}
}
