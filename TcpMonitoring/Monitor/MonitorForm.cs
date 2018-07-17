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

namespace Monitor
{
	public partial class MonitorForm : Form
	{
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
					MonitorClientHost.Instance().Connect(ipadd, port);
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
	}
}
