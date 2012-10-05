using System;
using System.Collections.Generic;
using Http.Message;
using Xcap.PathParser;

namespace XcapServer
{
	class XcapServer
	{
		[ThreadStatic]
		private static XcapPathParser pathParser;

		public delegate void SendAsyncDelegate(HttpConnection c, HttpMessageWriter writer);
		public SendAsyncDelegate SendAsync;

		public void ProcessIncomingMessage(HttpConnection c, out bool closeConnection)
		{
			InitializeXcapPathParser();

			closeConnection = false;

			StatusCodes statusCode;

			if (pathParser.ParseAll(c.HttpReader.RequestUri.ToArraySegment()))
			{
				pathParser.SetArray(c.HttpReader.RequestUri.Bytes);

				Console.Write("/ {0} / {1}", pathParser.Auid.ToString(), pathParser.Usage.ToString());
				if (pathParser.Username.IsValid)
					Console.Write(" / {0}", pathParser.Username.ToString());
				Console.WriteLine(" / {0}", pathParser.DocumentNameId.ToString());

				statusCode = StatusCodes.OK;
			}
			else
			{
				statusCode = StatusCodes.NotFound;
			}

			using (var writer = new HttpMessageWriter())
			{
				writer.WriteStatusLine(statusCode);
				writer.WriteContentLength(0);
				writer.WriteCRLF();

				SendAsync(c, writer);
			}
		}

		private void InitializeXcapPathParser()
		{
			if (pathParser == null)
				pathParser = new XcapPathParser();

			pathParser.SetDefaultValue();
		}
	}
}
