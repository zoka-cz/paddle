using System;
using Xamarin.Essentials;
using Zoka.Paddle.ConnectionState.Abstractions;

namespace Zoka.Paddle.ConnectionState.Xamarin
{
	/// <summary>Xamarin dependent implementation of the connection state</summary>
	public class XamarinConnectionState : IConnectionState
	{
		/// <summary>Constructor</summary>
		public XamarinConnectionState()
		{
			Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
		}

		private void										Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
		{
			OnConnectionStateChanged?.Invoke(NetworkAccessToEInternetConnectionState(e.NetworkAccess));
		}

		/// <inheritdoc />
		public EInternetConnectionState						ConnectionState => NetworkAccessToEInternetConnectionState(Connectivity.NetworkAccess);

		private static EInternetConnectionState				NetworkAccessToEInternetConnectionState(NetworkAccess _network_access)
		{
			switch (_network_access)
			{
				case NetworkAccess.Unknown:
					return Abstractions.EInternetConnectionState.Unknown;
				case NetworkAccess.None:
				case NetworkAccess.Local:
				case NetworkAccess.ConstrainedInternet:
					return Abstractions.EInternetConnectionState.Disconnected;
				case NetworkAccess.Internet:
					return Abstractions.EInternetConnectionState.Connected;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <inheritdoc />
		public event ConnectionStateChangedHandler			OnConnectionStateChanged;
	}
}
