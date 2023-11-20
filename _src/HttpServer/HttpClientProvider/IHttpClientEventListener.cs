using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Zoka.Paddle.HttpClientProvider
{
	/// <summary>Event listener for IHttpClient events</summary>
	public interface IHttpClientEventListener
	{
		/// <summary>When request is about to be issued</summary>
		void OnRequestBegin(HttpClient _http_client, Guid _id, HttpMethod _method, Uri _target_url);

		/// <summary>When request has been finished (either with exception (_ex != null) or with any result success or not success)</summary>
		void OnRequestEnd(HttpClient _http_client, Guid _id, HttpResponseMessage _response, Exception _ex);
	}
}
