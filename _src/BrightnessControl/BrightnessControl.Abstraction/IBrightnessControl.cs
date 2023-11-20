using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zoka.Paddle.BrightnessControl.Abstraction
{
	/// <summary>Interface for controlling brightness</summary>
	public interface IBrightnessControl
	{
		/// <summary>Will set the brightness in the percentage (0-100, negative values are interpreted as 0, >100 are interpreted as 100)</summary>
		Task SetBrightness(int _brightness_percentage);
		
		/// <summary>Will return current brightness (0-100)</summary>
		Task<int> GetBrightness();
	}
}
