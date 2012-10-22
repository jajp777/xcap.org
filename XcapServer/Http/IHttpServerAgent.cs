using System;
using Base.Message;
using Http.Message;
using SocketServers;

namespace Server.Http
{
	static class HttpServerAgent
	{
		public struct IsHandledResult
		{
			public bool IsHandled;
			public ByteArrayPart Realm;

			public IsHandledResult(bool isHandled)
			{
				IsHandled = isHandled;
				Realm = ByteArrayPart.Invalid;
			}

			public IsHandledResult(bool isHandled, ByteArrayPart realm)
			{
				IsHandled = isHandled;
				Realm = realm;
			}

			public bool IsAuthorizationRequred
			{
				get { return Realm.IsValid; }
			}

			public static IsHandledResult NotHandle()
			{
				return new IsHandledResult(false);
			}

			public static IsHandledResult Handle()
			{
				return new IsHandledResult(true);
			}

			public static IsHandledResult HandleWithAuthorization(ByteArrayPart realm)
			{
				return new IsHandledResult(true, realm);
			}

			public static implicit operator IsHandledResult(bool isHandled)
			{
				return new IsHandledResult(isHandled);
			}
		}
	}

	interface IHttpServerAgent
		: IDisposable
	{
		IHttpServer IHttpServer { set; }

		HttpServerAgent.IsHandledResult IsHandled(HttpMessageReader reader);
		bool IsAuthorized(HttpMessageReader reader, ByteArrayPart username);
		void HandleRequest(BaseConnection connection, HttpMessageReader httpReader, ArraySegment<byte> httpContent);
	}
}
