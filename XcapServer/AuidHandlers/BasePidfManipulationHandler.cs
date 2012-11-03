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

		//protected abstract byte[] GetPidf(ByteArrayPart username, ByteArrayPart domain);

		//public HttpMessageWriter ProcessGetItem(ByteArrayPart username, ByteArrayPart domain)
		//{
		//    var pidf = GetPidf(username, domain);

		//    return (pidf == null)
		//        ? base.CreateResponse(StatusCodes.NotFound)
		//        : base.CreateResponse(StatusCodes.OK, ContentType.ApplicationPidfXml, GetPidf(username, domain));
		//}

		public abstract HttpMessageWriter ProcessGetItem(ByteArrayPart username, ByteArrayPart domain);

		public HttpMessageWriter ProcessPutItem(ByteArrayPart username, ByteArrayPart domain, ArraySegment<byte> content)
		{
			return CreateErrorResponse(XcapErrors.CannotInsert, "Not Implemented");
		}

		public HttpMessageWriter ProcessDeleteItem(ByteArrayPart username, ByteArrayPart domain)
		{
			return CreateErrorResponse(XcapErrors.CannotDelete, "Not Implemented");
		}
	}
}
