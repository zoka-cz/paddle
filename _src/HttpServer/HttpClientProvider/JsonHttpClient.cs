using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Zoka.Paddle.HttpClientProvider
{
	/// <summary>Implementation of IHttpClient, which communicates with server using Json</summary>
	public abstract class JsonHttpClient : IHttpClient
	{
		/// <summary>Logger</summary>
		protected readonly ILogger<JsonHttpClient>			m_Logger;

		/// <summary>If not null, the next call to the GetConfiguredHttpClient should use and then null these credentials</summary>
		private (string, string)?							m_SingleUseCredentials = null;
		private readonly Dictionary<string, IEnumerable<string>> m_SingleUseAdditionalHeaders = new Dictionary<string, IEnumerable<string>>();
		private readonly IEnumerable<IHttpClientEventListener> m_EventListeners;

		/// <summary>Constructor</summary>
		public JsonHttpClient(IEnumerable<IHttpClientEventListener> _event_listeners, ILogger<JsonHttpClient> _logger)
		{
			m_EventListeners = _event_listeners;
			m_Logger = _logger;
		}

		/// <inheritdoc />
		public IHttpClient									WithSingleUseBasicAuth(string _username, string _password)
		{
			m_SingleUseCredentials = (_username, _password);
			return this;
		}

		/// <inheritdoc />
		public IHttpClient									WithSingleUseHeaders(string _name, IEnumerable<string> _values)
		{
			m_SingleUseAdditionalHeaders[_name] = _values;
			return this;
		}

		/// <inheritdoc />
		public async Task<TOut>								GetAsync<TOut>(string _api_url)
		{
			var http_client = GetHttpClient();
			m_Logger.LogTrace($"GET {_api_url}");
			var guid = RaiseOnBeginRequest(http_client, HttpMethod.Get, new Uri(_api_url, UriKind.Relative));
			HttpResponseMessage response = null;


			try
			{
				response = await http_client.GetAsync(_api_url);
			}
			catch (Exception ex)
			{
				RaiseOnEndRequest(http_client, guid, response, ex);
				Console.WriteLine(ex.ToString());
				throw;
			}

			RaiseOnEndRequest(http_client, guid, response, null);
			m_Logger.LogTrace($"GET {_api_url}. Received {response.StatusCode:G}");
			if (response.IsSuccessStatusCode)
			{
				var out_data_str = await response.Content.ReadAsStringAsync();
				var out_data = Newtonsoft.Json.JsonConvert.DeserializeObject<TOut>(out_data_str, GetJsonSerializerSettings());
				return out_data;
			}
			else
			{
				ProcessErrorResponse(response);
				return default(TOut);
			}

		}

		/// <summary>Will download file with GET request</summary>
		public async Task<ContentFileInfo>					GetFileAsync(string _api_url)
		{
			var http_client = GetHttpClient();
			m_Logger.LogTrace($"GET file {_api_url}");
			var response = await http_client.GetAsync(_api_url);
			m_Logger.LogTrace($"GET file {_api_url}. Received {response.StatusCode:G}");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsByteArrayAsync();
				return new ContentFileInfo()
				{
					MimeType = response.Content.Headers.ContentType?.MediaType,
					Filename = response.Content.Headers.ContentDisposition?.FileName,
					FileBytes = data,
				};
			}
			else
			{
				ProcessErrorResponse(response);
				return null;
			}
		}


		/// <inheritdoc />
		public async Task<TOut>								PostAsync<TOut, TIn>(string _api_url, TIn _data_to_send)
		{
			var http_client = GetHttpClient();
			var in_data_str = Newtonsoft.Json.JsonConvert.SerializeObject(_data_to_send, GetJsonSerializerSettings());
			m_Logger.LogTrace($"POST {_api_url}");
			var response = await http_client.PostAsync(_api_url, new StringContent(in_data_str, Encoding.UTF8, "application/json"));
			m_Logger.LogTrace($"POST {_api_url}. Received {response.StatusCode:G}");
			if (response.IsSuccessStatusCode)
			{
				var out_data_str = await response.Content.ReadAsStringAsync();
				var out_data = Newtonsoft.Json.JsonConvert.DeserializeObject<TOut>(out_data_str, GetJsonSerializerSettings());
				return out_data;
			}
			else
			{
				ProcessErrorResponse(response);
				return default(TOut);
			}
		}

		/// <inheritdoc />
		public async Task									PostAsync(string _api_url)
		{
			var http_client = GetHttpClient();
			m_Logger.LogTrace($"POST {_api_url}");
			var response = await http_client.PostAsync(_api_url, null);
			m_Logger.LogTrace($"POST {_api_url}. Received {response.StatusCode:G}");
			if (response.IsSuccessStatusCode)
			{
				return;
			}
			else
			{
				ProcessErrorResponse(response);
			}
		}

		/// <inheritdoc />
		public async Task<TOut>								PostAsyncDataAndFiles<TOut, TIn>(string _api_url, TIn _data_to_send, string _data_section_name, IEnumerable<ContentFileInfo> _files)
		{
			var http_client = GetHttpClient();
			var cnt = new MultipartFormDataContent();
			var in_data_str = Newtonsoft.Json.JsonConvert.SerializeObject(_data_to_send, GetJsonSerializerSettings());
			if (string.IsNullOrWhiteSpace(_data_section_name))
				cnt.Add(new StringContent(in_data_str, Encoding.UTF8, "application/json"));
			else
				cnt.Add(new StringContent(in_data_str, Encoding.UTF8, "application/json"), _data_section_name);
			var i = 0;
			foreach (var file in _files)
			{
				var file_cnt = new ByteArrayContent(file.FileBytes);
				file_cnt.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.MimeType);
				cnt.Add(file_cnt, $"file{i++}", file.Filename);
			}

			var response = await http_client.PostAsync(_api_url, cnt);
			if (response.IsSuccessStatusCode)
			{
				var out_data_str = await response.Content.ReadAsStringAsync();
				var out_data = Newtonsoft.Json.JsonConvert.DeserializeObject<TOut>(out_data_str, GetJsonSerializerSettings());
				return out_data;
			}
			else
			{
				ProcessErrorResponse(response);
				return default(TOut);
			}
		}

		/// <inheritdoc />
		public async Task									PutAsync<TIn>(string _api_url, TIn _data_to_send)
		{
			var http_client = GetHttpClient();
			var in_data_str = Newtonsoft.Json.JsonConvert.SerializeObject(_data_to_send, GetJsonSerializerSettings());
			m_Logger.LogTrace($"PUT {_api_url}");
			var response = await http_client.PutAsync(_api_url, new StringContent(in_data_str, Encoding.UTF8, "application/json"));
			m_Logger.LogTrace($"PUT {_api_url}. Received {response.StatusCode:G}");
			if (response.IsSuccessStatusCode)
			{
				return;
			}
			else
			{
				ProcessErrorResponse(response);
			}
		}


		/// <summary>Will send the PUT request with passed model and also passed files (multipart content)</summary>
		public async Task<TOut>								PutAsyncDataAndFiles<TOut, TIn>(string _api_url, TIn _data_to_send, string _data_section_name, IEnumerable<ContentFileInfo> _files)
		{
			var http_client = GetHttpClient();
			var cnt = new MultipartFormDataContent();
			var in_data_str = Newtonsoft.Json.JsonConvert.SerializeObject(_data_to_send, GetJsonSerializerSettings());
			if (string.IsNullOrWhiteSpace(_data_section_name))
				cnt.Add(new StringContent(in_data_str, Encoding.UTF8, "application/json"));
			else
				cnt.Add(new StringContent(in_data_str, Encoding.UTF8, "application/json"), _data_section_name);
			var i = 0;
			foreach (var file in _files)
			{
				var file_cnt = new ByteArrayContent(file.FileBytes);
				file_cnt.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.MimeType);
				cnt.Add(file_cnt, $"file{i++}", file.Filename);
			}

			var response = await http_client.PutAsync(_api_url, cnt);
			if (response.IsSuccessStatusCode)
			{
				var out_data_str = await response.Content.ReadAsStringAsync();
				var out_data = Newtonsoft.Json.JsonConvert.DeserializeObject<TOut>(out_data_str, GetJsonSerializerSettings());
				return out_data;
			}
			else
			{
				ProcessErrorResponse(response);
				return default(TOut);
			}
		}


		/// <inheritdoc />
		public async Task									DeleteAsync(string _api_url)
		{
			var http_client = GetHttpClient();
			m_Logger.LogTrace($"DELETE {_api_url}");
			var response = await http_client.DeleteAsync(_api_url);
			m_Logger.LogTrace($"DELETE {_api_url}. Received {response.StatusCode:G}");
			if (response.IsSuccessStatusCode)
			{
				return;
			}
			else
			{
				ProcessErrorResponse(response);
			}
		}

		private HttpClient									GetHttpClient()
		{
			var http_client = GetConfiguredHttpClient();
			if (m_SingleUseCredentials.HasValue)
			{
				http_client.DefaultRequestHeaders.Authorization = HttpClientHelpers.GetBasicAuthorizationHeader(m_SingleUseCredentials.Value.Item1, m_SingleUseCredentials.Value.Item2);
				m_SingleUseCredentials = null;
			}

			http_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			if (m_SingleUseAdditionalHeaders.Any())
			{
				foreach (var additional_header in m_SingleUseAdditionalHeaders)
				{
					http_client.DefaultRequestHeaders.Add(additional_header.Key, additional_header.Value);
				}
				m_SingleUseAdditionalHeaders.Clear();
			}

			return http_client;
		}
		/// <summary>Will return instance of the HttpClient correctly configured</summary>
		protected abstract HttpClient						GetConfiguredHttpClient();

		/// <summary>Will retrieve JsonSerializedSettings</summary>
		protected virtual JsonSerializerSettings			GetJsonSerializerSettings()
		{
			var sett = new JsonSerializerSettings();
			sett.DateParseHandling = DateParseHandling.None;
			sett.Culture = CultureInfo.InvariantCulture;
			return sett;
		}

		/// <summary>Error response processor</summary>
		protected virtual void								ProcessErrorResponse(HttpResponseMessage _response)
		{
			var cnt = _response.Content.ReadAsStringAsync().Result;
			Debug.WriteLine(cnt);
			m_Logger.LogWarning($"Received error response ({_response.StatusCode}) from ({_response.RequestMessage.Method.Method} {_response.RequestMessage.RequestUri.ToString()}) with content: {cnt}");
			throw new Exception($"Error receiving response {_response.StatusCode} - {_response.ReasonPhrase}");
		}

		private Guid										RaiseOnBeginRequest(HttpClient _http_client, HttpMethod _http_method, Uri _uri)
		{
			var guid = Guid.NewGuid();

			if (m_EventListeners == null)
				return guid;

			foreach (var event_listener in m_EventListeners)
			{
				event_listener.OnRequestBegin(_http_client, guid, _http_method, _uri);
			}

			return guid;
		}

		private void										RaiseOnEndRequest(HttpClient _http_client, Guid _id, HttpResponseMessage _message, Exception _ex)
		{
			if (m_EventListeners == null)
				return;

			foreach (var event_listener in m_EventListeners)
			{
				event_listener.OnRequestEnd(_http_client, _id, _message, _ex);
			}
		}
	}
}
