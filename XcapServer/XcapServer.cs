using System;
using System.Text;
using System.Collections.Generic;
using Base.Message;
using Http.Message;
using Xcap.PathParser;
using SocketServers;
using Server.Http;

namespace Server.Xcap
{
	class XcapServer
		: IHttpServerAgent
		, IAuidHandlerContext
	{
		private static readonly byte[] xcapUri = Encoding.UTF8.GetBytes("/xcap-root/");

		[ThreadStatic]
		private static XcapPathParser pathParser;

		private IHttpServer httpServer;
		private readonly XcapCapsHandler xcapCapsHander;
		private readonly List<IGenericAuidHandler> genericHandlers;
		private IResourceListHandler resourceListHandler;

		public XcapServer()
		{
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

		IHttpServer IHttpServerAgent.IHttpServer
		{
			set { httpServer = value; }
		}

		HttpServerAgent.IsHandledResult IHttpServerAgent.IsHandled(HttpMessageReader httpReader)
		{
			if (httpReader.RequestUri.StartsWith(xcapUri) == false)
				return HttpServerAgent.IsHandledResult.NotHandle();

			if (ParsePath(httpReader.RequestUri) && pathParser.Domain.IsValid)
				return HttpServerAgent.IsHandledResult.HandleWithAuthorization(pathParser.Domain);

			return HttpServerAgent.IsHandledResult.Handle();
		}

		bool IHttpServerAgent.IsAuthorized(HttpMessageReader httpReader, ByteArrayPart username)
		{
			return ParsePath(httpReader.RequestUri)
				&& pathParser.Username.IsValid
				&& pathParser.Username.Equals(username);
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

		private bool ParsePath(ByteArrayPart requestUri)
		{
			InitializeXcapPathParser();

			if (pathParser.ParseAll(requestUri.ToArraySegment()))
			{
				pathParser.SetArray(requestUri.Bytes);

				return true;
			}

			return false;
		}

		private void InitializeXcapPathParser()
		{
			if (pathParser == null)
				pathParser = new XcapPathParser();

			pathParser.SetDefaultValue();
		}
	}
}
