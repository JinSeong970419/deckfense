using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using GoblinGames;
using GoblinGames.Collections;
using GoblinGames.Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace Deckfense
{
	public class Client : MonoBehaviour
	{
		public static Client Instance { get; private set; }

		[SerializeField] private string serverIP;
		[SerializeField] private int port;
		[SerializeField] private bool autoConnection = true;
		[SerializeField] private int receiveBufferSize = 1024;
		[SerializeField] private int packetPoolSize = 1000;
		[SerializeField] private GameEvent<object> OnReceive;
		[SerializeField] private GameEvent<object> OnConnectEvent;

		private Socket socket;
		private MemoryPool<SocketAsyncEventArgs> readWritePool;
		private IPEndPoint remoteEndPoint;

		private MemoryPool<Packet> packetPool;
		private RingBuffer recvBuffer;

		public bool IsConnected { get { return socket == null ? false : socket.Connected; } }

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;

				recvBuffer = new RingBuffer(receiveBufferSize);
				packetPool = new MemoryPool<Packet>(0);
				for (int i = 0; i < packetPoolSize; i++)
				{
					Packet packet = new Packet(receiveBufferSize);
					packetPool.Free(packet);
				}

				remoteEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), port);
				readWritePool = new MemoryPool<SocketAsyncEventArgs>(0);

				SocketAsyncEventArgs readWriteEventArg;
				for (int i = 0; i < ushort.MaxValue; i++)
				{
					readWriteEventArg = new SocketAsyncEventArgs();
					readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);

					readWritePool.Free(readWriteEventArg);
				}

				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socket.LingerState = new LingerOption(true, 0);
				socket.NoDelay = true;

				DontDestroyOnLoad(this);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		private void Start()
		{


			if (autoConnection)
			{
				ConnectAsync();
			}
		}

		private void OnApplicationQuit()
		{
			Disconnect();
		}

		public void ConnectAsync()
		{
			if (socket.Connected) return;
			Debug.Log("Try Connect...");
			SocketAsyncEventArgs args = new SocketAsyncEventArgs();
			args.Completed += ConnectCompleted;
			args.RemoteEndPoint = remoteEndPoint;
			bool pending = socket.ConnectAsync(args);
			if (!pending)
			{
				ProcessConnect(args);
			}
		}

		private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
		{
			ProcessConnect(e);
		}

		private void ProcessConnect(SocketAsyncEventArgs e)
		{
			SocketError error = e.SocketError;
			if (e.SocketError == SocketError.Success)
			{

				OnConnectEvent.Invoke(error);
				Debug.Log($"Connection success.");

				SocketAsyncEventArgs readEventArgs = readWritePool.Allocate();
				if(readEventArgs == null)
				{
					readEventArgs = new SocketAsyncEventArgs();
				}
				readEventArgs.SetBuffer(recvBuffer.Buffer, recvBuffer.Rear, recvBuffer.WritableLength);
				bool pending = socket.ReceiveAsync(readEventArgs);
				if (!pending)
				{
					ProcessReceive(readEventArgs);
				}
			}
			else
			{
				OnConnectEvent.Invoke(error);
				Debug.Log($"Connect Failed!!");
			}
		}

		private void IO_Completed(object sender, SocketAsyncEventArgs e)
		{
			switch (e.LastOperation)
			{
				case SocketAsyncOperation.Receive:
					ProcessReceive(e);
					break;
				case SocketAsyncOperation.Send:
					ProcessSend(e);
					break;
				default:
					Disconnect();
					Debug.Log($"Invalid Packet.");
					break;
			}
		}

		private void ProcessReceive(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
			{
				recvBuffer.MoveRear(e.BytesTransferred);

				ProcessPacket();

				if (recvBuffer.WritableLength == 0)
				{
					recvBuffer.Resize(recvBuffer.BufferSize * 2);
				}
				e.SetBuffer(recvBuffer.Buffer, recvBuffer.Rear, recvBuffer.WritableLength);
				bool pending = socket.ReceiveAsync(e);
				if (!pending)
				{
					ProcessReceive(e);
				}
			}
			else if (e.BytesTransferred == 0)
			{
				Debug.Log($"정상 종료.");
				readWritePool.Free(e);
				Disconnect();
			}
			else
			{
				Debug.Log($"Receicve Failed! SocketError.{e.SocketError}");
				readWritePool.Free(e);
				Disconnect();
			}
		}

		private void ProcessPacket()
		{
			NetHeader header;
			header.Code = 0;
			header.Length = 0;
			int headerSize = Marshal.SizeOf(header);
			int size;

			while (true)
			{
				size = recvBuffer.Length;
				if (size < headerSize) break;
				recvBuffer.Peek<NetHeader>(ref header);
				if (header.Code != Packet.CODE)
				{
					Debug.LogError($"패킷의 코드가 일치하지 않습니다. Code : {header.Code}");
					break;
				}
				if (size < headerSize + header.Length) break;

				Packet packet = packetPool.Allocate();
				if(packet == null )
				{
					packet = new Packet();
				};
				packet.Initialize();
				if (packet.WritableLength < header.Length)
				{
					packet.Resize(header.Length);
				}

				recvBuffer.MoveFront(headerSize);

				byte[] buf = new byte[header.Length];
				recvBuffer.Read(ref buf, packet.Rear, header.Length);
				packet.Write(buf, 0, buf.Length);

				string typeName = string.Empty;
				string json = string.Empty;
				packet.Read(ref typeName);
				packet.Read(ref json);

				packetPool.Free(packet);

				Debug.Log(typeName);
				Debug.Log(json);
				Assembly assem = Assembly.GetAssembly(typeof(Protocol.Network.Message));
				string assemblyQualifiedName = Assembly.CreateQualifiedName(assem.FullName, typeName);

				Type type = Type.GetType(assemblyQualifiedName);
				if (type == null)
				{
					Debug.Log("type is null.");
				}
				object msg = JsonConvert.DeserializeObject(json, type);

				OnReceive.Invoke(msg);
			}
		}

		private void ProcessSend(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				Packet packet = (Packet)e.UserToken;
				packetPool.Free(packet);
			}
			else if (e.SocketError == SocketError.IOPending)
			{

			}
			else
			{
				Debug.Log($"Send Failed. SocketError : {e.SocketError}");
				Packet packet = (Packet)e.UserToken;
				packetPool.Free(packet);
				readWritePool.Free(e);
				Disconnect();
			}
		}

		public void Send(object data)
		{
			Packet packet = packetPool.Allocate();
            if (packet == null)
            {
                packet = new Packet();
            };
            packet.Initialize();

			string typeName = data.GetType().FullName;
			string json = JsonConvert.SerializeObject(data);
			packet.Write(typeName);
			packet.Write(json);

			packet.SetHeader();
			SocketAsyncEventArgs args = readWritePool.Allocate();
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
            };
            args.UserToken = packet;
			args.SetBuffer(packet.Buffer, packet.Front, packet.Length);
			bool pending = socket.SendAsync(args);
			if (!pending)
			{
				ProcessSend(args);
			}
		}

		public void Disconnect()
		{
			if (socket != null)
			{
				socket.Shutdown(SocketShutdown.Both);
				socket.Disconnect(false);
				socket.Close(5);
				socket.Dispose();
				socket = null;
				Debug.Log("Disconnected.");
			}
		}

		public string GetPublicIPAddress()
		{
			var request = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");

			request.UserAgent = "curl"; // this will tell the server to return the information as if the request was made by the linux "curl" command

			string publicIPAddress;

			request.Method = "GET";
			using (WebResponse response = request.GetResponse())
			{
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					publicIPAddress = reader.ReadToEnd();
				}
			}

			return publicIPAddress.Replace("\n", "");
		}

		public string GetLocalIPAddress()
		{
			string localIP;
			using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
			{
				socket.Connect("8.8.8.8", 65530);
				IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
				localIP = endPoint.Address.ToString();
			}
			return localIP;
		}
	}
}
