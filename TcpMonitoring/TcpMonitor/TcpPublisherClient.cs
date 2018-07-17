using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using TcpMonitoring;
using TcpMonitoring.MessagingObjects;

namespace TcpMonitor
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
		private ManualResetEvent _ConnectDone = new ManualResetEvent(false);
		private ManualResetEvent _SendDone = new ManualResetEvent(false);
		private ManualResetEvent _ReceiveDone = new ManualResetEvent(false);
		
		private Thread _heartBeatChecker;
		private Thread _receiveLoopTask;

		

		public int _MissedHeartBeats = 0;
		public bool IsConnected = false;

		private static TcpPublisherClient _Instance = null;

		public Socket _publisherTcpClient;

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
			Console.WriteLine("Publisher Client Started");
		}

		public void BeginConnect(IPAddress ipAddress, int port)
		{
			_publisherTcpClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
			_publisherTcpClient.BeginConnect(ipAddress, port, new AsyncCallback(ConnectCallback), null);
			_ConnectDone.WaitOne();
		}

		private void ConnectCallback(IAsyncResult result)
		{
			_publisherTcpClient.EndConnect(result);
			_ConnectDone.Set();

			if (result.IsCompleted)
			{
				IsConnected = true;
				Console.WriteLine("Connected");
				_receiveLoopTask = NewReceiveLoop();
				_receiveLoopTask.Start();
				_heartBeatChecker = HeartbeatChecker();
				_heartBeatChecker.Start();
			}
		}

		public void Unsubscribe()
		{
			StateObject state = new StateObject();
			state.workSocket = _publisherTcpClient;
			_publisherTcpClient.BeginDisconnect(false, UnSubscribeCallback, state);
			//Send(new UnsubscribeMessageObject() { Data = "Unsubscribe" });
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

		private void UnSubscribeCallback(IAsyncResult result)
		{
			if (result.IsCompleted)
			{
				IsConnected = false;                    
				_receiveLoopTask.Abort();
				_heartBeatChecker.Abort();
				_publisherTcpClient = null;
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
			try
			{
				StateObject state = (StateObject)result.AsyncState;
				Socket client = state.workSocket;
				int bytesRead = client.EndReceive(result);
				Receive();

				state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
				string msg = state.sb.ToString();

				HandleMessage(msg);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void HandleMessage(string jsonMessage)
		{
			try
			{
				IMessage message = JsonConvert.DeserializeObject<IMessage>(jsonMessage, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All});
				
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
			return new Thread(() => {
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
