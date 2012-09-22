using System;
using System.Collections.Generic;
using SocketServers;
using Http.Message;

namespace XcapServer
{
	class HttpConnection
		: HeaderContentConnection
		, IDisposable
	{
		[ThreadStatic]
		private static HttpMessageReader httpReader;

		public HttpConnection()
		{
		}

		void IDisposable.Dispose()
		{
		}

		public HttpMessageReader HttpReader
		{
			get { return httpReader; }
		}

		protected override void ResetParser(ResetReason reason)
		{
			InitializeHttpReader(true);
		}

		protected override void MessageReady()
		{
			httpReader.SetArray(base.Header.Array);
		}

		protected override void PreProcessRaw(ArraySegment<byte> data)
		{
		}

		protected override ParseResult Parse(ArraySegment<byte> data)
		{
			InitializeHttpReader(false);

			int headerLength = httpReader.Parse(data.Array, data.Offset, data.Count);

			if (httpReader.IsFinal)
				return ParseResult.HeaderDone(headerLength, httpReader.HasContentLength ? httpReader.ContentLength : 0);

			if (httpReader.IsError)
				return ParseResult.Error();

			return ParseResult.NotEnoughData();
		}

		private void InitializeHttpReader(bool forceDefaultValue)
		{
			if (httpReader == null)
			{
				httpReader = new HttpMessageReader();
				httpReader.SetDefaultValue();
			}
			else if (forceDefaultValue)
			{
				httpReader.SetDefaultValue();
			}
		}
	}
}
