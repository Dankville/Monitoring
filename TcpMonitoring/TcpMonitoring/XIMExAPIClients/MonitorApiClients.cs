using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Avit.XIMEx.APIClients.Discovery;
using Avit.XIMEx.APIClients.SecurityManagement;

using Avit.XIMEx.Discovery.SDK;
using Avit.XIMEx.SecurityManagement.SDK;
using Avit.XIMEx.API.SDK;

namespace TcpMonitoring.XIMExAPIClients
{
	public class MonitorApiClients
	{
		DiscoveryClient _discoveryClient;
		SecurityManagerClient _securityClient;

		ServerAPIContext _context;

		string _token;

		public MonitorApiClients(string username, string password)
		{
			_discoveryClient = new DiscoveryClient("http://localhost:53931/");
			_context = _discoveryClient.GetServerAPIContext();

			_securityClient = new SecurityManagerClient(_context.SecurityManagementAPIAddress);
			RequestToken(username, password);
		}

		public void PrepMonitoring()
		{
			ChangeToSystemContext();
		}

		private void RequestToken(string username, string password)
		{
			var response = _securityClient.Authenticate("LocalManagementApplicationAccess", username, password);
			if (response.Successfull)
				_token = response.Token;
			else
				throw new UnauthorizedAccessException(response.ErrorMessage);
		}

		private void ChangeToSystemContext()
		{
			_securityClient.ChangeToSystemContext(_token);
		}
	}
}
