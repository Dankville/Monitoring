using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TcpMonitoring.MessagingObjects;
using TcpMonitoring.QueueingItems;

namespace Monitor
{
	public partial class MonitorForm : Form
	{
		static Dictionary<Guid, ListViewItem> _listViewItems = new Dictionary<Guid, ListViewItem>();

		public MonitorForm()
		{
			UpdateMonitorForm.MonitorForm = this;
			InitializeComponent();
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			try
			{
				if (Int32.TryParse(txtBoxPort.Text, out var port) && IPAddress.TryParse(txtBoxIpAddress.Text, out var ipadd))
				{
					if (TcpPublisherClient.Instance.BeginConnect(ipadd, port))
					{
						UpdateMonitorForm.OnInitializingQueueItemsInListView += InitializeQueueItemsInForm;
						UpdateMonitorForm.OnConnectionStateChange += ConnectionStateChange;
					}
				}
				else
				{
					throw new Exception("Invalid ip address or port");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}


		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			if (TcpPublisherClient.Instance != null && TcpPublisherClient.Instance.isConnected)
			{
				ClearListviews();
				ConnectionStateChange(false);
			}
		}


		private void InitializeQueueItemsInForm(List<QueueItem> queueItems)
		{
			try
			{
				ClearListviews();

				for (int x = 0; x < queueItems.Count; x++)
				{
					ListViewItem lvi = new ListViewItem(queueItems[x].Data);
					_listViewItems.Add(queueItems[x].ID, lvi);

					switch (queueItems[x].QueueItemState)
					{
						case (StateType.Waiting):
							this.listViewWaiting.Items.Add(lvi);
							break;
						case (StateType.Queued):
							this.listViewQueued.Items.Add(lvi);
							break;
						case (StateType.InProgress):
							this.listViewInProgress.Items.Add(lvi);
							break;
						default:
							break;
					}
				}	
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			ResetAmountCount();
			UpdateMonitorForm.OnQueueItemChanged += QueueItemStateChange;
		}

		private void QueueItemStateChange(Guid itemID, StateType oldState, StateType newState)
		{
			try
			{
				var lvi = _listViewItems[itemID];
				
				switch (oldState)
				{
					case StateType.Waiting:
						this.listViewWaiting.Items.Remove(lvi);
						break;
					case StateType.Queued:
						this.listViewQueued.Items.Remove(lvi);
						break;
					case StateType.InProgress:
						this.listViewInProgress.Items.Remove(lvi);
						break;
				}

				switch (newState)
				{
					case StateType.Waiting:
						this.listViewWaiting.Items.Insert(0, lvi);
						break;
					case StateType.Queued:
						this.listViewQueued.Items.Insert(0, lvi);
						break;
					case StateType.InProgress:
						this.listViewInProgress.Items.Insert(0, lvi);
						break;
				}

				ResetAmountCount();
			}

			catch (Exception ex)
			{
				TcpPublisherClient.Instance.SendAsync(new ErrorMessageObject() { Data = ex.Message});
				MessageBox.Show(ex.Message);
			}
		}

		private void ConnectionStateChange(bool connected)
		{
			if (connected)
			{
				this.lblAlive.Text = "Connected";
				this.lblAlive.ForeColor = Color.FromName("Green");
			}
			else
			{
				HeartBeatClient.Instance.Disconnect();
				TcpPublisherClient.Instance.Unsubscribe();
				this.lblAlive.Text = "Disconnected";
				this.lblAlive.ForeColor = Color.FromName("Red");
			}
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
			this.lblWaitingAmount.Text = this.listViewWaiting.Items.Count.ToString();
			this.lblQueuedAmount.Text = this.listViewQueued.Items.Count.ToString();
			this.lblInProgressAmount.Text = this.listViewInProgress.Items.Count.ToString();
		}
	}
}
