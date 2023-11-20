using System;
using System.Collections.Generic;
using System.Text;

namespace Zoka.Paddle.PlatformConfiguration.Abstraction
{
	/// <summary>Information about device</summary>
	public class DeviceInformation
	{
		/// <summary>Constructor</summary>
		public DeviceInformation(string _model, string _manufacturer, string _device_name, string _version, string _platform, string _idiom, string _device_type)
		{
			Model = _model;
			Manufacturer = _manufacturer;
			DeviceName = _device_name;
			Version = _version;
			Platform = _platform;
			Idiom = _idiom;
			DeviceType = _device_type;
		}

		/// <summary>Model</summary>
		public string										Model { get; }
		/// <summary>Manufacturer</summary>
		public string										Manufacturer { get; }
		/// <summary>Name of the device</summary>
		public string										DeviceName { get; }
		/// <summary>Version</summary>
		public string										Version { get; }
		/// <summary>Platform</summary>
		public string										Platform { get; }
		/// <summary>Idiom</summary>
		public string										Idiom { get; }
		/// <summary>DeviceType</summary>
		public string										DeviceType { get; }
	}
}
