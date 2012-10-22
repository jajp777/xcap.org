using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using Base.Message;
using Http.Message;

namespace Server.Xcap
{
	abstract class BaseResourceListsHandler
		: BaseAuidHandler
		, IResourceListHandler
	{
		#region struct Entry {...}

		protected struct Entry
		{
			public Entry(string uri)
				: this(uri, null)
			{
			}

			public Entry(string uri, string displayName)
			{
				this.Uri = uri;
				this.DisplayName = displayName;
			}

			public readonly string Uri;
			public readonly string DisplayName;
		}

		#endregion

		public BaseResourceListsHandler()
			: base("resource-lists", "urn:ietf:params:xml:ns:resource-lists", "users")
		{
		}

		public HttpMessageWriter ProcessGetItem(ByteArrayPart username, ByteArrayPart domain)
		{
			var content = CreateResourceList(GetEntries(username, domain));

			var writer = Context.GetWriter();

			writer.WriteStatusLine(StatusCodes.OK);
			writer.WriteContentType(ContentType.ApplicationResourceListsXml);
			writer.WriteContentLength(content.Length);
			writer.WriteCRLF();
			writer.Write(content);

			return writer;
		}

		public HttpMessageWriter ProcessPutItem(ByteArrayPart username, ByteArrayPart domain, ArraySegment<byte> content)
		{
			return CreateErrorResponse(XcapErrors.CannotInsert, "Not Implemented");
		}

		public HttpMessageWriter ProcessDeleteItem(ByteArrayPart username, ByteArrayPart domain)
		{
			return CreateErrorResponse(XcapErrors.CannotDelete, "Not Implemented");
		}

		protected abstract IEnumerable<Entry> GetEntries(ByteArrayPart username, ByteArrayPart domain);

		private byte[] CreateResourceList(IEnumerable<Entry> list)
		{
			using (var memoryStream = new MemoryStream(4096))
			using (var writer = XmlWriter.Create(memoryStream, new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false), }))
			{
				writer.WriteStartElement(Auid, Namespace);

				writer.WriteStartElement("list");
				writer.WriteAttributeString("name", "RootGroup");

				foreach (var entry in list)
				{
					writer.WriteStartElement("entry");
					writer.WriteAttributeString("uri", entry.Uri);

					if (string.IsNullOrEmpty(entry.DisplayName) == false)
						writer.WriteElementString("display-name", entry.DisplayName);

					writer.WriteEndElement();
				}

				//writer.WriteStartElement("list");
				//writer.WriteAttributeString("name", "All Contacts");
				//writer.WriteStartElement("entry");
				//writer.WriteAttributeString("uri", "sip:test@officesip.local");
				//writer.WriteEndElement();
				//writer.WriteEndElement();

				writer.WriteEndElement(); // list
				writer.WriteEndElement(); // auid

				writer.Flush();

				return memoryStream.ToArray();
			}
		}
	}
}
