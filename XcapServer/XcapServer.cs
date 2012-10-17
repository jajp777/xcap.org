using System;
using System.Text;
using System.Collections.Generic;
using Base.Message;
using Http.Message;
using Xcap.PathParser;
using SocketServers;
using Http.Server;

namespace XcapServer
{
	class XcapServer
		: IHttpServerAgent
	{
		private static readonly byte[] xcapUri = Encoding.UTF8.GetBytes("/xcap-root/");

		[ThreadStatic]
		private static XcapPathParser pathParser;

		private readonly IHttpServer httpServer;
		private readonly List<IAuidHandler> handlers;
		private readonly XcapCapsHandler xcapCapsHander;

		public XcapServer(IHttpServerAgentRegistrar registrar)
		{
			this.httpServer = registrar.Register(this);
			this.handlers = new List<IAuidHandler>();
			this.xcapCapsHander = new XcapCapsHandler();

			AddHandler(this.xcapCapsHander);
		}

		public void Dispose()
		{
		}

		public void AddHandler(IAuidHandler handler)
		{
			handler.GetWritter = GetWriter;

			handlers.Add(handler);

			xcapCapsHander.Invalidate();
		}

		bool IHttpServerAgent.IsHandled(HttpMessageReader httpReader)
		{
			return httpReader.RequestUri.StartsWith(xcapUri);
		}

		void IHttpServerAgent.HandleRequest(BaseConnection c, HttpMessageReader httpReader, ArraySegment<byte> httpContent)
		{
			Console.WriteLine("{0} :: {1}", httpReader.Method.ToString(), httpReader.RequestUri.ToString());

			HttpMessageWriter response = null;

			InitializeXcapPathParser();

			int parsed;
			if (pathParser.ParseAll(httpReader.RequestUri.ToArraySegment(), out parsed) == false)
			{
				Console.WriteLine("Failed to parse requesr uri.");
				Console.WriteLine("   " + httpReader.RequestUri.ToString());
				Console.WriteLine("   " + "^".PadLeft(parsed + 1, '-'));
			}
			else
			{
				pathParser.SetArray(httpReader.RequestUri.Bytes);

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
						switch (httpReader.Method)
						{
							case Methods.Get:
								response = handler.ProcessGetItem(pathParser.Item.ToString());
								break;
							case Methods.Put:
								response = handler.ProcessPutItem(pathParser.Item.ToString(), httpContent);
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

			httpServer.SendResponse(c, response);
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
