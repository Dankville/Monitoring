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
	public delegate void ConnectionDoneEventHandler();
	public delegate void InitializingQueueItemsEventHandler(List<QueueItem> queueItems);
	public delegate void QueueItemStateChangedEventHandler(int itemId, StateType newState);

	public partial class MonitorForm : Form
	{
		TcpPublisherClient _client;
		List<QueueItem> _queueItems;
		List<QueueListViewItem> _listViewItems = new List<QueueListViewItem>();

		public MonitorForm()
		{
			InitializeComponent();
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			try
			{
				IPAddress ipadd;
				int port;
				if (Int32.TryParse(txtBoxPort.Text, out port) && IPAddress.TryParse(txtBoxIpAddress.Text, out ipadd))
				{
					Connect(ipadd, port);
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

		private void Connect(IPAddress ipAddres, int port)
		{
			try
			{
				_client = TcpPublisherClient.Instance();
				_client.connectionDone += OnConnected;

				_client.BeginConnect(ipAddres, port);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void OnConnected()
		{
			try
			{
				_client.connectionDone -= OnConnected;
				_client.initialQueueItemsReceived += InitializeQueueItemsInForm;

				IMessage initMessage = new InitalizeMessage();
				initMessage.Data = "Initialize";
				_client.Send(initMessage);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void InitializeQueueItemsInForm(List<QueueItem> queueItems)
		{
			_queueItems = queueItems;
			_client.initialQueueItemsReceived -= InitializeQueueItemsInForm;

			try
			{
				foreach (var i in queueItems)
				{
					var lvi = new ListViewItem(new string[] {i.ID.ToString(), i.Data });
					_listViewItems.Add(new QueueListViewItem() { ID = i.ID, ListViewItem = lvi });
					
					this.listViewWaiting.Items.Add(lvi);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			_client.queueItemStateChange += QueueItemStateChange;
		}

		private void QueueItemStateChange(int itemID, StateType newState)
		{
			QueueItem itemToChange = _queueItems.Where(i => i.ID == itemID).First();

			var lvi = _listViewItems.Where(i => i.ID == itemID).First().ListViewItem;
			this.listViewWaiting.Items.Remove(lvi);

			this.listView1.Items.Insert(0, lvi);
		}

		private void ShowException(Exception ex)
		{
			MessageBox.Show(ex.Message);
		}

		private void UpdateQueueItemInForm()
		{

		}
	}
}
