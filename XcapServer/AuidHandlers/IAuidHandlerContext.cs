using System;
using Http.Message;

namespace Server.Xcap
{
	interface IAuidHandlerContext
	{
		HttpMessageWriter GetWriter();
	}
}
