using System;
using System.Threading.Tasks;
using Zoka.Paddle.BrightnessControl.Abstraction;

namespace Zoka.Paddle.ConsoleFakeBrightnessControl
{
	/// <summary>Brightness control, which only outputs set value to the console</summary>
	public class ConsoleFakeBrightnessControl : IBrightnessControl
	{
		private int											m_BrightnessLevel = 50;


		/// <inheritdoc />
		public Task											SetBrightness(int _brightness_percentage)
		{
			if (_brightness_percentage < 0)
				_brightness_percentage = 0;
			if (_brightness_percentage > 100)
				_brightness_percentage = 100;


			m_BrightnessLevel = _brightness_percentage;
			Console.WriteLine($"Setting brightness to {m_BrightnessLevel}");
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task<int>									GetBrightness()
		{
			return Task.FromResult(m_BrightnessLevel);
		}
	}
}
