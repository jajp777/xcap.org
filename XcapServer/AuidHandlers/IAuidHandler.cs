using System;
using Base.Message;
using Http.Message;

namespace XcapServer
{
	interface IAuidHandler
	{
		string Auid { get; }
		string Segment2 { get; }
		string Namespace { get; }

		IAuidHandlerContext Context { set; }
	}
}
