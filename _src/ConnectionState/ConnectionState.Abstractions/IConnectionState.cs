using System;
using System.Collections.Generic;
using System.Text;

namespace Zoka.Paddle.ConnectionState.Abstractions
{
	/// <summary>The handler for internet connection change</summary>
	public delegate void ConnectionStateChangedHandler(EInternetConnectionState _new_state);

	/// <summary>The class responsible for monitoring internet connection state</summary>
	public interface IConnectionState
	{
		/// <summary>Returns the internet connection state</summary>
		EInternetConnectionState ConnectionState { get; }

		/// <summary>Event which allows clients to monitor changes in the connection state</summary>
		event ConnectionStateChangedHandler OnConnectionStateChanged;
	}
}
