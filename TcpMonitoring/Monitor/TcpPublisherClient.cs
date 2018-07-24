using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;


using TcpMonitoring.MessagingObjects;
using TcpMonitoring.QueueingItems;
using System.Windows.Forms;
using System.IO;

namespace Monitor
{
	public class StateObject
	{
		// Client socket.
		public Socket workSocket = null;
		// Size of receive buffer.
		public const int BufferSize = 1024;
		// Receive buffer.
		public byte[] buffer = new byte[BufferSize];
		// Received data string.
		public StringBuilder sb = new StringBuilder();
	}

	public class TcpPublisherClient
	{
		public Socket publisherTcpClient { get; private set; }
		public int missedHeartBeats = 0;
		public bool isConnected = false;
		
		private string _lastReceivedMessage;

		// Singleton
		private static readonly TcpPublisherClient _Instance = new TcpPublisherClient();
		public static TcpPublisherClient Instance => _Instance;

		private TcpPublisherClient()
		{
		}

		public bool BeginConnect(IPAddress ipAddress, int port)
		{

			if (!isConnected)
			{
				publisherTcpClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
				publisherTcpClient.BeginConnect(ipAddress, port, new AsyncCallback(ConnectCallback), null);
				return true;
			}
			return false;
		}

		private void ConnectCallback(IAsyncResult result)
		{
			try
			{
				publisherTcpClient.EndConnect(result);

				if (result.IsCompleted)
				{
					isConnected = true;
					Receive();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				//MessageBox.Show("Could not make a connection to the machine with those credentials.");
			}
		}

		public void Unsubscribe()
		{
			if (publisherTcpClient.Connected)
			{
				StateObject state = new StateObject();
				state.workSocket = publisherTcpClient;
				SendAsync(new UnsubscribeMessageObject() { Data = "unsub" });

				publisherTcpClient.BeginDisconnect(true, new AsyncCallback(UnSubscribeCallback), state);
			}
		}

		private void UnSubscribeCallback(IAsyncResult result)
		{
			try
			{
				if (result.IsCompleted)
				{
					isConnected = false;
					publisherTcpClient.EndDisconnect(result);
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
				MessageBox.Show(ex.Message);
			}
		}

		public void SendAsync(IMessage message)
		{
			try
			{
				string msgJson = JsonConvert.SerializeObject(message, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
				byte[] byteData = Encoding.ASCII.GetBytes(msgJson);
				switch (message)
				{
					case UnsubscribeMessageObject U:
						publisherTcpClient.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(UnSubscribeCallback), publisherTcpClient);
						break;
					case ErrorMessageObject E:
						publisherTcpClient.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(ErrorMessageCallback), publisherTcpClient);
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
				// show exception in monitoring form
				MessageBox.Show(ex.Message);
			}
		}

		private void ErrorMessageCallback(IAsyncResult ar)
		{
			if (ar.IsCompleted)
			{
				// handle error message
			}
			else
			{
				throw new Exception();
			}
		}

		private void InitializeCallback(IAsyncResult result)
		{
			if (result.IsCompleted)
			{
				Socket client = (Socket)result.AsyncState;
				int bytesRead = client.EndSend(result);
				
			}
			else
			{
				throw new Exception();
			}
		
		}

		private void Receive()
		{
			try
			{
				StateObject state = new StateObject();
				state.workSocket = publisherTcpClient;

				publisherTcpClient.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
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

		private void ReceiveCallback(IAsyncResult result)
		{
			try
			{
				if (result.IsCompleted)
				{
					StateObject state = (StateObject)result.AsyncState;
					Socket client = state.workSocket;
					int bytesRead = client.EndReceive(result);

					state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
					_lastReceivedMessage += state.sb.ToString();
					// Check if last part of sent message.
					if (_lastReceivedMessage.IndexOf("<EOM>") != -1)
					{
						HandleReceivedMessage(_lastReceivedMessage);
					}

					Receive();
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
				MessageBox.Show(ex.Message);
			}
		}

		private void HandleReceivedMessage(string jsonMessage)
		{
			_lastReceivedMessage = "";
			try
			{
				List<string> messages = new List<string>();
				while (true)
				{
					if (String.IsNullOrEmpty(jsonMessage))
						break;

					int EOFIndex = jsonMessage.IndexOf("<EOM>");
					if (EOFIndex == -1)
						break;

					messages.Add(jsonMessage.Substring(0, EOFIndex));
					jsonMessage = jsonMessage.Remove(0, EOFIndex + 5);
				}
				
				foreach (string msg in messages)
				{
					IMessage iMsgObj = JsonConvert.DeserializeObject<IMessage>(msg, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, });
					switch (iMsgObj)
					{
						case ErrorMessageObject E:
							HandleErrorMessage(E);
							break;
						case MonitoringInitializationMessage I:
							HandleInitializationMessage(I);
							break;
						case QueueItemStateChangeMessage Q:
							HandleQueueItemStateChangeMessage(Q);
							break;
						case ChangedItemsInInitializingMessage C:
							HandleChangeItemsWhileInitializingMessage(C);
							break;
						default:
							break;
					}
				}
			}
			catch (Exception ex)
			{
				// show exception in monitoring form
				MessageBox.Show(ex.Message);

			}
		}

		private void HandleChangeItemsWhileInitializingMessage(ChangedItemsInInitializingMessage c)
		{
			foreach (var item in c.items)
			{
				UpdateMonitorForm.UpdateQueueListViewItem(item.QueueItemId, item.OldState, item.NewState);
			}
		}

		private void HandleQueueItemStateChangeMessage(QueueItemStateChangeMessage q)
		{
			UpdateMonitorForm.UpdateQueueListViewItem(q.QueueItemId, q.OldState, q.NewState);
		}

		private void HandleInitializationMessage(MonitoringInitializationMessage i)
		{
			try
			{
				HeartBeatClient.Instance.BeginConnect(i.HeartbeatPublisherIpAdress, i.HeartbeatPublisherPort);
				UpdateMonitorForm.InitializeQueueListView(i.QueueItems);
			}
			catch (Exception ex)
			{
				// TODO heartbeat initialization exception.
				MessageBox.Show(ex.Message);
			}
		}

		private void HandleErrorMessage(ErrorMessageObject error)
		{
			MessageBox.Show(error.Data);
		}

		public void Close()
		{
			publisherTcpClient.Close();
		}
		
		private void CheckSocketExceptionOnConnectedSocket(SocketException ex)
		{
			if (publisherTcpClient.Connected)
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