using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
		TcpPublisherClient _client;
		static object _ListViewItemLock = new object();
		
		static Dictionary<Guid, ListViewItem> _listViewItems = new Dictionary<Guid, ListViewItem>();

		public MonitorForm()
		{
			UpdateQueueListView.MonitorForm = this;
			InitializeComponent();
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			try
			{
				if (Int32.TryParse(txtBoxPort.Text, out var port) && IPAddress.TryParse(txtBoxIpAddress.Text, out var ipadd))
				{
					try
					{
						_client = TcpPublisherClient.Instance();
						UpdateQueueListView.OnInitializingQueueItemsInListView += InitializeQueueItemsInForm;
						_client.BeginConnect(ipadd, port);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
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

		private void InitializeQueueItemsInForm(List<QueueItem> queueItems)
		{
			try
			{
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
			UpdateQueueListView.OnQueueItemChanged += QueueItemStateChange;
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
			}
			catch (Exception ex)
			{
				_client.Send(new ErrorMessageObject() { Data = "Error occured "});
				MessageBox.Show(ex.Message);
			}
		}
	}
}
