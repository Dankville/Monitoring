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
		private Thread _heartBeatChecker;

		public Socket _publisherTcpClient;
		public Socket _heartbeatTcpClient;
		public int _MissedHeartBeats = 0;
		public bool IsConnected = false;
		
		// Items in the actual queue
		public QueueItemsHandler queueItemsHandler;

		private string _lastReceivedMessage;

		// Singleton
		private static TcpPublisherClient _Instance = null;
		public static TcpPublisherClient Instance()
		{
			if (_Instance == null)
			{
				_Instance = new TcpPublisherClient();
			}
			return _Instance;
		}

		private TcpPublisherClient()
		{
			queueItemsHandler = new QueueItemsHandler();
		}

		public void BeginConnect(IPAddress ipAddress, int port)
		{
			if (!IsConnected)
			{
				_publisherTcpClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
				_publisherTcpClient.BeginConnect(ipAddress, port, new AsyncCallback(ConnectCallback), null);
			}
		}

		private void ConnectCallback(IAsyncResult result)
		{
			try
			{
				_publisherTcpClient.EndConnect(result);

				if (result.IsCompleted)
				{
					IsConnected = true;
					Console.WriteLine("Connected");

					Receive();

					_heartBeatChecker = HeartbeatChecker();
					//_heartBeatChecker.Start();
				}
			}
			catch (Exception)
			{
				MessageBox.Show("Could not make a connection to the machine with those credentials.");
			}
		}

		public void Unsubscribe()
		{
			StateObject state = new StateObject();
			state.workSocket = _publisherTcpClient;
			_publisherTcpClient.BeginDisconnect(false, UnSubscribeCallback, state);
		}

		public void Send(IMessage message)
		{
			try
			{
				string msgJson = JsonConvert.SerializeObject(message, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
				byte[] byteData = Encoding.ASCII.GetBytes(msgJson);
				switch (message)
				{
					case UnsubscribeMessageObject U:
						_publisherTcpClient.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(UnSubscribeCallback), _publisherTcpClient);
						break;
					case ErrorMessageObject E:
						_publisherTcpClient.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(ErrorMessageCallback), _publisherTcpClient);
						break;
					default:
						break;
				}

			}
			catch (Exception ex)
			{
				// show exception in monitoring form
				Console.WriteLine(ex.Message);
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

		private void UnSubscribeCallback(IAsyncResult result)
		{
			if (result.IsCompleted)
			{
				IsConnected = false;
				_heartBeatChecker.Abort();
				_publisherTcpClient = null;
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
				state.workSocket = _publisherTcpClient;

				_publisherTcpClient.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
			}
			catch (SocketException ex)
			{
				// Publisher was closed.
				Console.WriteLine(ex.Message);
			}
		}

		private void ReceiveCallback(IAsyncResult result)
		{
			if (result.IsCompleted)
			{
				StateObject state = (StateObject)result.AsyncState;
				Socket client = state.workSocket;
				int bytesRead = client.EndReceive(result);

				state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
				_lastReceivedMessage += state.sb.ToString();
				// Check if last part of send message.
				if (_lastReceivedMessage.IndexOf("<EOF>") != -1)
				{
					HandleMessage(_lastReceivedMessage);
				}
				
				Receive();
			}
			else
			{
				throw new Exception();
			}
		}

		private void HandleMessage(string jsonMessage)
		{
			_lastReceivedMessage = "";
			try
			{
				jsonMessage = jsonMessage.Remove(jsonMessage.Length - 5);
				IMessage message = JsonConvert.DeserializeObject<IMessage>(jsonMessage, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

				switch (message)
				{
					case HeartbeatObject H:
						HandleHeartBeatMessage(H);
						break;
					case MessageObject M:
						HandleObjectMessage(M);
						break;
					case ErrorMessageObject E:
						HandleErrorMessage(E);
						break;
					case QueueItemsMessage I:
						HandleQueueItemsMessage(I);
						break;
					case QueueItemStateChangeMessage Q:
						HandleQueueItemStateChangeMessage(Q);
						break;
					default:
						break;
				}
			}
			catch (Exception ex)
			{
				// show exception in monitoring form
				Console.WriteLine(ex.Message);
			}
		}

		private void HandleQueueItemStateChangeMessage(QueueItemStateChangeMessage q)
		{
			//QueueItemStateChangedEventHandler handler = queueItemStateChange;
			//handler?.Invoke(q.QueueItemId, q.OldState, q.NewState);
			UpdateQueueListView.UpdateQueueListViewItem(q.QueueItemId, q.OldState, q.NewState);
		}

		private void HandleQueueItemsMessage(QueueItemsMessage i)
		{
			//InitializingQueueItemsEventHandler handler = initialQueueItemsReceived;
			//handler?.Invoke(i.QueueItems);
			UpdateQueueListView.InitializeQueueListView(i.QueueItems);
		}
		

		private void HandleHeartBeatMessage(HeartbeatObject message)
		{
			// do something idk what yet, depends on XIMEx.
			_MissedHeartBeats = 0;
		}

		private void HandleObjectMessage(MessageObject message)
		{
			try
			{
				Console.WriteLine(message.Data);
			}
			catch (Exception ex)
			{
				// show exception in monitoring form
				Console.WriteLine(ex.Message);
			}
		}

		private void HandleErrorMessage(ErrorMessageObject error)
		{
			Console.WriteLine(error.Data);
		}

		private Thread HeartbeatChecker()
		{
			return new Thread(() =>
			{
				while (true)
				{
					try
					{
						Thread.Sleep(1000);

						if (_MissedHeartBeats >= 5)
						{
							Console.WriteLine("5 heartbeats missed, connection closed.");
							Close();
							break;
						}
						_MissedHeartBeats++;
					}
					catch (Exception ex)
					{
						// show exception in monitoring form
						Console.WriteLine(ex.Message);
					}
				}
			});
		}


		private Thread NewReceiveLoop()
		{
			return new Thread(() =>
			{
				try
				{
					Receive();
				}
				catch (Exception ex)
				{
					// show exception in monitoring form
					Console.WriteLine(ex.Message);
				}
			});
		}

		public void Close()
		{
			_publisherTcpClient.Close();
		}
	}
}