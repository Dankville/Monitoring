using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TcpMonitoring.QueueingItems;

namespace Monitor
{
	public class QueueListViewItem
	{
		public QueueItem Item { get; set; }
		public ListViewItem ListViewItem { get; set; }
	}
}
