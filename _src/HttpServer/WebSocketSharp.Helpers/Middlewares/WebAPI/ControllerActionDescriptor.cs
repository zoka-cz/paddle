using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Zoka.Paddle.WebSocketSharp.Helpers.Middlewares.WebAPI
{
	internal class ControllerActionDescriptor
	{
		public MethodInfo									MethodInfo { get; set; }
		public string										Name { get; set; }
		public string										Method { get; set; }
		public List<ControllerActionParameterDescriptor>	Parameters { get; } = new List<ControllerActionParameterDescriptor>();

		public async Task<bool>								TryRunAction(WebSocketSharpMiddlewareContext _context, Controller _controller)
		{
			if (string.Compare(_context.RequestMethod, Method, StringComparison.InvariantCultureIgnoreCase) != 0)
				throw new InvalidOperationException($"Requested to run action for {_context.RequestMethod} method but action is for {Method} method.");

			var (resolve_res, parameters) = await TryResolveParameters(_context);
			if (!resolve_res)
				return false;

			var res = MethodInfo.Invoke(_controller, parameters);
			if (typeof(Task).IsAssignableFrom(MethodInfo.ReturnType))
			{
				var awaitable_res = res as Task;
				await awaitable_res;
				
				if (awaitable_res.Status == TaskStatus.RanToCompletion)
				{
					var pi = MethodInfo.ReturnType.GetProperty("Result");
					if (pi == null)
					{
						return await _context.RespondWithStatusCode(HttpStatusCode.Accepted, GetCORSHeaders(_context));
					}
					var obj = pi.GetValue(awaitable_res);
					return await _context.RespondWithJson(obj, GetCORSHeaders(_context));
				}
				else
				{
					return false;
				}
			}
			else if (MethodInfo.ReturnType == typeof(void))
			{
				return await _context.RespondWithStatusCode(HttpStatusCode.Accepted, GetCORSHeaders(_context));
			}
			else
			{
				return await _context.RespondWithJson(res, GetCORSHeaders(_context));
			}
		}

		private async Task<(bool, object [])>				TryResolveParameters(WebSocketSharpMiddlewareContext _context)
		{
			var parameters = new object[Parameters.Count];
			if (Parameters.Count == 0)
				return (true, parameters);

			for (int i = 0; i < Parameters.Count; i++)
			{
				bool res;
				(res, parameters[i]) = await Parameters[i].TryResolveParameter(_context);
				if (!res)
					return (false, parameters);
			}

			return (true, parameters);
		}

		private IEnumerable<(string, string)>				GetCORSHeaders(WebSocketSharpMiddlewareContext _context)
		{
			var origin = _context.RequestHeaders.GetValues("Origin")?.SingleOrDefault();
			if (!string.IsNullOrWhiteSpace(origin))
				yield return ("Access-Control-Allow-Origin", "*");
		}

		public static ControllerActionDescriptor			FromMethodInfo(MethodInfo _method_info)
		{
			var action_descriptor = new ControllerActionDescriptor() { MethodInfo = _method_info, Name = _method_info.Name };

			if (_method_info.Name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase))
				action_descriptor.Method = "GET";
			if (_method_info.Name.StartsWith("Post", StringComparison.InvariantCultureIgnoreCase))
				action_descriptor.Method = "POST";
			if (_method_info.Name.StartsWith("Put", StringComparison.InvariantCultureIgnoreCase))
				action_descriptor.Method = "PUT";
			if (_method_info.Name.StartsWith("Delete", StringComparison.InvariantCultureIgnoreCase))
				action_descriptor.Method = "DELETE";

			if (action_descriptor.Method == null)
				return null;

			var parameters = _method_info.GetParameters();
			foreach (var parameter_info in parameters)
			{
				action_descriptor.Parameters.Add(ControllerActionParameterDescriptor.FromParameterInfo(parameter_info));
			}

			return action_descriptor;
		}
	}
}
