using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SocketServers;
using Http.Message;
using Server.Http;
using Server.Xcap;
using Xcap.PathParser;

namespace Server.Xcap
{
	class Program
	{
		private static ServersManager<HttpConnection> serversManager;
		private static XcapServer xcapServer;
		private static HttpServer httpServer;

		static void Main(string[] args)
		{
			var exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

			/////////////////////////////////////////////////////////////////////////

			serversManager = new ServersManager<HttpConnection>(new ServersManagerConfig());

			serversManager.Bind(new ProtocolPort() { Protocol = ServerProtocol.Tcp, Port = 8080, });
			serversManager.ServerAdded += ServersManager_ServerAdded;
			serversManager.ServerRemoved += ServersManager_ServerRemoved;
			serversManager.ServerInfo += ServersManager_ServerInfo;
			serversManager.NewConnection += ServersManager_NewConnection;
			serversManager.EndConnection += ServersManager_EndConnection;

			serversManager.Received += ServersManager_Received;
			serversManager.Sent += ServersManager_Sent;

			serversManager.Logger.Enable(exePath + @"\Log.pcap");

			/////////////////////////////////////////////////////////////////////////

			HttpMessage.BufferManager = new BufferManagerProxy();

			/////////////////////////////////////////////////////////////////////////

			Console.WriteLine(@"Loading DFA table...");

			HttpMessageReader.LoadTables(exePath + @"\Http.Message.dfa");
			XcapPathParser.LoadTables(exePath);

			/////////////////////////////////////////////////////////////////////////

			xcapServer = new XcapServer();
			xcapServer.AddHandler(new ResourceListsHandlerExample());
			//xcapServer.AddHandler(new RlsServicesHandler());

			/////////////////////////////////////////////////////////////////////////

			httpServer = new HttpServer();
			httpServer.SendAsync = serversManager.SendAsync;
			(httpServer as IHttpServerAgentRegistrar).Register(xcapServer, 0, true);

			/////////////////////////////////////////////////////////////////////////

			Console.WriteLine(@"Starting...");

			try
			{
				serversManager.Start();
				Console.WriteLine(@"Started!");
			}
			catch (Exception ex)
			{
				Console.WriteLine(@"Failed to start");
				Console.WriteLine(@"Error: {0}", ex.Message);
			}

			/////////////////////////////////////////////////////////////////////////

			Console.WriteLine(@"Press any key to stop server...");
			Console.ReadKey(true);
			Console.WriteLine();
		}

		static bool ServersManager_Received(ServersManager<HttpConnection> s, HttpConnection connection, ref ServerAsyncEventArgs e)
		{
			bool closeConnection = false, repeat = true;

			while (repeat)
			{
				repeat = connection.Proccess(ref e, out closeConnection);

				if (connection.IsMessageReady)
				{
					httpServer.ProcessIncomingRequest(connection);
					connection.ResetState();
				}
			}

			return !closeConnection;
		}

		static void ServersManager_Sent(ServersManager<HttpConnection> s, ref ServerAsyncEventArgs e)
		{
			if (e.SocketError != SocketError.Success)
				Console.WriteLine("Sent error: {0}", e.SocketError.ToString());
		}

		static void ServersManager_ServerRemoved(object sender, ServerChangeEventArgs e)
		{
			Console.WriteLine(@"  - Removed: {0}", e.ServerEndPoint.ToString());
		}

		static void ServersManager_ServerAdded(object sender, ServerChangeEventArgs e)
		{
			Console.WriteLine(@"  -   Added: {0}", e.ServerEndPoint.ToString());
		}

		static void ServersManager_ServerInfo(object sender, ServerInfoEventArgs e)
		{
			Console.WriteLine(@"  -    Info: [ {0} ] {1}", e.ServerEndPoint.ToString(), e.ToString());
		}

		static void ServersManager_NewConnection(ServersManager<HttpConnection> s, BaseConnection e)
		{
			Console.WriteLine(@"     New Connection: {0}", e.RemoteEndPoint.ToString(), e.ToString());
		}

		static void ServersManager_EndConnection(ServersManager<HttpConnection> s, BaseConnection e)
		{
			Console.WriteLine(@"  Connection Closed: {0}", e.RemoteEndPoint.ToString(), e.ToString());
		}
	}
}
