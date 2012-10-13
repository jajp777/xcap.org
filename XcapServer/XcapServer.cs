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
		}

		public void AddHandler(IAuidHandler handler)
		{
			handler.GetWritter = GetWriter;

			handlers.Add(handler);

			xcapCapsHander.Invalidate();
		}

		public void ProcessIncomingMessage(HttpConnection c, out bool closeConnection)
		{
			Console.WriteLine("{0} :: {1}", c.HttpReader.Method.ToString(), c.HttpReader.RequestUri.ToString());

			closeConnection = false;

			HttpMessageWriter response = null;

			InitializeXcapPathParser();

			int parsed;
			if (pathParser.ParseAll(c.HttpReader.RequestUri.ToArraySegment(), out parsed) == false)
			{
				Console.WriteLine("Failed to parse requesr uri.");
				Console.WriteLine("   " + c.HttpReader.RequestUri.ToString());
				Console.WriteLine("   " + "^".PadLeft(parsed + 1, '-'));
			}
			else
			{
				pathParser.SetArray(c.HttpReader.RequestUri.Bytes);

				//Console.Write("{2} / {0} / {1}", pathParser.Auid.ToString(), pathParser.Segment2.ToString(), c.HttpReader.Method.ToString());
				//if (pathParser.IsGlobal == false)
				//    Console.Write(" / {0}", pathParser.Item.ToString());
				//Console.WriteLine(" / {0}", pathParser.DocumentName.ToString());

				var handler = GetHandler(pathParser.Auid.ToString());

				if (handler == xcapCapsHander && xcapCapsHander.IsValid == false)
					xcapCapsHander.Update(handlers);

				if (handler != null)
				{
					if (pathParser.IsGlobal)
					{
						response = handler.ProcessGlobal();
					}
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
							case Methods.Delete:
								response = handler.ProcessDeleteItem(pathParser.Item.ToString());
								break;
							default:
								response = null;
								break;
						}
					}
				}
			}


			if (response == null)
				response = GenerateErrorResponse(StatusCodes.NotFound);

			sendAsync(c, response);
		}

		private HttpMessageWriter GenerateErrorResponse(StatusCodes statusCode)
		{
			var writer = GetWriter();

			writer.WriteStatusLine(statusCode);
			writer.WriteContentLength(0);
			writer.WriteCRLF();

			return writer;
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
