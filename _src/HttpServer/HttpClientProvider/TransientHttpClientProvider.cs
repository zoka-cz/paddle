using System;
using System.Collections.Generic;
using System.Text;

namespace Zoka.Paddle.HttpClientProvider
{
	/// <summary>Transient IHttpClientProvider, which provides new instance for every single request</summary>
	/// <remarks>The IHttpClients must be registered in DI as Transient</remarks>
	public class TransientHttpClientProvider : IHttpClientProvider
	{
		private readonly Dictionary<string, Type>			m_ClientTypes = new Dictionary<string, Type>();
		private readonly IServiceProvider					m_ServiceProvider;

		/// <summary>Constructor</summary>
		public TransientHttpClientProvider(IServiceProvider _service_provider)
		{
			m_ServiceProvider = _service_provider;
		}

		/// <summary>Will register the IHttpClient type for some specific server id</summary>
		public void											RegisterClientForServerId(string _server_id, Type _http_client_type)
		{
			if (!typeof(IHttpClient).IsAssignableFrom(_http_client_type))
				throw new ArgumentException($"Type {_http_client_type} is not of IHttpClient type.");

			m_ClientTypes[_server_id] = _http_client_type;
		}

		/// <inheritdoc />
		public IHttpClient									GetHttpClient(string _server_id)
		{
			IHttpClient client = null;

			if (m_ClientTypes.ContainsKey(_server_id))
			{
				client = m_ServiceProvider.GetService(m_ClientTypes[_server_id]) as IHttpClient;
			}

			return client;
		}
	}
}
