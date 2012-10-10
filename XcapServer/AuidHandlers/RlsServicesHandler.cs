using System;
using System.Collections.Generic;
using Http.Message;

namespace XcapServer
{
	class RlsServicesHandler
		: BaseAuidHandler
	{
		public RlsServicesHandler()
			: base("rls-services", "urn:ietf:params:xml:ns:rls-services", "users")
		{
		}

		public override HttpMessageWriter ProcessGlobal()
		{
			var writer = GetWritter();

			writer.WriteStatusLine(StatusCodes.OK);
			writer.WriteContentLength(0);
			writer.WriteCRLF();

			return writer;
		}

		public override HttpMessageWriter ProcessGetItem(string item)
		{
			throw new NotImplementedException();
		}
	}
}
