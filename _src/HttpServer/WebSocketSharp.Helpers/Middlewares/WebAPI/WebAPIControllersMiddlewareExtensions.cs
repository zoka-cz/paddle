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
	/// <summary>Middleware simulating WebAPI controllers extensions</summary>
	public static class WebAPIControllersMiddlewareExtensions
	{
		/// <summary>Will register controllers from all dependencies as scoped services</summary>
		public static IServiceCollection					AddWebAPIControllersFromRunningAssembly(this IServiceCollection _services)
		{
			foreach (var type in TypeExtensions.GetTypesFromAllAssemblies())
			{
				if (typeof(Controller).IsAssignableFrom(type) && type != typeof(Controller))
				{
					Debug.WriteLine($"Controller found: {type.FullName}");
					_services.AddScoped(type);
				}
			}
			return _services;
		}
	}
}