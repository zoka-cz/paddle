using System;
using System.Collections.Generic;
using System.Text;

namespace Zoka.Paddle.ConnectionState.Abstractions
{
	/// <summary>The states of internet connection</summary>
	public enum EInternetConnectionState
	{
		/// <summary>The state cannot be detected</summary>
		Unknown,
		/// <summary>The internet is connected</summary>
		Connected,
		/// <summary>The internet is disconnected</summary>
		Disconnected
	}
}
