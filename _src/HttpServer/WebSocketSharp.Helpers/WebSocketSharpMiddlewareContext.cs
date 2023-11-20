using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HeyRed.Mime;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebSocketSharp.Server;

namespace Zoka.Paddle.WebSocketSharp.Helpers
{
	/// <summary>Small server request context</summary>
	public class WebSocketSharpMiddlewareContext
	{
		#region Private data members

		private readonly Guid								m_Id = Guid.NewGuid();
		private readonly HttpRequestEventArgs				m_HttpRequestContext;
		private readonly List<Uri>							m_Uris = new List<Uri>();
		private readonly Dictionary<string, object>			m_ContextStore = new Dictionary<string, object>();

		#endregion // Private data members

		#region Public members

		/// <summary>Returns the unique id of this request</summary>
		public Guid											Id => m_Id;

		#region URLs

		/// <summary>Returns the original request URI</summary>
		public Uri											OriginalUrl => m_HttpRequestContext.Request.Url;

		/// <summary>Returns the other possible URI (modified by some middleware</summary>
		public IEnumerable<Uri>								AlternativeUrls => m_Uris;
		/// <summary>Will add alternate uri into the list of URLs</summary>
		public void											AddAlternateUrl(Uri _uri)
		{
			m_Uris.Add(_uri);
		}

		/// <summary>The HTTP Request method</summary>
		public string										RequestMethod => m_HttpRequestContext.Request.HttpMethod;

		/// <summary>Endpoint</summary>
		public IPEndPoint									EndPoint => m_HttpRequestContext.Request.RemoteEndPoint;


		#endregion // URLs

		#region Headers

		/// <summary>Returns the request headers</summary>
		public NameValueCollection							RequestHeaders => m_HttpRequestContext.Request.Headers;

		#endregion // Headers

		#region Body

		/// <summary>Type of the content</summary>
		public string										ContentType => m_HttpRequestContext.Request.ContentType;

		/// <summary>Length of the content</summary>
		public long											ContentLength => m_HttpRequestContext.Request.ContentLength64;

		/// <summary>Content stream</summary>
		public Stream										ContentStream => m_HttpRequestContext.Request.InputStream;

		#endregion // Body

		#region Context store

		/// <summary>Will add object into the context store</summary>
		public void											AddToStore<T>(string _key, T _object)
		{
			m_ContextStore[_key] = _object;
		}

		/// <summary>Will return the T typed object from the store.</summary>
		/// <exception cref="InvalidCastException">When the object to be returned is not of the T type</exception>
		/// <exception cref="KeyNotFoundException">When the _key is not contained in the store</exception>
		public T											GetFromStore<T>(string _key)
		{
			if (m_ContextStore.ContainsKey(_key))
				return (T)m_ContextStore[_key];
			throw new KeyNotFoundException($"Key {_key} is not contained in the context store");
		}

		#endregion // Context store

		#endregion // Public members

		/// <summary>Constructor</summary>
		public WebSocketSharpMiddlewareContext(HttpRequestEventArgs _http_request_context)
		{
			m_HttpRequestContext = _http_request_context;
		}

		#region Response setting up

		/// <summary></summary>
		public async Task<bool>								RespondWithFile(Stream _file_stream, ulong? _file_length, string _filename, IEnumerable<(string, string)> _additional_headers = null)
		{
			if (_additional_headers != null)
			{
				foreach (var additional_header in _additional_headers)
				{
					m_HttpRequestContext.Response.Headers.Add(additional_header.Item1, additional_header.Item2);
				}
			}
			m_HttpRequestContext.Response.ContentType = MimeTypesMap.GetMimeType(_filename);
			await _file_stream.CopyToAsync(m_HttpRequestContext.Response.OutputStream);
			if (_file_length.HasValue)
			{
				m_HttpRequestContext.Response.ContentLength64 = (long)_file_length;
				Debug.WriteLine($"Setting Content-Length to {_file_length}");
			}
			_file_stream.Close();
			m_HttpRequestContext.Response.Close();
			return true;
		}

		/// <summary></summary>
		public Task<bool>									RespondWithInternalError()
		{
			m_HttpRequestContext.Response.StatusCode = 500;
			m_HttpRequestContext.Response.StatusDescription = "No middleware processed response";
			m_HttpRequestContext.Response.Close();
			return Task.FromResult(true);
		}

		/// <summary>Will respond with status code</summary>
		public Task<bool>									RespondWithStatusCode(HttpStatusCode _status_code, IEnumerable<(string, string)> _additional_headers = null)
		{
			if (_additional_headers != null)
			{
				foreach (var additional_header in _additional_headers)
				{
					m_HttpRequestContext.Response.Headers.Add(additional_header.Item1, additional_header.Item2);
				}
			}
			m_HttpRequestContext.Response.StatusCode = (int)_status_code;
			m_HttpRequestContext.Response.StatusDescription = _status_code.ToString();
			m_HttpRequestContext.Response.Close();
			return Task.FromResult(true);
		}

		/// <summary></summary>
		public async Task<bool>								RespondWithJson(object _data, IEnumerable<(string, string)> _additional_headers = null)
		{
			if (_additional_headers != null)
			{
				foreach (var additional_header in _additional_headers)
				{
					m_HttpRequestContext.Response.Headers.Add(additional_header.Item1, additional_header.Item2);
				}
			}
			m_HttpRequestContext.Response.StatusCode = 200;
			m_HttpRequestContext.Response.ContentType = "application/json";
			DefaultContractResolver contractResolver = new DefaultContractResolver
			{
				NamingStrategy = new CamelCaseNamingStrategy()
			}; 
			var json_sett = new JsonSerializerSettings();
			json_sett.ContractResolver = contractResolver;
			var data_json = JsonConvert.SerializeObject(_data, json_sett);
			var buffer = UTF8Encoding.UTF8.GetBytes(data_json);
			await m_HttpRequestContext.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
			m_HttpRequestContext.Response.ContentLength64 = buffer.Length;
			m_HttpRequestContext.Response.Close();
			return true;
		}

#endregion // Response setting up

	}
}
