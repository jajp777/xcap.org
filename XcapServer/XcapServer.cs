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
		, IAuidHandlerContext
	{
		private static readonly byte[] xcapUri = Encoding.UTF8.GetBytes("/xcap-root/");

		[ThreadStatic]
		private static XcapPathParser pathParser;

		private readonly IHttpServer httpServer;
		private readonly XcapCapsHandler xcapCapsHander;
		private readonly List<IGenericAuidHandler> genericHandlers;
		private IResourceListHandler resourceListHandler;

		public XcapServer(IHttpServerAgentRegistrar registrar)
		{
			this.httpServer = registrar.Register(this);
			this.genericHandlers = new List<IGenericAuidHandler>();
			this.xcapCapsHander = new XcapCapsHandler();

			AddHandler(this.xcapCapsHander);
		}

		public void Dispose()
		{
		}

		public void AddHandler(IGenericAuidHandler handler)
		{
			handler.Context = this;

			genericHandlers.Add(handler);

			xcapCapsHander.Invalidate();
		}

		public void AddHandler(IResourceListHandler handler)
		{
			if (resourceListHandler != null)
				throw new InvalidProgramException();

			handler.Context = this;

			resourceListHandler = handler;

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

				if (pathParser.Username.IsValid && resourceListHandler != null)
				{
					switch (httpReader.Method)
					{
						case Methods.Get:
							response = resourceListHandler.ProcessGetItem(pathParser.Username, pathParser.Domain);
							break;
						case Methods.Put:
							response = resourceListHandler.ProcessPutItem(pathParser.Username, pathParser.Domain, httpContent);
							break;
						case Methods.Delete:
							response = resourceListHandler.ProcessDeleteItem(pathParser.Username, pathParser.Domain);
							break;
						default:
							response = null;
							break;
					}
				}
				else
				{
					var handler = GetGenericHandler(pathParser.Auid.ToString());

					if (handler == xcapCapsHander && xcapCapsHander.IsValid == false)
						xcapCapsHander.Update(Handlers);

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
									response = handler.ProcessGetItem(pathParser.Item);
									break;
								case Methods.Put:
									response = handler.ProcessPutItem(pathParser.Item, httpContent);
									break;
								case Methods.Delete:
									response = handler.ProcessDeleteItem(pathParser.Item);
									break;
								default:
									response = null;
									break;
							}
						}
					}
				}
			}


			if (response == null)
				response = GenerateErrorResponse(StatusCodes.NotFound);

			httpServer.SendResponse(c, response);
		}

		private IEnumerable<IAuidHandler> Handlers
		{
			get
			{
				if (resourceListHandler != null)
					yield return resourceListHandler;

				for (int i = 0; i < genericHandlers.Count; i++)
					yield return genericHandlers[i];
			}
		}

		private HttpMessageWriter GenerateErrorResponse(StatusCodes statusCode)
		{
			var writer = GetWriter();

			writer.WriteStatusLine(statusCode);
			writer.WriteContentLength(0);
			writer.WriteCRLF();

			return writer;
		}

		private IGenericAuidHandler GetGenericHandler(string auid)
		{
			foreach (var handler in genericHandlers)
			{
				if (handler.Auid == auid)
					return handler;
			}

			return null;
		}

		HttpMessageWriter IAuidHandlerContext.GetWriter()
		{
			return GetWriter();
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
