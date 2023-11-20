using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Zoka.Paddle.BrightnessControl.Abstraction;
using Xamarin.Essentials;
using Android.Views;

namespace Zoka.Paddle.BrightnessControl.Android
{
	/// <summary>Implementation of the IBrightnessControl for Android system</summary>
	public class AndroidBrightnessControl : IBrightnessControl
	{
		/// <inheritdoc />
		public Task SetBrightness(int _brightness_percentage)
		{
			//_brightness_percentage = 80;
			Platform.CurrentActivity.RunOnUiThread(() =>
			{
				var window = Platform.CurrentActivity.Window;
				var attributesWindow = new WindowManagerLayoutParams();

				attributesWindow.CopyFrom(window.Attributes);
				attributesWindow.ScreenBrightness = (float)_brightness_percentage / 100;

				window.Attributes = attributesWindow;
			});

			return Task.CompletedTask;
		}

		private int b;

		/// <inheritdoc />
		public Task<int> GetBrightness()
		{
			Platform.CurrentActivity.RunOnUiThread(() =>
			{
				var window = Platform.CurrentActivity.Window;
				Debug.WriteLine($"window {window != null}");
				var attributesWindow = new WindowManagerLayoutParams();

				attributesWindow.CopyFrom(window.Attributes);
				var brightness = attributesWindow.ScreenBrightness;

				window.Attributes = attributesWindow;
				b = (int)Math.Round(brightness * 100);
			});


			return Task.FromResult(b);
		}
	}
}