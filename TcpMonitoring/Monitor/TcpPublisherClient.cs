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
				SendAsync(new UnsubscribeMessageObject());

				publisherTcpClient.BeginDisconnect(true, new AsyncCallback(UnSubscribeCallback), state);
			}
		}

		private void UnSubscribeCallback(IAsyncResult result)
		{
			try
			{
				if (result.IsCompleted)
				{
					_lastReceivedMessage = "";
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
			int lastMsgLength;
			int EOMIndex;
			try
			{
				if (result.IsCompleted)
				{
					StateObject state = (StateObject)result.AsyncState;
					Socket client = state.workSocket;
					int bytesRead = client.EndReceive(result);


					state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
					_lastReceivedMessage += state.sb.ToString();
					
					// The received message  of 1024 bytes or less comes in.
					// If the message doesn't have a EOM block, it's only a partial messages, so we can't process it and listen for more message bytes.
					// The next 1024 bytes or less comes in, whe add the current message to the end of the previous message. This way beginning of the message in the previous byte []
					// and the end of the message in the current byte [] are joined again. The <EOM> is found and we process the message respectively.
					// This way we can split the continuous stream of json strings in a bytes array format into seperate json messages and handle them respectively.

					if (_lastReceivedMessage.IndexOf("<EOM>", 0) != -1 && !String.IsNullOrEmpty(_lastReceivedMessage))
						while (true)
						{
							string jsonMessage = "";

							if (String.IsNullOrEmpty(_lastReceivedMessage))
								break;
							
							EOMIndex = _lastReceivedMessage.IndexOf("<EOM>", 0);
							if (EOMIndex == -1 || EOMIndex+1 > _lastReceivedMessage.Length)
								break;
							
							jsonMessage = _lastReceivedMessage.Substring(0, EOMIndex);
							_lastReceivedMessage = _lastReceivedMessage.Remove(0, EOMIndex + 5);

							HandleReceivedMessage(jsonMessage);
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
			try
			{
				IMessage iMsgObj = JsonConvert.DeserializeObject<IMessage>(jsonMessage, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, });
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
					case ChangedItemsWhileInitializingMessage C:
						HandleChangedItemsWhileInitializingMessage(C);
						break;
					default:
						break;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void HandleChangedItemsWhileInitializingMessage(ChangedItemsWhileInitializingMessage c)
		{
			foreach (var item in c.items)
				UpdateMonitorForm.UpdateQueueListViewItem(item.QueueItemId, item.OldState, item.NewState);
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
			MessageBox.Show(error.ErrorMessage);
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