using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoka.Paddle.ConsoleInput;
using Zoka.Paddle.Location.Abstraction;

namespace Zoka.Paddle.Location.Fake
{
	/// <summary></summary>
	public abstract class ConsoleInputLocationProvider : ILocationProvider, ICommandLineHandler
	{
		/// <summary></summary>
		public class FakeLocation : Abstraction.Location
		{
			/// <summary></summary>
			public string Description { get; set; }

			/// <inheritdoc />
			public override string ToString()
			{
				return $"{Description}: Lat: {Latitude:##.#####} Lon: {Longitude:##.#####}";
			}
		}


		private FakeLocation								m_CurrentLocation = null;

		/// <inheritdoc />
		public Task<Zoka.Paddle.Location.Abstraction.Location> GetCurrentLocation()
		{
			return Task.FromResult((Abstraction.Location)m_CurrentLocation);
		}

		/// <inheritdoc />
		public string Command => "loc";
		/// <inheritdoc />
		public string ShortCommand => "l";
		/// <inheritdoc />
		public string HelpText => "Changes current location";

		/// <summary>Called, when Command is typed in the console. This function may perform multiple tasks.</summary>
		/// <param name="_wait_for_line_input_callback">Callback which waits until user inputs full line, which is returned</param>
		/// <param name="_wait_for_key_input_callback">Callback which waits until user press one key, which is returned</param>
		/// <param name="_service_provider">Service provider</param>
		/// <returns>true, when the console loop should be finished, false otherwise</returns>
		public bool StartProcessing(Func<string> _wait_for_line_input_callback, Func<ConsoleKeyInfo> _wait_for_key_input_callback, IServiceProvider _service_provider)
		{
			var locations = GetLocations(_service_provider).ToList();
			if (m_CurrentLocation == null)
				Console.Write("Current location: not set yet.");
			else
				Console.Write($"Current location: {m_CurrentLocation}.");
			Console.WriteLine(" Enter number with new location.");
			var i = 0;
			foreach (var fake_location in locations)
			{
				Console.WriteLine($" {i++}) {fake_location}");
			}
			var snumber = _wait_for_line_input_callback();
			if (!int.TryParse(snumber, out var idx))
			{
				Console.WriteLine("Invalid input (not a number)");
				return false;
			}
			if (idx < 0 || idx >= locations.Count)
			{
				Console.WriteLine("Invlaid input (enter one of listed numbers)");
				return false;
			}

			m_CurrentLocation = locations[idx];
			return false;
		}

		/// <summary></summary>
		protected abstract IEnumerable<FakeLocation>		GetLocations(IServiceProvider _service_provider);

	}
}
