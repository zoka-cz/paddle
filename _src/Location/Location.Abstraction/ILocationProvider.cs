using System.Threading.Tasks;

namespace Zoka.Paddle.Location.Abstraction
{
	/// <summary>Interface for providing current device location</summary>
	public interface ILocationProvider
	{
		/// <summary>Will return current location</summary>
		Task<Location> GetCurrentLocation();
	}
}
