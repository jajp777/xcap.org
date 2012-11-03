using System;
using Base.Message;
using Http.Message;

namespace Server.Xcap
{
	interface IUsersAuidHandler
		: IAuidHandler
	{
		HttpMessageWriter ProcessGetItem(ByteArrayPart username, ByteArrayPart domain);
		HttpMessageWriter ProcessPutItem(ByteArrayPart username, ByteArrayPart domain, ArraySegment<byte> content);
		HttpMessageWriter ProcessDeleteItem(ByteArrayPart username, ByteArrayPart domain);
	}
}
