using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TcpMonitoring.MessagingObjects;

namespace Monitor
{
	public class HeartBeatClient
	{
		private static readonly HeartBeatClient _Instance = new HeartBeatClient();
		public static HeartBeatClient Instance => _Instance;

		public Socket heartBeatClient { get; private set; }
		public bool Connected { get; private set; }

		private HeartBeatClient()
		{
		}
		
		private int _missedHeartBeats = 0;
		private Thread _heartBeatChecker;

		public void BeginConnect(string ipAddress, int port)
		{
			if (IPAddress.TryParse(ipAddress, out var ipAdd))
			{
				heartBeatClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
				heartBeatClient.BeginConnect(new IPEndPoint(ipAdd, port), new AsyncCallback(HeartBeatPublisherConnectCallback), null);
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

		public void UnSubscribe()
		{
			if (heartBeatClient != null && heartBeatClient.Connected)
				heartBeatClient.BeginDisconnect(true, new AsyncCallback(HeartbeatPublisherDisconnectCallback), null);
		}

		private void HeartbeatPublisherDisconnectCallback(IAsyncResult ar)
		{
			if (ar.IsCompleted)
				heartBeatClient.EndDisconnect(ar);
		}

		private void HeartBeatReceive()
		{
			if (heartBeatClient != null)
			{
				StateObject state = new StateObject();
				state.workSocket = heartBeatClient;
				
				heartBeatClient.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(HeartBeatReceiveCallback), state);	
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
				CheckSocketExceptionOnConnectedSocket(sockEx);
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
						Thread.Sleep(1000);

						if (_missedHeartBeats >= 5)
						{
							Disconnect();
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

		public void Disconnect()
		{
			if (heartBeatClient.Connected) {
				_heartBeatChecker.Abort();
				SendAsync(new UnsubscribeMessageObject());
			}
			UpdateMonitorForm.ConnectionStateChange(false);
		}

		private void SendAsync(IMessage msg)
		{
			try
			{
				string msgJson = JsonConvert.SerializeObject(msg, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
				byte[] msgBytes = Encoding.ASCII.GetBytes(msgJson);
				switch (msg)
				{
					case UnsubscribeMessageObject U:
						heartBeatClient.BeginSend(msgBytes, 0, msgBytes.Length, 0, new AsyncCallback(UnsubscribeMessageCallback), heartBeatClient);
						break;
					default:
						break;
				}
			}
			catch (SocketException sockEx)
			{
				CheckSocketExceptionOnConnectedSocket(sockEx);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void UnsubscribeMessageCallback(IAsyncResult ar)
		{
			if (ar.IsCompleted)
			{
				heartBeatClient.Close(5000);
			}
		}

		private void CheckSocketExceptionOnConnectedSocket(SocketException ex)
		{
			if (heartBeatClient.Connected)
			{
				MessageBox.Show(ex.Message);
			}
			else
			{
				// try reconnect;
			}
		}
	}
}
