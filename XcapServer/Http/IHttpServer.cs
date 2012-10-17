using System;
using Http.Message;
using SocketServers;

namespace Http.Server
{
	interface IHttpServer
	{
		HttpMessageWriter GetHttpMessageWriter();

		void SendResponse(BaseConnection c, HttpMessageWriter writer);
		void SendResponse(BaseConnection c, ArraySegment<byte> data);
	}
}
