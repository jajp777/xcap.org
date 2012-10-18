using System;
using Http.Message;

namespace XcapServer
{
	interface IAuidHandlerContext
	{
		HttpMessageWriter GetWriter();
	}
}
