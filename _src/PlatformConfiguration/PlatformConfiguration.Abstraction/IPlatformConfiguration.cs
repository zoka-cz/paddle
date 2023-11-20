using Zoka.Paddle.FileProviders.Abstraction;

namespace Zoka.Paddle.PlatformConfiguration.Abstraction
{
	/// <summary>Provides the interface for platform specific configurations and access to these platform specific providers</summary>
	public interface IPlatformConfiguration
	{
		/// <summary>Name of the application</summary>
		string AppName { get; }

		/// <summary>Application id</summary>
		string AppId { get; }

		/// <summary>Version of the application</summary>
		string Version { get; }
		/// <summary>Folder used to store all the app data</summary>
		string LocalAppFolder { get; }
		/// <summary>Folder used to store all the app temporary data (app must continue when deleted)</summary>
		string TempAppFolder { get; }

		/// <summary>according to the platform returns the url of the localhost so it can be bound in HTTP server</summary>
		string LocalhostBindUrl { get; }

		/// <summary>Returns the port, which should be bound by the HTTP server to listen on ng requests</summary>
		int LocalhostBindPort { get; }

		/// <summary>Returns, whether the keyboard is currently available, and UI may receive user input, or it should display own keyboard</summary>
		bool HasKeyboard { get; }

		/// <summary>
		///		When set, the brightness may be controlled by code. IBrightnessControl service is available.
		///		When not set, the brightness may not be changed by user code. IBrightnessControl service is not available.
		/// </summary>
		bool MayControlBrightness { get; }

		/// <summary>Will return whether the platform has possibility to scan QR codes</summary>
		bool MayScanQRCodes { get; }

		/// <summary>Will return whether the platform has possibility to access current location</summary>
		bool LocationServiceAvailable { get; }

		/// <summary>If set, the fingerprints are supported. If not set, the support is disabled</summary>
		bool FingerprintsSupport { get; }

		/// <summary>Information about device</summary>
		DeviceInformation DeviceInformation { get; }
	}
}
