using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Zoka.Paddle.Location.Abstraction;

namespace Zoka.Paddle.Location.Xamarin
{
	/// <summary>Implementation of the ILocationProvider using Xamarin.Essentials features</summary>
	public class XamarinLocationProvider : ILocationProvider
	{
		/// <summary>Will return current location</summary>
		public async Task<Abstraction.Location>					GetCurrentLocation()
		{
			var locationTaskCompletionSource = new TaskCompletionSource<global::Xamarin.Essentials.Location>();

			Device.BeginInvokeOnMainThread(async () =>
			{
				global::Xamarin.Essentials.Location loc = null;
				try
				{
					loc = await Geolocation.GetLastKnownLocationAsync();
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.ToStringAllExceptionDetails());
				}
				locationTaskCompletionSource.SetResult(loc);
			});

			
			var xloc = await locationTaskCompletionSource.Task;

			return new Abstraction.Location()
			{
				Latitude = xloc.Latitude,
				Longitude = xloc.Longitude,
				Altitude = xloc.Altitude,
				Accuracy = xloc.Accuracy,
				IsFromMockProvider = xloc.IsFromMockProvider,
				Timestamp = xloc.Timestamp.LocalDateTime,
			};
		}

	}
}
