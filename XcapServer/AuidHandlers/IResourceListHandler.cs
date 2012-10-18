﻿using System;
using Base.Message;
using Http.Message;

namespace XcapServer
{
	interface IResourceListHandler
		: IAuidHandler
	{
		HttpMessageWriter ProcessGetItem(ByteArrayPart username, ByteArrayPart domain);
		HttpMessageWriter ProcessPutItem(ByteArrayPart username, ByteArrayPart domain, ArraySegment<byte> content);
		HttpMessageWriter ProcessDeleteItem(ByteArrayPart username, ByteArrayPart domain);
	}
}