using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WebSocketSharp.Server;

namespace Zoka.Paddle.WebSocketSharp.Helpers
{

	internal delegate Task ProcessDelegate(WebSocketSharpMiddlewareContext _context, IServiceProvider _service_provider, NextProcessDelegate _process_delegate);
	/// <summary>Delegate, which define the Next delegate of middleware to be processed</summary>
	public delegate Task NextProcessDelegate(WebSocketSharpMiddlewareContext _context, IServiceProvider _service_provider);


	/// <summary></summary>
	public class MiddlewareRequestProcessor
	{
		class MiddlewareComponentNode
		{
			public ProcessDelegate ProcessDelegate;
			public NextProcessDelegate NextDelegate;

			private IMiddleware m_Instance;
			public IMiddleware Instance(IServiceProvider _service_provider)
			{
				if (m_Instance == null)
					m_Instance = _service_provider.GetService(Component) as IMiddleware;

				return m_Instance;
			}
			public Type Component;
		}


		private readonly IServiceProvider					m_ServiceProvider;
		private readonly List<Type>							m_Middlewares = new List<Type>();

		/// <summary>Constructor</summary>
		public MiddlewareRequestProcessor(IServiceProvider _service_provider)
		{
			m_ServiceProvider = _service_provider;
		}


		/// <summary></summary>
		public void AddMiddleware(Type _middleware_type)
		{
			if (_middleware_type == null)
				throw new ArgumentNullException(nameof(_middleware_type));
			if (!typeof(IMiddleware).IsAssignableFrom(_middleware_type))
				throw new ArgumentException($"Type {_middleware_type.Name} is not of IMiddlewareType");
			m_Middlewares.Add(_middleware_type);
		}

		/// <summary></summary>
		public virtual async Task							ProcessRequestAsync(HttpRequestEventArgs e)
		{
			Debug.WriteLine($"Request for {e.Request.HttpMethod} {e.Request.Url}");
			using (var service_scope = m_ServiceProvider.CreateScope())
			{
				try
				{
					var context = new WebSocketSharpMiddlewareContext(e);

					var pipeline = BuildPipeline(service_scope.ServiceProvider);
					await pipeline(context, service_scope.ServiceProvider);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Error processing request pipeline");
					Debug.WriteLine(ex.ToStringAllExceptionDetails());
				}
			}
		}

		private NextProcessDelegate							BuildPipeline(IServiceProvider _service_provider)
		{
			if (m_Middlewares.Count == 0)
				return (_context, _provider) => Task.CompletedTask;

			var middlewares = new LinkedList<MiddlewareComponentNode>(m_Middlewares.Select(m => new MiddlewareComponentNode() { Component = m }));

			var node = middlewares.Last;
			while (node != null)
			{
				node.Value.NextDelegate = GetNextDelegate(node);
				node.Value.ProcessDelegate = GetProcessDelegate(node);
				node = node.Previous;
			}

			return (_context, _sp) => middlewares.First.Value.Instance(_sp).ProcessAsync(_context, _sp, middlewares.First.Value.NextDelegate);
		}

		private NextProcessDelegate							GetNextDelegate(LinkedListNode<MiddlewareComponentNode> _node)
		{
			if (_node.Next == null)
				return (_context, _provider) => Task.CompletedTask;
			else
			{
				return (_context, _provider) => _node.Next.Value.Instance(_provider).ProcessAsync(_context, _provider, _node.Next.Value.NextDelegate);
			}
		}

		private ProcessDelegate								GetProcessDelegate(LinkedListNode<MiddlewareComponentNode> _node)
		{
			if (_node.Next == null)
				return (_context, _next_middleware, _provider) => Task.CompletedTask;
			else
			{
				return (_context, _provider, _delegate) => _node.Next.Value.Instance(_provider).ProcessAsync(_context, _provider, _delegate);
			}
		}


	}
}
