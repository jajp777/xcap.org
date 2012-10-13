using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using Http.Message;

namespace XcapServer
{
	abstract class BaseAuidHandler
		: IAuidHandler
	{
		public BaseAuidHandler(string auid, string @namespace, string segment2)
		{
			this.Auid = auid;
			this.Namespace = @namespace;
			this.Segment2 = segment2;
		}

		public string Auid { get; private set; }
		public string Namespace { get; private set; }
		public string Segment2 { get; private set; }

		public Func<HttpMessageWriter> GetWritter { protected get; set; }

		public virtual HttpMessageWriter ProcessGlobal()
		{
			return null;
		}

		public virtual HttpMessageWriter ProcessGetItem(string item)
		{
			return null;
		}

		public virtual HttpMessageWriter ProcessPutItem(string item, ArraySegment<byte> content)
		{
			return CreateErrorResponse(XcapErrors.CannotInsert, "Not Implemented");
		}

		public virtual HttpMessageWriter ProcessDeleteItem(string item)
		{
			return CreateErrorResponse(XcapErrors.CannotDelete, "Not Implemented");
		}

		protected HttpMessageWriter CreateErrorResponse(XcapErrors error, string phrase)
		{
			return CreateResponse(StatusCodes.Conflict, ContentType.ApplicationXcapErrorXml, CreateErrorContent(XcapErrors.CannotInsert, phrase));
		}

		protected HttpMessageWriter CreateResponse(StatusCodes statusCodes, ContentType contentType, byte[] content)
		{
			var writer = GetWritter();

			writer.WriteStatusLine(statusCodes);
			writer.WriteContentType(contentType);
			writer.WriteContentLength(content.Length);
			writer.WriteCRLF();
			writer.Write(content);

			return writer;
		}

		protected byte[] CreateErrorContent(XcapErrors error, string phrase)
		{
			using (var memoryStream = new MemoryStream(2048))
			using (var writer = XmlWriter.Create(memoryStream, new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false), }))
			{
				writer.WriteStartElement("xcap-error", "urn:ietf:params:xml:ns:xcap-error");

				writer.WriteStartElement(error.Convert());
				if (string.IsNullOrEmpty(phrase) == false)
					writer.WriteAttributeString("phrase", phrase);
				writer.WriteEndElement();

				writer.WriteEndElement();

				writer.Flush();

				var result = Encoding.UTF8.GetString(memoryStream.ToArray());

				return memoryStream.ToArray();
			}
		}
	}
}
