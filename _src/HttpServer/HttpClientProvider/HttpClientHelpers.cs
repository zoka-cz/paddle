using System;
using System.Collections.Generic;
using System.Text;

namespace Zoka.Paddle.HttpClientProvider
{
	/// <summary>Helpers for HttpClients</summary>
	public class HttpClientHelpers
	{
		/// <summary>Will return authorization header with Basic auth and parameter set to credentials provided</summary>
		/// <param name="_username">Username</param>
		/// <param name="_password">Password</param>
		/// <returns>authorization header with Basic auth and parameter set to credentials provided</returns>
		public static System.Net.Http.Headers.AuthenticationHeaderValue GetBasicAuthorizationHeader(string _username, string _password)
		{
			var param = _username + ":" + _password;
			var param_encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(param));
			return new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", param_encoded);
		}
	}
}
