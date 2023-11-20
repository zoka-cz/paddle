using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Zoka.Paddle.FileProviders.Abstraction;

namespace Zoka.Paddle.WebSocketSharp.Helpers.Middlewares.StaticFiles
{
	/// <summary>
	/// Serves static files
	/// </summary>
	public class StaticFilesMiddleware : IMiddleware
	{
		private readonly StaticFilesFileProviders			m_FileProviders;

		/// <summary>Constructor</summary>
		public StaticFilesMiddleware(string _url_prefix, StaticFilesFileProviders _file_providers)
		{
			m_FileProviders = _file_providers;
		}

		/// <inheritdoc />
		public async Task									ProcessAsync(WebSocketSharpMiddlewareContext _context, IServiceProvider _service_provider, NextProcessDelegate _next)
		{
			if (m_FileProviders == null)
			{
				await _next(_context, _service_provider);
				return;
			}
			Debug.WriteLine($"StaticFiles Url ({_context.OriginalUrl.ToString()})");

			var url = _context.OriginalUrl;
			var fi = await m_FileProviders.GetFileInfoByPathAsync(url);
			if (fi == null)
			{
				foreach (var url2 in _context.AlternativeUrls)
				{
					url = url2;
					fi = await m_FileProviders.GetFileInfoByPathAsync(url);
					if (fi != null)
						break;
				}
			}

			if (fi == null)
			{
				await _next(_context, _service_provider);
				return;
			}

			var filename = url.Segments.Last();
			if (await _context.RespondWithFile(fi.FileStream, fi.FileLength, filename))
				return;

			await _next(_context, _service_provider);
		}

		/// <inheritdoc />
		public void											ServerUnbinding()
		{
		}
	}
}
