using System;
using Http.Message;
using SocketServers;

namespace Http.Server
{
	interface IHttpServerAgent
		: IDisposable
	{
		bool IsHandled(HttpMessageReader httpReader);
		void HandleRequest(BaseConnection connection, HttpMessageReader httpReader, ArraySegment<byte> httpContent);
	}
}
