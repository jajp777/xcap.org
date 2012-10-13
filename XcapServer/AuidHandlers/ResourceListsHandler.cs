using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using Http.Message;

namespace XcapServer
{
	abstract class ResourceListsHandler
		: BaseAuidHandler
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

		public ResourceListsHandler()
			: base("resource-lists", "urn:ietf:params:xml:ns:resource-lists", "users")
		{
		}

		public override HttpMessageWriter ProcessGetItem(string item)
		{
			var content = CreateResourceList(GetEntries());

			var writer = GetWritter();

			writer.WriteStatusLine(StatusCodes.OK);
			writer.WriteContentType(ContentType.ApplicationResourceListsXml);
			writer.WriteContentLength(content.Length);
			writer.WriteCRLF();
			writer.Write(content);

			return writer;
		}

		protected abstract IEnumerable<Entry> GetEntries();

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
