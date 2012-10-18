using System;
using System.IO;
using System.Text;
using System.Timers;
using System.Collections.Generic;
using System.Security.Cryptography;
using Http.Message;
using SocketServers;
using XcapServer;

namespace Http.Server
{
	class HttpServer
		: IDisposable
		, IHttpServer
		, IHttpServerAgentRegistrar
	{
		private IHttpServerAgent xcapServer;

		public Action<ServerAsyncEventArgs> SendAsync;

		public HttpServer()
		{
		}

		public void Dispose()
		{
			xcapServer.Dispose();
		}

		IHttpServer IHttpServerAgentRegistrar.Register(IHttpServerAgent agent, int priority)
		{
			xcapServer = agent;
			return this;
		}

		public void ProcessIncomingRequest(HttpConnection c)
		{
			if (xcapServer.IsHandled(c.HttpReader))
				xcapServer.HandleRequest(c, c.HttpReader, c.Content);
		}

		HttpMessageWriter IHttpServer.GetHttpMessageWriter()
		{
			return new HttpMessageWriter();
		}

		void IHttpServer.SendResponse(BaseConnection c, HttpMessageWriter writer)
		{
			var r = EventArgsManager.Get();
			r.CopyAddressesFrom(c);
			r.Count = writer.Count;
			r.OffsetOffset = writer.OffsetOffset;
			r.AttachBuffer(writer.Detach());

			SendAsync(r);
		}

		void IHttpServer.SendResponse(BaseConnection c, ArraySegment<byte> data)
		{
			int offset = data.Offset, left = data.Count;

			while (left > 0)
			{
				var r = EventArgsManager.Get();
				r.CopyAddressesFrom(c);
				r.OffsetOffset = r.MinimumRequredOffsetOffset;
				r.AllocateBuffer();
				r.SetMaxCount();

				if (r.Count > left)
					r.Count = left;

				r.BlockCopyFrom(new ArraySegment<byte>(data.Array, offset, r.Count));

				offset += r.Count;
				left -= r.Count;

				SendAsync(r);
			}
		}
	}
}
