using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zoka.Paddle.WebSocketSharp.Helpers.Middlewares.DefaultDocument
{
	/// <inheritdoc />
	public class DefaultDocumentMiddleware : IMiddleware
	{
		private readonly List<string>						m_DefaultDocuments;

		/// <summary>Constructor</summary>
		public DefaultDocumentMiddleware(IEnumerable<string> _default_documents)
		{
			m_DefaultDocuments = new List<string>(_default_documents);
		}

		/// <inheritdoc />
		public Task											ProcessAsync(WebSocketSharpMiddlewareContext _context, IServiceProvider _service_provider, NextProcessDelegate _next)
		{
			if (_context.OriginalUrl.AbsolutePath == "/")
			{
				m_DefaultDocuments.ForEach(def_doc => _context.AddAlternateUrl(new Uri($"{_context.OriginalUrl.Scheme}://{_context.OriginalUrl.Authority}/{def_doc}")));
			}
			return _next(_context, _service_provider);
		}

		/// <inheritdoc />
		public void											ServerUnbinding()
		{
		}
	}
}
