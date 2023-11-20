using System;
using System.Diagnostics;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Zoka.Paddle.WebSocketSharp.Helpers
{
	/// <summary></summary>
	public class WebSocketConnection : WebSocketBehavior
	{
		private WebSocketConnectionManager					m_Owner;

		/// <summary>Will set the WebSocketConnectionManager</summary>
		public void											SetWebSocketConnectionManager(WebSocketConnectionManager _web_socket_connection_manager)
		{
			if (m_Owner != null)
				throw new InvalidOperationException("WebSocketConnectionManager has been already set");
			m_Owner = _web_socket_connection_manager;
		}

		/// <inheritdoc />
		protected override void OnOpen()
		{
			base.OnOpen();
		}

		/// <inheritdoc />
		protected override void OnMessage(MessageEventArgs e)
		{
			if (e.IsText)
			{
				Debug.WriteLine($"Websocket message received from client #{this.ID}: {e.Data}");
			}
			else
			{
				Debug.WriteLine($"Websocket message received from client #{this.ID}: Binary data");
			}
			base.OnMessage(e);
		}

		/// <inheritdoc />
		protected override void OnClose(CloseEventArgs e)
		{
			m_Owner.OnConnectionClosed(this);
			base.OnClose(e);
		}

		/// <inheritdoc />
		protected override void OnError(ErrorEventArgs e)
		{
			m_Owner.OnConnectionError(this);
			base.OnError(e);
		}

		/// <summary>Will close the connection</summary>
		public void											Close(CloseStatusCode _close_status_code = CloseStatusCode.Normal)
		{
			this.CloseAsync(_close_status_code, _close_status_code.ToString("G"));
		}

		/// <summary></summary>
		public void											SendString(string _data)
		{
			this.Send(_data);
		}
	}
}
