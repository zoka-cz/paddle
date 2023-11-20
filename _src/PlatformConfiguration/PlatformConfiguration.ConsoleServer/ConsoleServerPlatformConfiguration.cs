using System;
using System.Net.Sockets;
using System.Net;
using Zoka.Paddle.PlatformConfiguration.Abstraction;

namespace PlatformConfiguration.ConsoleServer
{
	/// <inheritdoc />
	public class ConsoleServerPlatformConfiguration : IPlatformConfiguration
	{
		/// <summary>Constructor</summary>
		public ConsoleServerPlatformConfiguration(string _app_id, string _app_name, string _version, bool _may_control_brightness, bool _fingerprint_support)
		{
			AppId = _app_id;
			AppName = _app_name;
			Version = _version;
			DeviceInformation = new DeviceInformation(
				_model: "Unknown",
				_manufacturer: "Unknown",
				_device_name: Environment.MachineName,
				_version: Environment.Version.ToString(),
				_platform: Environment.OSVersion.Platform.ToString(),
				_idiom: "Desktop",
				_device_type: "Unknown");
			MayControlBrightness = _may_control_brightness;
			FingerprintsSupport = _fingerprint_support;
		}


		/// <summary>Application id</summary>
		public string										AppId { get; }
		/// <inheritdoc />
		public string										AppName { get; }
		/// <inheritdoc />
		public string										Version { get; }
		/// <inheritdoc />
		public string										LocalAppFolder => System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);
		/// <inheritdoc />
		public string										TempAppFolder => System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);

		/// <inheritdoc />
#if !DOCKERIZED
		public string										LocalhostBindUrl => "127.0.0.1";
#else
		public string 										LocalhostBindUrl => $"{GetLocalIPAddress()}";
#endif


		/// <inheritdoc />
		public int											LocalhostBindPort { get; set; } = 8000;

		/// <inheritdoc />
		public virtual bool									HasKeyboard { get; } = true;

		/// <inheritdoc />
		public bool											MayControlBrightness { get; }

		/// <inheritdoc />
		public bool											MayScanQRCodes { get; } = false;

		/// <summary>Will return whether the platform has possibility to access current location</summary>
		public bool											LocationServiceAvailable { get; } = false;

		/// <inheritdoc />
		public bool											FingerprintsSupport { get; }


		/// <inheritdoc />
		public DeviceInformation							DeviceInformation { get; protected set; }

		static string GetLocalIPAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			throw new Exception("No network adapters with an IPv4 address in the system!");
		}

	}
}
