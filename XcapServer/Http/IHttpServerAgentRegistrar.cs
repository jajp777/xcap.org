using System;
using System.Collections.Generic;

namespace Http.Server
{
	interface IHttpServerAgentRegistrar
	{
		IHttpServer Register(IHttpServerAgent agent, int priority);
	}
}
