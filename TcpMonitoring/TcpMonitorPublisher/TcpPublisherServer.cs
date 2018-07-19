using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;


using TcpMonitoring;
using TcpMonitoring.MessagingObjects;
using TcpMonitoring.QueueingItems;

namespace TcpMonitorPublisher
{
	public class StateObject
	{
		// Client  socket.
		public Socket workSocket = null;
		// Size of receive buffer.
		public const int BufferSize = 1024;
		// Receive buffer.
		public byte[] buffer = new byte[BufferSize];
		// Received data string.
		public StringBuilder sb = new StringBuilder();
	}

	public class TcpPublisherServer : IMonitoringPublisher
	{
		private static readonly IMonitoringPublisher _Instance = new TcpPublisherServer();
		
		private TcpListener _MonitorServer;
		private Socket _clientSocket = null;
		private Thread _heartbeatTask;
		
		private MockQueueItems _mockQueueItems = new MockQueueItems();
		
		private TcpPublisherServer() { }

		public static IMonitoringPublisher Instance => _Instance;

		public void StartServer(string ipStr, int port)
		{
			try
			{
				IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
				_MonitorServer = new TcpListener(endpoint);
				_MonitorServer.Start();
				Console.WriteLine("Publisher Server running and waiting for clients.");
				WaitForClients();
			}
			catch (Exception ex)
			{

			}
		}

		// Connection calls;

		public void WaitForClients()
		{
			Socket monitorClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
			_MonitorServer.BeginAcceptSocket(new AsyncCallback(OnClientConnected), monitorClient);
		}

		public void OnClientConnected(IAsyncResult result)
		{
			Console.WriteLine("Client Connected.");
			_clientSocket = _MonitorServer.EndAcceptSocket(result);

			//_heartbeatTask = NewHeartBeatTask();
			//_heartbeatTask.Start();

			WaitForClients();
			Receive();
		}

		// Sending calls

		private void SendAsync(string data)
		{
			data += "<EOF>";
			if (_clientSocket != null)
			{
				byte[] byteArr = Encoding.ASCII.GetBytes(data);
				_clientSocket.BeginSend(byteArr, 0, byteArr.Length, 0, new AsyncCallback(SendCallback), _clientSocket);
			}
		}
		
		private void Send(string data)
		{
			if (_clientSocket != null)
			{
				byte[] dataBytes = Encoding.ASCII.GetBytes(data);
				_clientSocket.Send(dataBytes);
			}
		}

		private void SendCallback(IAsyncResult result)
		{
			try
			{
				Socket client = (Socket)result.AsyncState;
				int bytesSent = client.EndSend(result);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		// Receiving calls.

		private void Receive()
		{
			try
			{
				StateObject state = new StateObject();
				state.workSocket = _clientSocket;

				_clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
			}
			catch (Exception ex)
			{

			}
		}

		public void ReceiveCallback(IAsyncResult result)
		{
			StateObject state = (StateObject)result.AsyncState;
			Socket handler = state.workSocket;

			if (handler.Connected)
			{
				int bytesRead = handler.EndReceive(result);
				
				state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
				string msg = state.sb.ToString();
				HandleMessage(msg);
				Receive();
			}
		}

		// Received Message Handling

		private void HandleMessage(string jsonMsg)
		{
			try
			{
				IMessage message = JsonConvert.DeserializeObject<IMessage>(jsonMsg, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });

				switch (message)
				{
					case UnsubscribeMessageObject U:
						HandleUnsubscribeMessage(U);
						break;
					case InitalizeMessage I:
						HandleInitializeMessage(I);
						break;
					default:
						break;
				}
			}
			catch (Exception ex)
			{

			}
		}

		private void HandleInitializeMessage(InitalizeMessage i)
		{
			QueueItemsMessage queueItems = new QueueItemsMessage();
			queueItems.Data = "All current queue items";
			queueItems.QueueItems = _mockQueueItems.mockItems;

			string queueItemsJson = SerializeObjectMessage(queueItems);

			SendAsync(queueItemsJson);
		}

		private void HandleUnsubscribeMessage(IMessage msg)
		{
			Console.WriteLine("\nunsubscribed");
			Close();
		}

		// Helper methods

		private Thread NewHeartBeatTask()
		{
			return new Thread(() =>
			{
				while (true)
				{
					SendHeartBeat();
					Thread.Sleep(1000);
				}
			});
		}

		private string SerializeObjectMessage(IMessage obj)
		{
			return JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
		}

		private string SerializeHeartbeat()
		{
			try
			{
				HeartbeatObject heartbeat = new HeartbeatObject();
				heartbeat.Data = "Heartbeat data";
				return JsonConvert.SerializeObject(heartbeat, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
			}
			catch (Exception ex)
			{
				throw new Exception();
			}
		}

		public void SendErrorMessage(IMessage errorMessage)
		{
			SendAsync(SerializeMonitorMessage(errorMessage));
		}

		public void SendObject(IMessage message)
		{
			SendAsync(SerializeObjectMessage(message));
		}

		public void SendHeartBeat()
		{
			Console.WriteLine("Heartbeat sent");
			Send(SerializeHeartbeat());
		}

		private string SerializeMonitorMessage(IMessage message)
		{
			return JsonConvert.SerializeObject(message, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
		}

		public void Close()
		{
			_heartbeatTask.Abort();
			_clientSocket.Close();
			_clientSocket = null;
		}
	}
}
