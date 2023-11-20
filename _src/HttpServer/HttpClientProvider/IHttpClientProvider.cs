using System;
using System.Collections.Generic;
using System.Text;

namespace Zoka.Paddle.HttpClientProvider
{
	/// <summary>Application provider of IHttpClient</summary>
	public interface IHttpClientProvider
	{
		/// <summary>Will return configured instance of the IHttpClient</summary>
		IHttpClient GetHttpClient(string _server_id);
	}
}
