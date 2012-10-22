
namespace Server.Http
{
	interface IHttpServerAgentRegistrar
	{
		void Register(IHttpServerAgent agent, int priority, bool isAuthorizationEnabled);
	}
}
