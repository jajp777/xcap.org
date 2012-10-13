using System;
using System.Collections.Generic;
using Http.Message;

namespace XcapServer
{
	interface IAuidHandler
	{
		string Auid { get; }
		string Segment2 { get; }
		string Namespace { get; }

		Func<HttpMessageWriter> GetWritter { set; }

		HttpMessageWriter ProcessGlobal();
		HttpMessageWriter ProcessGetItem(string item);
		HttpMessageWriter ProcessPutItem(string item, ArraySegment<byte> content);
		HttpMessageWriter ProcessDeleteItem(string item);
	}
}
