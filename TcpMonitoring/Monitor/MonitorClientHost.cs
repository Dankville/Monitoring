using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TcpMonitor;

namespace Monitor
{
	public class MonitorClientHost
	{
		static TcpPublisherClient _client = null;
		static MonitorClientHost _Instance = null;

		private MonitorClientHost()
		{
			_client = TcpPublisherClient.Instance();
		}

		public static MonitorClientHost Instance()
		{
			if (_Instance == null)
			{
				_Instance = new MonitorClientHost();
			}
			return _Instance;
		}

		public TcpPublisherClient Client => _client;

		public void Connect(IPAddress ipAddres, int port)
		{
			_client.BeginConnect(ipAddres, port);
		}

	}
}
