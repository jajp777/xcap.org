using System;
using System.Collections.Generic;
using Base.Message;
using Http.Message;
using Xcap.PathParser;

namespace XcapServer
{
	class XcapServer
	{
		public delegate void SendAsyncDelegate(HttpConnection c, HttpMessageWriter writer);

		[ThreadStatic]
		private static XcapPathParser pathParser;
		private List<IAuidHandler> handlers;
		private XcapCapsHandler xcapCapsHander;
		private SendAsyncDelegate sendAsync;

		public XcapServer(SendAsyncDelegate sendAsync)
		{
			this.sendAsync = sendAsync;

			this.handlers = new List<IAuidHandler>();
			this.xcapCapsHander = new XcapCapsHandler();

			AddHandler(this.xcapCapsHander);
			AddHandler(new ResourceListsHandler());
			AddHandler(new RlsServicesHandler());

			//InitializeXcapPathParser();
			//int parsed;
			//var uri = "/rls-resourses/users/sip:jitsi@officesip.local/index";
			//if (pathParser.ParseAll(new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(uri)), out parsed) == false)
			//{
			//    Console.WriteLine(uri);
			//    Console.WriteLine("^".PadLeft(parsed + 1));
			//}
			//else
			//{
			//    Console.WriteLine(uri + " - ok");
			//}
		}

		public void AddHandler(IAuidHandler handler)
		{
			handler.GetWritter = GetWriter;

			handlers.Add(handler);

			xcapCapsHander.Update(handlers);
		}

		public void ProcessIncomingMessage(HttpConnection c, out bool closeConnection)
		{
			closeConnection = false;

			InitializeXcapPathParser();

			bool error = true;
			int parsed;
			if (pathParser.ParseAll(c.HttpReader.RequestUri.ToArraySegment(), out parsed))
			{
				pathParser.SetArray(c.HttpReader.RequestUri.Bytes);

				var request = System.Text.Encoding.UTF8.GetString(c.Header.Array, c.Header.Offset, c.Header.Count);

				Console.Write("{2} / {0} / {1}", pathParser.Auid.ToString(), pathParser.Segment2.ToString(), c.HttpReader.Method.ToString());
				if (pathParser.IsGlobal == false)
					Console.Write(" / {0}", pathParser.Item.ToString());
				Console.WriteLine(" / {0}", pathParser.DocumentName.ToString());

				var handler = GetHandler(pathParser.Auid.ToString());

				if (handler != null)
				{
					HttpMessageWriter response;

					if (pathParser.IsGlobal)
						response = handler.ProcessGlobal();
					else
					{
						switch (c.HttpReader.Method)
						{
							case Methods.Get:
								response = handler.ProcessGetItem(pathParser.Item.ToString());
								break;
							case Methods.Put:
								response = handler.ProcessPutItem(pathParser.Item.ToString(), c.Content);
								break;
							default:
								response = null;
								break;
						}
					}

					if (response != null)
					{
						sendAsync(c, response);
						error = false;
					}
				}
			}
			else
			{
				Console.WriteLine(c.HttpReader.RequestUri.ToString());
				Console.WriteLine("^".PadLeft(parsed + 1));
			}

			if (error)
			{
				StatusCodes statusCode = StatusCodes.NotFound;

				using (var writer = new HttpMessageWriter())
				{
					writer.WriteStatusLine(statusCode);
					writer.WriteContentLength(0);
					writer.WriteCRLF();

					sendAsync(c, writer);
				}
			}
		}

		private IAuidHandler GetHandler(string auid)
		{
			foreach (var handler in handlers)
			{
				if (handler.Auid == auid)
					return handler;
			}

			return null;
		}

		private HttpMessageWriter GetWriter()
		{
			return new HttpMessageWriter();
		}

		private void InitializeXcapPathParser()
		{
			if (pathParser == null)
				pathParser = new XcapPathParser();

			pathParser.SetDefaultValue();
		}
	}
}
