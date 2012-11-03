using System;
using System.Text;
using System.Collections.Generic;
using Base.Message;
using Http.Message;
using SocketServers;
using Server.Http;

namespace Server.Xcap
{
	class XcapServer
		: IHttpServerAgent
		, IAuidHandlerContext
	{
		private static readonly byte[] xcapUri = Encoding.UTF8.GetBytes("/xcap-root/");
		private static readonly byte[] pidfManipulation = Encoding.UTF8.GetBytes("pidf-manipulation");

		[ThreadStatic]
		private static XcapUriParser pathParser;

		private IHttpServer httpServer;
		private readonly XcapCapsHandler xcapCapsHander;
		private readonly HandlerCollection<IGenericAuidHandler> genericHandlers;
		private readonly HandlerCollection<IUsersAuidHandler> usersHandlers;

		#region class HandlerCollection<T> {...}

		class HandlerCollection<T>
			where T : IAuidHandler
		{
			private readonly List<T> list;

			public HandlerCollection()
			{
				list = new List<T>();
			}

			public void Add(T handler)
			{
				list.Add(handler);
			}

			public T Get(string auid)
			{
				foreach (var handler in list)
				{
					if (handler.Auid == auid)
						return handler;
				}

				return default(T);
			}

			public int Count
			{
				get { return list.Count; }
			}

			public T this[int index]
			{
				get { return list[index]; }
			}
		}

		#endregion

		public XcapServer()
		{
			this.genericHandlers = new HandlerCollection<IGenericAuidHandler>();
			this.usersHandlers = new HandlerCollection<IUsersAuidHandler>();
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

		public void AddHandler(IUsersAuidHandler handler)
		{
			handler.Context = this;

			usersHandlers.Add(handler);

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

			if (httpReader.Method == Methods.Options)
				return HttpServerAgent.IsHandledResult.Handle();

			if (ParsePath(httpReader.RequestUri) == false)
				return HttpServerAgent.IsHandledResult.Handle();

			if (pathParser.Domain.IsInvalid || pathParser.Username.IsInvalid)
				return HttpServerAgent.IsHandledResult.Handle();

			if (pathParser.Auid.Equals(pidfManipulation) && httpReader.Method == Methods.Get)
				return HttpServerAgent.IsHandledResult.Handle();

			return HttpServerAgent.IsHandledResult.HandleWithAuthorization(pathParser.Domain);
		}

		bool IHttpServerAgent.IsAuthorized(HttpMessageReader httpReader, ByteArrayPart username)
		{
			return ParsePath(httpReader.RequestUri)
				&& pathParser.Username.IsValid && pathParser.Username.Equals(username);
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

				if (httpReader.Method == Methods.Options)
				{
					response = GetWriter();
					response.WriteOptionsResponse();
				}
				else
				{
					if (pathParser.Username.IsValid && pathParser.Domain.IsValid)
					{
						var handler = usersHandlers.Get(pathParser.Auid.ToString());

						switch (httpReader.Method)
						{
							case Methods.Get:
								response = handler.ProcessGetItem(pathParser.Username, pathParser.Domain);
								break;
							case Methods.Put:
								response = handler.ProcessPutItem(pathParser.Username, pathParser.Domain, httpContent);
								break;
							case Methods.Delete:
								response = handler.ProcessDeleteItem(pathParser.Username, pathParser.Domain);
								break;
							default:
								response = null;
								break;
						}
					}
					else
					{
						var handler = genericHandlers.Get(pathParser.Auid.ToString());

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
			}


			if (response == null)
			{
				response = GetWriter();
				response.WriteEmptyResponse(StatusCodes.NotFound);
			}

			httpServer.SendResponse(c, response);
		}

		private IEnumerable<IAuidHandler> Handlers
		{
			get
			{
				for (int i = 0; i < usersHandlers.Count; i++)
					yield return usersHandlers[i];

				for (int i = 0; i < genericHandlers.Count; i++)
					yield return genericHandlers[i];
			}
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
				pathParser = new XcapUriParser();

			pathParser.SetDefaultValue();
		}
	}
}
