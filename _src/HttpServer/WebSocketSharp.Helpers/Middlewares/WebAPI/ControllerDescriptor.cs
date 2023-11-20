using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Zoka.Paddle.WebSocketSharp.Helpers.Middlewares.WebAPI
{
	internal class ControllerDescriptor
	{
		/// <summary>Controller name</summary>
		public string										Name { get; private set; }

		public Type											ControllerType { get; private set; }

		public List<ControllerActionDescriptor>				ActionDescriptors { get; } = new List<ControllerActionDescriptor>();

		public async Task<bool>				RunAction(WebSocketSharpMiddlewareContext _context, IServiceProvider _service_provider)
		{
			if (_context.RequestMethod == HttpMethod.Options.Method)
			{
				return await RespondToOptions(_context);
			}

			var actions = ActionDescriptors
				.Where(d => string.Compare(d.Method, _context.RequestMethod, StringComparison.InvariantCultureIgnoreCase) == 0)
				.OrderByDescending(d => d.Parameters.Count);
			if (!actions.Any())
				return false;

			var controller_inst = _service_provider.GetService(ControllerType) as Controller;
			if (controller_inst == null)
				throw new InvalidOperationException($"Error instantiating the controller of {ControllerType} type");
			controller_inst.Context = _context;

			foreach (var action in actions)
			{
				if (await action.TryRunAction(_context, controller_inst))
					return true;
			}

			return false;
		}

		public Task<bool>									RespondToOptions(WebSocketSharpMiddlewareContext _context)
		{
			var headers = new List<(string, string)>();

			var requested_methods = _context.RequestHeaders.GetValues("Access-Control-Request-Method");
			bool cors_requested_methods = requested_methods?.Any() ?? false;
			var methods = ActionDescriptors.GroupBy(ad => ad.Method);
			foreach (var method in methods)
			{
				headers.Add(("Allow", method.Key));
				if (cors_requested_methods)
					headers.Add(("Access-Control-Allow-Methods", method.Key));
			}

			var requested_headers = _context.RequestHeaders.GetValues("Access-Control-Request-Headers");
			foreach (var requested_header in requested_headers ?? Enumerable.Empty<string>())
			{
				headers.Add(("Access-Control-Allow-Headers", requested_header));
			}

			var origin = _context.RequestHeaders.GetValues("Origin");
			if (origin?.Any() ?? false)
			{
				headers.Add(("Access-Control-Allow-Origin", origin.First()));
			}

			return _context.RespondWithStatusCode(HttpStatusCode.NoContent, headers);
		}

		public static ControllerDescriptor					FromType(Type _controller_type)
		{
			if (!typeof(Controller).IsAssignableFrom(_controller_type))
				throw new InvalidOperationException("Registered type is not derived from Controller");

			var desc = new ControllerDescriptor() { ControllerType = _controller_type };
			// controller name
			desc.Name = _controller_type.Name.EndsWith("Controller") ? _controller_type.Name.Substring(0, _controller_type.Name.Length - "Controller".Length) : _controller_type.Name;
			// Action descriptors
			var methods = _controller_type.GetMethods();
			foreach (var method_info in methods.Where(m => m.DeclaringType == _controller_type))
			{
				try
				{
					var action_descriptor = ControllerActionDescriptor.FromMethodInfo(method_info);
					if (action_descriptor != null)
						desc.ActionDescriptors.Add(action_descriptor);
				}
				catch { }
			}

			return desc;
		}
	}
}
