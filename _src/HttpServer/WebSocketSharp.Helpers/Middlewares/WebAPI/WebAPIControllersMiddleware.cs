using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zoka.CSCommon;
using Zoka.Paddle.CSCommon;

namespace Zoka.Paddle.WebSocketSharp.Helpers.Middlewares.WebAPI
{
	/// <summary>Middleware simulating WebAPI controllers</summary>
	public class WebAPIControllersMiddleware : IMiddleware
	{
		private readonly ILogger<WebAPIControllersMiddleware> m_Logger;
		private readonly string								m_RoutePrefix;

		/// <summary>Constructor</summary>
		public WebAPIControllersMiddleware(ILogger<WebAPIControllersMiddleware> _logger, string _route_prefix = "")
		{
			m_Logger = _logger;
			m_RoutePrefix = _route_prefix;
		}

		private readonly List<ControllerDescriptor>			m_ControllerDescriptors = new List<ControllerDescriptor>();

		/// <summary>Will register the passed type as WebAPI controller</summary>
		public void											RegisterController(Type _controller_type, IServiceCollection _service_collection)
		{
			var desc = ControllerDescriptor.FromType(_controller_type);
			if (desc != null && !m_ControllerDescriptors.Any(d => d.Name == desc.Name))
			{
				m_ControllerDescriptors.Add(desc);
			}
		}

		/// <summary></summary>
		public void											RegisterControllersFromRunningAssembly(IServiceCollection _service_collection)
		{
			foreach (var type in TypeExtensions.GetTypesFromAllAssemblies())
			{
				if (typeof(Controller).IsAssignableFrom(type) && type != typeof(Controller))
				{
					Debug.WriteLine($"Controller found: {type.FullName}");
					RegisterController(type, _service_collection);
				}
			}
		}

		/// <inheritdoc />
		public async Task									ProcessAsync(WebSocketSharpMiddlewareContext _context, IServiceProvider _service_provider, NextProcessDelegate _next)
		{
			int seg_idx = 1;

			if ((_context.OriginalUrl.Segments.Length <= 1) || 
			    (!string.IsNullOrEmpty(m_RoutePrefix) && _context.OriginalUrl.Segments[seg_idx++].TrimCharacters('/'.Yield()) != m_RoutePrefix))
			{
				await _next(_context, _service_provider);
				return;
			}
			
			// now try to find the controller
			var controller_segment = _context.OriginalUrl.Segments[seg_idx].TrimCharacters('/'.Yield());
			var controller_desc = m_ControllerDescriptors.FirstOrDefault(d => string.Compare(d.Name, controller_segment, StringComparison.InvariantCultureIgnoreCase) == 0);
			if (controller_desc == null)
			{
				await _next(_context, _service_provider);
				return;
			}

			try
			{
				var result = await controller_desc.RunAction(_context, _service_provider);
				if (result)
					return;
			}
			catch (Exception ex)
			{
				m_Logger.LogWarning($"Error responding to {_context.RequestMethod} {_context.OriginalUrl.PathAndQuery}. Error {ex.ToStringAllExceptionDetails()}");
				await _context.RespondWithInternalError();
				return;
			}

			await _next(_context, _service_provider);
		}

		/// <inheritdoc />
		public void											ServerUnbinding()
		{
		}
	}
}