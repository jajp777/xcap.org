using System;
using Http.Message;
using SocketServers;

namespace Server.Http
{
	interface IHttpServer
	{
		HttpMessageWriter GetHttpMessageWriter();

		void SendResponse(BaseConnection c, HttpMessageWriter writer);
		void SendResponse(BaseConnection c, ArraySegment<byte> data);
	}
}
