using System;
using System.Collections.Generic;
using Base.Message;
using Http.Message;

namespace Server.Xcap
{
	class RlsServicesHandler
		: BaseGenericAuidHandler
	{
		public RlsServicesHandler()
			: base("rls-services", "urn:ietf:params:xml:ns:rls-services", "users")
		{
		}

		public override HttpMessageWriter ProcessGlobal()
		{
			var writer = Context.GetWriter();

			writer.WriteStatusLine(StatusCodes.OK);
			writer.WriteContentLength(0);
			writer.WriteCRLF();

			return writer;
		}

		public override HttpMessageWriter ProcessGetItem(ByteArrayPart item)
		{
			throw new NotImplementedException();
		}
	}
}
