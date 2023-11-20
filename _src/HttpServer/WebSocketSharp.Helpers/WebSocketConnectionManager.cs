using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebSocketSharp.Server;

namespace Zoka.Paddle.WebSocketSharp.Helpers
{
	/// <summary></summary>
	public abstract class WebSocketConnectionManager
	{
		private readonly string								m_WebSocketPath;
		private List<WebSocketConnection>					m_WebSocketConnections = new List<WebSocketConnection>();
		private readonly HttpServer							m_HttpServer;


		/// <summary>Constructor</summary>
		protected WebSocketConnectionManager(string _path, HttpServer _http_server)
		{
			m_WebSocketPath = _path;
			m_HttpServer = _http_server;
		}

		/// <summary>Will start listening on websocket</summary>
		public void											StartListening()
		{
			m_HttpServer.AddWebSocketService<WebSocketConnection>(m_WebSocketPath, OnNewConnection);
		}

		/// <summary>Will stop listening</summary>
		public void											StopListening()
		{
			m_HttpServer.RemoveWebSocketService(m_WebSocketPath);
		}

		/// <summary></summary>
		public void											OnNewConnection(WebSocketConnection _web_socket_connection)
		{
			Debug.WriteLine($"WebsocketConnectionManager: new connection ({m_WebSocketPath})");
			_web_socket_connection.SetWebSocketConnectionManager(this);
			m_WebSocketConnections.Add(_web_socket_connection);
		}

		/// <summary></summary>
		public void											OnConnectionClosed(WebSocketConnection _web_socket_connection)
		{
			Debug.WriteLine($"WebsocketConnectionManager: connection closed ({m_WebSocketPath})");
			m_WebSocketConnections.Remove(_web_socket_connection);
		}

		/// <summary></summary>
		public void											OnConnectionError(WebSocketConnection _web_socket_connection)
		{
			Debug.WriteLine($"WebsocketConnectionManager: Socker error ({m_WebSocketPath})");
			_web_socket_connection.Close();
		}

		/// <summary></summary>
		public void											SendString(string _data)
		{
			m_WebSocketConnections.ForEach(ws => ws.SendString(_data));
		}

		/// <summary></summary>
		public void											SendData<T>(T _data)
		{
			JsonSerializerSettings sett = new JsonSerializerSettings();
			sett.ContractResolver = new DefaultContractResolver()
			{
				NamingStrategy = new CamelCaseNamingStrategy()
			};
			var data_ser = JsonConvert.SerializeObject(_data, sett);
			m_WebSocketConnections.ForEach(ws => ws.SendString(data_ser));
		}

	}
}
