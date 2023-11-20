using System;
using System.Collections.Generic;
using System.Text;

namespace Zoka.Paddle.Location.Abstraction
{
	/// <summary>Container for location information</summary>
	public class Location
	{
		/// <summary>Latitude</summary>
		public double										Latitude { get; set; }
		/// <summary>Longitude</summary>
		public double										Longitude { get; set; }
		/// <summary>Altitude</summary>
		public double?										Altitude { get; set; }
		/// <summary>Accuracy</summary>
		public double?										Accuracy { get; set; }
		/// <summary>When true, the value comes from some mock provider and may not be correct or may be faked</summary>
		public bool											IsFromMockProvider { get; set; }
		/// <summary>Timestamp when this location was measured</summary>
		public DateTime										Timestamp { get; set; }
	}
}
