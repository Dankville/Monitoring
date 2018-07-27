using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TcpMonitoring.MessagingObjects;


namespace TcpMonitorPublisher
{
	public class HeartBeatPublisher
	{
		private static readonly HeartBeatPublisher _Instance = new HeartBeatPublisher();
		public static HeartBeatPublisher Instance => _Instance;
		
		private HeartBeatPublisher() { }

		private TcpListener _heartBeatPublisher;
		private Socket _heartBeatClient;

		private Thread _heartBeatTask;
		private int _missedHeartbeats = 0;

		public static IPAddress ipAddress { get; set; }
		public static int Port { get; set; }

		public void StartServer(string ipAddrStr, int port)
		{
			if (IPAddress.TryParse(ipAddrStr, out var ipAdd))
			{
				Port = port;
				ipAddress = ipAdd;
				IPEndPoint endpoint = new IPEndPoint(ipAdd, port);
				try
				{
					_heartBeatPublisher = new TcpListener(endpoint);
					_heartBeatPublisher.Start();
					WaitForHeartBeatConnection();
				}
				catch (Exception)
				{

				}
			}
		}

		private void WaitForHeartBeatConnection()
		{
			Console.WriteLine("Waiting for heartbeat client.");
			Socket _heartBeatClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
			_heartBeatPublisher.BeginAcceptSocket(new AsyncCallback(OnHeartBeatClientConnected), _heartBeatClient);
		}

		private void OnHeartBeatClientConnected(IAsyncResult ar)
		{
			if (ar.IsCompleted)
			{
				Console.WriteLine("Heartbeat client connected");
				_heartBeatClient = _heartBeatPublisher.EndAcceptSocket(ar);
				_heartBeatTask = HeartBeatTask();
				Receive();
			}
		}

		private Thread HeartBeatTask()
		{
			var thread = new Thread(() =>
			{
				while (true)
				{
					SendHeartBeat();
					Thread.Sleep(1000);
				}
			});
			thread.Start();
			return thread;
		}

		private void SendHeartBeat()
		{
			if (_missedHeartbeats >= 5)
			{
				// reconnect

				Close();
				return;
			}
			_missedHeartbeats++;

			IMessage hb = new HeartbeatObject() { HeartbeatData = "Heart Beat" };
			string msg = JsonConvert.SerializeObject(hb, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
			byte[] heartbeatBytes = Encoding.ASCII.GetBytes(msg);

			_heartBeatClient.BeginSend(heartbeatBytes, 0, heartbeatBytes.Length, 0, new AsyncCallback(HeartBeatCallback), null);
		}

		private void HeartBeatCallback(IAsyncResult ar)
		{
			if (ar.IsCompleted)
				_missedHeartbeats = 0;
		}

		private void Receive()
		{
			try
			{
				StateObject state = new StateObject();
				state.workSocket = _heartBeatClient;
				_heartBeatClient.BeginReceive(state.buffer, 0, StateObject.BufferSize,0, new AsyncCallback(ReceiveCallback), state);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			try
			{
				if (ar.IsCompleted && _heartBeatClient.Connected)
				{
					StateObject state = (StateObject)ar.AsyncState;
					Socket client = state.workSocket;
					int bytesRead = client.EndReceive(ar);
					state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
					string msg = state.sb.ToString();

					HandleReceivedMessage(msg);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void HandleReceivedMessage(string jsonMsg)
		{
			try
			{
				IMessage message = JsonConvert.DeserializeObject<IMessage>(jsonMsg, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });

				switch (message)
				{
					case UnsubscribeMessageObject U:
						HandleUnsubscribeMessage();
						break;
					default:
						break;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void HandleUnsubscribeMessage()
		{
			Close();
		}

		public void Close()
		{
			Console.WriteLine("Heartbeat client disconnected \n");
			_heartBeatTask.Abort();
			_heartBeatClient.Close();
			WaitForHeartBeatConnection();
		}
	}
}
