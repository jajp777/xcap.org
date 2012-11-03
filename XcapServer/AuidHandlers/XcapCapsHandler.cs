using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using Base.Message;
using Http.Message;

namespace Server.Xcap
{
	class XcapCapsHandler
		: BaseGenericAuidHandler
	{
		private byte[] response;

		public XcapCapsHandler()
			: base("xcap-caps", "urn:ietf:params:xml:ns:xcap-caps", null)
		{
		}

		public bool IsValid
		{
			get { return response != null; }
		}

		public void Invalidate()
		{
			response = null;
		}

		public void Update(IEnumerable<IAuidHandler> handlers)
		{
			using (var memoryStream = new MemoryStream(2048))
			using (var writer = XmlWriter.Create(memoryStream, new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false), }))
			{
				writer.WriteStartElement(Auid, Namespace);

				writer.WriteStartElement("auids");
				foreach (var handler in handlers)
					writer.WriteElementString("auid", handler.Auid);
				writer.WriteEndElement();

				writer.WriteStartElement("extensions");
				writer.WriteComment(" No extensions defined ");
				writer.WriteEndElement();

				writer.WriteStartElement("namespaces");
				writer.WriteElementString("namespace", "urn:ietf:params:xml:ns:xcap-error");
				foreach (var handler in handlers)
					if (string.IsNullOrEmpty(handler.Namespace) == false)
						writer.WriteElementString("namespace", handler.Namespace);
				writer.WriteEndElement();

				writer.WriteEndElement();

				writer.Flush();
				response = memoryStream.ToArray();
			}
		}

		public override HttpMessageWriter ProcessGlobal()
		{
			var writer = Context.GetWriter();

			writer.WriteStatusLine(StatusCodes.OK);
			writer.WriteContentType(ContentType.ApplicationXcapCapsXml);
			writer.WriteAccessControlAllowOrigin(true);
			writer.WriteContentLength(response.Length);
			writer.WriteCRLF();
			writer.Write(response);

			return writer;
		}

		public override HttpMessageWriter ProcessGetItem(ByteArrayPart item)
		{
			throw new NotImplementedException();
		}
	}
}
