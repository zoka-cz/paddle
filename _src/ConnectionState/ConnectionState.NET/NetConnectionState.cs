using System;
using System.Net.NetworkInformation;
using Zoka.Paddle.ConnectionState.Abstractions;

namespace Zoka.Paddle.ConnectionState.NET
{
	/// <summary>Connection state handler</summary>
	public class NetConnectionState : IConnectionState
	{
		/// <summary>Constructor</summary>
		public NetConnectionState()
		{
			NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
			NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
			m_ConnectionState = IsNetworkAvailable() ? EInternetConnectionState.Connected : EInternetConnectionState.Disconnected;
		}

		private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
		{
			var old_conn_state = m_ConnectionState;
			m_ConnectionState = IsNetworkAvailable() ? EInternetConnectionState.Connected : EInternetConnectionState.Disconnected;
			if (old_conn_state != m_ConnectionState)
				OnConnectionStateChanged?.Invoke(m_ConnectionState);
		}

		private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
		{
			m_ConnectionState = e.IsAvailable ? EInternetConnectionState.Connected : EInternetConnectionState.Disconnected;
			OnConnectionStateChanged?.Invoke(m_ConnectionState);
		}

		private EInternetConnectionState					m_ConnectionState = EInternetConnectionState.Unknown;

		/// <summary>
		/// Indicates whether any network connection is available.
		/// Filter connections below a specified speed, as well as virtual network cards.
		/// </summary>
		/// <param name="minimumSpeed">The minimum speed required. Passing 0 will not filter connection using speed.</param>
		/// <returns>
		///     <c>true</c> if a network connection is available; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>https://social.msdn.microsoft.com/Forums/vstudio/en-US/a6b3541b-b7de-49e2-a7a6-ba0687761af5/networkavailabilitychanged-event-does-not-fire?forum=csharpgeneral</remarks>
		private static bool									IsNetworkAvailable(long minimumSpeed = 0)
		{
			if (!NetworkInterface.GetIsNetworkAvailable())
				return false;

			foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
			{
				// discard because of standard reasons
				if ((ni.OperationalStatus != OperationalStatus.Up) ||
				    (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
				    (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
					continue;

				// this allow to filter modems, serial, etc.
				// I use 10000000 as a minimum speed for most cases
				if (ni.Speed < minimumSpeed)
					continue;

				// discard virtual cards (virtual box, virtual pc, etc.)
				if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
				    (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
					continue;

				// discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
				if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
					continue;

				return true;
			}
			return false;
		}

		/// <inheritdoc />
		public EInternetConnectionState						ConnectionState => m_ConnectionState;

		/// <inheritdoc />
		public event ConnectionStateChangedHandler			OnConnectionStateChanged;
	}
}
