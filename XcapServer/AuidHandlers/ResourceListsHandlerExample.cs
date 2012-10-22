using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using Base.Message;
using Http.Message;

namespace Server.Xcap
{
	class ResourceListsHandlerExample
		: BaseResourceListsHandler
	{
		public ResourceListsHandlerExample()
		{
		}

		protected override IEnumerable<Entry> GetEntries(ByteArrayPart username, ByteArrayPart domain)
		{
			return Generate(20);
		}

		private IEnumerable<Entry> Generate(int count)
		{
			yield return new Entry("sip:jitsi@officesip.local", "Jitsi");

			for (int i = 0; i < count; i++)
			{
				var id = string.Format("{0:d4}", i);
				yield return new Entry("sip:user" + id + "@officesip.local", "User #" + id);
			}
		}
	}
}
