using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Zoka.Paddle.HttpClientProvider
{
	/// <summary>Provides list of HttpClient name - corresponding type</summary>
	public interface IScopedHttpClientTypes
	{
		/// <summary></summary>
		bool HasServerId(string _server_id);
		/// <summary></summary>
		Type this[string _server_id] { get; }
	}

	/// <summary>Scoped IHttpClientProvider, which provides new instance for every single created scope</summary>
	/// <remarks>
	///		Must be registered as scoped.
	///		IScopedHttpClientTypes should be registered as singleton
	///		Each IHttpClient must be registered as Singleton or Scoped (prefered), or Transient
	/// </remarks>
	public class ScopedHttpClientProvider : IHttpClientProvider
	{
		private readonly IScopedHttpClientTypes				m_ScopedHttpClientTypes;
		private readonly IServiceProvider					m_ServiceProvider;

		/// <summary>Constructor</summary>
		public ScopedHttpClientProvider(IScopedHttpClientTypes _scoped_http_client_types, IServiceProvider _service_provider)
		{
			m_ScopedHttpClientTypes = _scoped_http_client_types;
			m_ServiceProvider = _service_provider;
		}

		/// <inheritdoc />
		public IHttpClient									GetHttpClient(string _server_id)
		{
			IHttpClient client = null;

			if (m_ScopedHttpClientTypes.HasServerId(_server_id))
			{
				client = m_ServiceProvider.GetService(m_ScopedHttpClientTypes[_server_id]) as IHttpClient;
			}

			return client;
		}
	}
}
