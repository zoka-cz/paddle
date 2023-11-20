using System;
using Xamarin.Essentials;
using Zoka.Paddle.PlatformConfiguration.Abstraction;

namespace Zoka.Paddle.PlatformConfiguration.UWP
{
	/// <inheritdoc />
	public class UWPPlatformConfiguration : Zoka.Paddle.PlatformConfiguration.Abstraction.IPlatformConfiguration
	{
		/// <summary>Constructor</summary>
		public UWPPlatformConfiguration(string _app_name, string _version)
		{
			AppId = Xamarin.Essentials.AppInfo.PackageName;
			AppName = _app_name;
			Version = _version;
			DeviceInformation = new DeviceInformation(
				_model: DeviceInfo.Model,
				_manufacturer: DeviceInfo.Manufacturer,
				_device_name: DeviceInfo.Name,
				_version: DeviceInfo.Version.ToString(),
				_platform: DeviceInfo.Platform.ToString(),
				_idiom: DeviceInfo.Idiom.ToString(),
				_device_type: DeviceInfo.DeviceType.ToString());
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
		public string										LocalhostBindUrl => "127.0.0.1";

		/// <inheritdoc />
		public int											LocalhostBindPort { get; set; } = 8000;

		/// <inheritdoc />
		public bool											HasKeyboard { get; } = true;

		/// <inheritdoc />
		public bool											MayControlBrightness { get; } = false;

		/// <inheritdoc />
		public bool											MayScanQRCodes { get; } = false;

		/// <summary>Will return whether the platform has possibility to access current location</summary>
		public bool											LocationServiceAvailable { get; } = false;

		/// <inheritdoc />
		public bool											FingerprintsSupport { get; } = false;


		/// <inheritdoc />
		public DeviceInformation							DeviceInformation { get; }
	}
}
