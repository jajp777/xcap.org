using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using Base.Message;
using Http.Message;

namespace Server.Xcap
{
	abstract class BasePidfManipulationHandler
		: BaseAuidHandler
		, IUsersAuidHandler
	{
		public BasePidfManipulationHandler()
			: base("pidf-manipulation", "urn:ietf:params:xml:ns:pidf", "users")
		{
		}

		public abstract HttpMessageWriter ProcessGetItem(ByteArrayPart username, ByteArrayPart domain);

		public virtual HttpMessageWriter ProcessPutItem(ByteArrayPart username, ByteArrayPart domain, HttpMessageReader reader, ArraySegment<byte> content)
		{
			return CreateErrorResponse(XcapErrors.CannotInsert, "Not Implemented");
		}

		public virtual HttpMessageWriter ProcessDeleteItem(ByteArrayPart username, ByteArrayPart domain)
		{
			return CreateErrorResponse(XcapErrors.CannotDelete, "Not Implemented");
		}
	}
}
