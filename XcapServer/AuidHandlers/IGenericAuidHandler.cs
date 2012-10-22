using System;
using Base.Message;
using Http.Message;

namespace Server.Xcap
{
	interface IGenericAuidHandler
		: IAuidHandler
	{
		HttpMessageWriter ProcessGlobal();

		HttpMessageWriter ProcessGetItem(ByteArrayPart item);
		HttpMessageWriter ProcessPutItem(ByteArrayPart item, ArraySegment<byte> content);
		HttpMessageWriter ProcessDeleteItem(ByteArrayPart item);
	}
}
