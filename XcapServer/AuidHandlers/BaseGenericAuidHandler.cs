using System;
using Base.Message;
using Http.Message;

namespace Server.Xcap
{
	class BaseGenericAuidHandler
		: BaseAuidHandler
		, IGenericAuidHandler
	{
		public BaseGenericAuidHandler(string auid, string @namespace, string segment2)
			: base(auid, @namespace, segment2)
		{
		}

		public virtual HttpMessageWriter ProcessGlobal()
		{
			return null;
		}

		public virtual HttpMessageWriter ProcessGetItem(ByteArrayPart item)
		{
			return null;
		}

		public virtual HttpMessageWriter ProcessPutItem(ByteArrayPart item, ArraySegment<byte> content)
		{
			return CreateErrorResponse(XcapErrors.CannotInsert, "Not Implemented");
		}

		public virtual HttpMessageWriter ProcessDeleteItem(ByteArrayPart item)
		{
			return CreateErrorResponse(XcapErrors.CannotDelete, "Not Implemented");
		}
	}
}
