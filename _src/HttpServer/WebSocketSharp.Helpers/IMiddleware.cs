using System;
using System.Threading.Tasks;

namespace Zoka.Paddle.WebSocketSharp.Helpers
{
	/// <summary>Middleware</summary>
	public interface IMiddleware
	{
		/// <summary>Will process given context</summary>
		/// <remarks>
		///		The implementation should process the request, and if this is the right middleware, which can serve the response,
		///		it should set it up (Do not close response) and return false, to indicate that no further processing is requested
		/// </remarks>
		Task ProcessAsync(WebSocketSharpMiddlewareContext _context,  IServiceProvider _service_provider, NextProcessDelegate _next);

		/// <summary>Called when the server is about to be finished</summary>
		void ServerUnbinding();
	}
}
