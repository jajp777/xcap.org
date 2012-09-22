using System;
using System.Collections.Generic;
using Http.Message;

namespace XcapServer
{
	class XcapServer
	{
		public delegate void SendAsyncDelegate(HttpConnection c, HttpMessageWriter writer);
		public SendAsyncDelegate SendAsync;

		public void ProcessIncomingMessage(HttpConnection c, out bool closeConnection)
		{
			closeConnection = false;

			using (var writer = new HttpMessageWriter())
			{
				writer.WriteStatusLine(StatusCodes.NotFound);
				//writer.WriteConnectionClose();
				writer.WriteContentLength(0);
				writer.WriteCRLF();

				SendAsync(c, writer);
			}
		}
	}
}
