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
		TcpPublisherClient _client;

		// delegates for Monitored Queue events.
		public delegate void ConnectionDoneEventHandler();
		public event ConnectionDoneEventHandler ConnectionDoneEvent;
		public ConnectionDoneEventHandler ConnectionHandler = null;
			   
		//public delegate void InitializeDoneEventHandler(List<QueueItem> queueItems);
		//public event InitializeDoneEventHandler InitializeDoneEvent;
		//public InitializeDoneEventHandler InitializeHandler = null;

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

			}
		}

		private void Connect(IPAddress ipAddres, int port)
		{
			_client = TcpPublisherClient.Instance();
			_client.BeginConnect(ipAddres, port);
			ConnectionHandler = new ConnectionDoneEventHandler(OnConnected);
			ConnectionDoneEvent += ConnectionHandler;
		}

		private void OnConnected()
		{
			IMessage initMessage = new InitalizeMessage();
			initMessage.Data = "Initialize";
			ConnectionDoneEvent -= ConnectionHandler;


			InitializeHandler = new InitializeDoneEventHandler(InitalizeQueueItemsInForm);
			InitializeDoneEvent += InitializeHandler;
			_client.Send(initMessage);
		}

		private void InitalizeQueueItemsInForm(List<QueueItem> queueItems)
		{
			try
			{
				InitializeDoneEvent -= InitializeHandler;

				foreach (var item in queueItems)
				{
					string[] row = { item.ID.ToString(), item.Data };
					var lvi = new ListViewItem(row);

					this.listViewWaiting.Items.Add(lvi);
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
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
