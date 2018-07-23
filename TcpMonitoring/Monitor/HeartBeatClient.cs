using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcpMonitoring.MessagingObjects;

namespace Monitor
{
	public class HeartBeatClient
	{
		private static readonly HeartBeatClient _Instance = new HeartBeatClient();
		public static HeartBeatClient Instance => _Instance; 

		private HeartBeatClient(){}
		
		private Socket _heartBeatClient = null;
		private int _missedHeartBeats = 0;
		private Thread _heartBeatChecker;

		public void BeginConnect(string ipAddress, int port)
		{
			if (IPAddress.TryParse(ipAddress, out var ipAdd))
			{
				_heartBeatClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
				_heartBeatClient.BeginConnect(new IPEndPoint(ipAdd, port), new AsyncCallback(HeartBeatPublisherConnectCallback), null);
			}
		}

		private void HeartBeatPublisherConnectCallback(IAsyncResult ar)
		{
			if (ar.IsCompleted)
			{
				_heartBeatChecker = HeartBeatChecker();
				HeartBeatReceive();
				UpdateMonitorForm.ConnectionStateChange(true);
			}
		}

		private void HeartBeatReceive()
		{
			if (_heartBeatClient != null)
			{
				StateObject state = new StateObject();
				state.workSocket = _heartBeatClient;

				_heartBeatClient.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(HeartBeatReceiveCallback), state);
			}
		}

		private void HeartBeatReceiveCallback(IAsyncResult ar)
		{
			try
			{
				if (ar.IsCompleted)
				{
					StateObject state = (StateObject)ar.AsyncState;
					Socket handler = state.workSocket;

					if (handler.Connected)
					{
						int bytesRead = handler.EndReceive(ar);

						state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
						string msg = state.sb.ToString();

						HandleHeartBeat(msg);
						HeartBeatReceive();
					}
				}
				else
				{
					throw new Exception();
				}
			}
			catch (SocketException sockEx)
			{
				// try reconnect
				MessageBox.Show("Heartbeat client socket error: " + sockEx.Message);
			}
			catch (Exception ex)
			{
				// handle exception
				MessageBox.Show(ex.Message);
			}
		}

		private void HandleHeartBeat(string msg)
		{
			try
			{
				HeartbeatObject hb = JsonConvert.DeserializeObject<HeartbeatObject>(msg, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
				
				if (hb is HeartbeatObject)
				{
					_missedHeartBeats = 0;
				}
			}
			catch (Exception)
			{

			}
		}

		private Thread HeartBeatChecker()
		{
			var thread = new Thread(() =>
			{
				while (true)
				{
					try
					{
						Thread.Sleep(5000);

						if (_missedHeartBeats >= 5)
						{
							CloseConnection();
							break;
						}
						_missedHeartBeats++;
					}
					catch (Exception ex)
					{
						
					}
				}
			});
			thread.Start();
			return thread;
		}

		private void CloseConnection()
		{
			UpdateMonitorForm.ConnectionStateChange(false);

			_heartBeatChecker.Abort();
			_heartBeatClient = null;
		}
	}
}
