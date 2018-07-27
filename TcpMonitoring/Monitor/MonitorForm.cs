using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using TcpMonitoring.MessagingObjects;
using TcpMonitoring.QueueingItems;
using TcpMonitoring.XIMExAPIClients;

namespace Monitor
{
	public partial class MonitorForm : Form
	{
		static Dictionary<Guid, ListViewItem> _listViewItems = new Dictionary<Guid, ListViewItem>();
		static ConcurrentQueue<QueueItemStateChangeMessage> StateChanges = new ConcurrentQueue<QueueItemStateChangeMessage>();

		private object _updateLock = new object();

		public MonitorForm()
		{
			UpdateMonitorForm.MonitorForm = this;
			InitializeComponent();
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			try
			{
				MonitorApiClients apiClients = new MonitorApiClients(this.txtBoxUsername.Text, this.txtPasswd.Text);
				apiClients.PrepMonitoring();
			}
			catch (UnauthorizedAccessException)
			{
				MessageBox.Show("Onbekende combinatie van gebruikersnaam en wachtwoord");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			
			//try
			//{
			//	if (Int32.TryParse("9000", out var port) && IPAddress.TryParse("127.0.0.1", out var ipadd))
			//	{
			//		if (TcpPublisherClient.Instance.BeginConnect(ipadd, port))
			//		{
			//			UpdateMonitorForm.OnInitializingQueueItemsInListView += InitializeQueueItemsInForm;
			//			UpdateMonitorForm.OnConnectionStateChange += UpdateFormOnStateChange;
			//		}
			//	}
			//	else
			//	{
			//		throw new Exception("Invalid ip address or port");
			//	}
			//}
			//catch (Exception ex)
			//{
			//	MessageBox.Show(ex.Message);
			//}
		}


		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			if (TcpPublisherClient.Instance != null && TcpPublisherClient.Instance.isConnected)
			{
				HeartBeatClient.Instance.Disconnect();
				TcpPublisherClient.Instance.Unsubscribe();
				ClearListviews();
				UpdateFormOnStateChange(false);
			}
		}


		private void InitializeQueueItemsInForm(List<QueueItem> queueItems)
		{
			try
			{
				ClearListviews();

				lock (_updateLock)
				{
					for (int x = 0; x < queueItems.Count; x++)
					{
						ListViewItem lvi = new ListViewItem(queueItems[x].Data);
						lvi.Tag = queueItems[x].QueueItemState;
						_listViewItems.Add(queueItems[x].ID, lvi);
					}
				}

				this.listViewWaiting.Items.AddRange(_listViewItems.Where(x => (StateType)x.Value.Tag == StateType.Waiting).Select(i => i.Value).ToArray());
				this.listViewQueued.Items.AddRange(_listViewItems.Where(x => (StateType)x.Value.Tag == StateType.Queued).Select(i => i.Value).ToArray());
				this.listViewInProgress.Items.AddRange(_listViewItems.Where(x => (StateType)x.Value.Tag == StateType.InProgress).Select(i => i.Value).ToArray());

				this.listViewWaiting.Update();
				this.listViewQueued.Update();
				this.listViewInProgress.Update();

				ResetAmountCount();

				UpdateMonitorForm.OnQueueItemChanged += QueueItemStateChange;

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void QueueItemStateChange(Guid itemID, StateType oldState, StateType newState)
		{
			var item = _listViewItems[itemID];
			try
			{
				lock (_updateLock)
					RemoveItemFromListView(oldState, item);
				lock (_updateLock)
					AddItemToListView(newState, item);

				
				this.listViewWaiting.Update();
				this.listViewQueued.Update();
				this.listViewInProgress.Update();

				ResetAmountCount();
			}
			catch (Exception ex)
			{
				// log exception
				MessageBox.Show(ex.Message);
			}
		}

		private void RemoveItemFromListView(StateType state, ListViewItem item)
		{
			
				switch (state)
				{
					case StateType.Waiting:
						this.listViewWaiting.Items.Remove(item);
						break;
					case StateType.Queued:
						this.listViewQueued.Items.Remove(item);
						break;
					case StateType.InProgress:
						this.listViewInProgress.Items.Remove(item);
						break;
				}
			
		}

		private void AddItemToListView(StateType state, ListViewItem item)
		{
				switch (state)
				{
					case StateType.Waiting:
						this.listViewWaiting.Items.Insert(0, item);
						break;
					case StateType.Queued:
						this.listViewQueued.Items.Insert(0, item);
						break;
					case StateType.InProgress:
						this.listViewInProgress.Items.Insert(0, item);
						break;
				}
			

		}

		private void UpdateFormOnStateChange(bool connected)
		{
			if (connected)
			{
				this.lblAlive.Text = "Connected";
				this.lblAlive.ForeColor = Color.FromName("Green");
			}
			else
			{
				this.lblAlive.Text = "Disconnected";
				this.lblAlive.ForeColor = Color.FromName("Red");

				UpdateMonitorForm.OnInitializingQueueItemsInListView -= InitializeQueueItemsInForm;
				UpdateMonitorForm.OnConnectionStateChange -= UpdateFormOnStateChange;
				UpdateMonitorForm.OnQueueItemChanged -= QueueItemStateChange;
			}
			this.lblAlive.Update();
		}

		private void ClearListviews()
		{
			if (_listViewItems.Count > 0)
			{
				_listViewItems.Clear();
				if (this.listViewWaiting.Items.Count > 0) this.listViewWaiting.Items.Clear();
				if (this.listViewQueued.Items.Count > 0) this.listViewQueued.Items.Clear();
				if (this.listViewInProgress.Items.Count > 0) this.listViewInProgress.Items.Clear();
				ResetAmountCount();
			}
		}

		private void ResetAmountCount()
		{
			int waitingAmount= this.listViewWaiting.Items.Count;
			int queuedAmount = this.listViewQueued.Items.Count;
			int inProgressAmount = this.listViewInProgress.Items.Count;
			int totalAmount = waitingAmount + queuedAmount + inProgressAmount;

			this.lblWaitingAmount.Text = waitingAmount.ToString();
			this.lblWaitingAmount.Update();
			this.lblQueuedAmount.Text = queuedAmount.ToString();
			this.lblQueuedAmount.Update();
			this.lblInProgressAmount.Text = inProgressAmount.ToString();
			this.lblInProgressAmount.Update();


			this.lblTotalAmount.Text = totalAmount.ToString();
		}
	}
}
