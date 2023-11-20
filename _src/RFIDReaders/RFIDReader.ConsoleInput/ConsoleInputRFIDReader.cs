using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zoka.Paddle.ConsoleInput;
using Zoka.Paddle.RFIDReader.Abstraction;

namespace Zoka.Paddle.RFIDReader.ConsoleInput
{
	/// <summary>Implementation of the IRFIDReader, which reads the RFID from console input and emulates the real RFID reader</summary>
	public abstract class ConsoleInputRFIDReader : IRFIDReader, ICommandLineHandler
	{
		/// <summary>The RFID input</summary>
		public class RFIDInput
		{
			/// <summary>The RFID</summary>
			public long										RFID { get; }
			/// <summary>Description of this RFID (like name of owning person, ...)</summary>
			public string									Description { get; }

			/// <summary>Constructor</summary>
			public RFIDInput(long _rfid, string _description)
			{
				RFID = _rfid;
				Description = _description;
			}
		}

		/// <summary>Constructor</summary>
		public ConsoleInputRFIDReader()
		{

		}

		/// <inheritdoc />
		public void											Initialize()
		{
		}

		/// <inheritdoc />
		public void											StartListening()
		{
			// nothing to do here
		}

		/// <inheritdoc />
		public void											StopListening()
		{
			// nothing to do here
		}

		/// <inheritdoc />
		public event RFIDReadHandler						OnRFIDRead;

		/// <summary>Will raise the OnRFIDRead event</summary>
		protected virtual void								RaiseOnRFIDRead(long _rfid)
		{
			OnRFIDRead?.Invoke(_rfid);
		}

		/// <inheritdoc />
		public string										Command => "rfid";

		/// <inheritdoc />
		public string										ShortCommand => "r";

		/// <inheritdoc />
		public string										HelpText => "simulates RFID punching";

		/// <inheritdoc />
		public bool											StartProcessing(Func<string> _wait_for_line_input_callback, Func<ConsoleKeyInfo> _wait_for_key_input_callback, IServiceProvider _service_provider)
		{
			using (var scoped_services = _service_provider.CreateScope())
			{
				var rfids = GetRFIDs(scoped_services.ServiceProvider).ToList();
				if (rfids.Count == 0)
				{
					Console.WriteLine($"No RFIDs provided from GetRFIDs function.");
					return false;
				}
				Console.WriteLine($"Which RFID should be \"punched\"? {(rfids.Count <= 10 ? "Press" : "Enter")} number of RFID or Esc to cancel:");
				for (int i = 0; i < rfids.Count; i++)
				{
					Console.WriteLine($" {i}) {rfids[i].Description} ({rfids[i].RFID})");
				}

				RFIDInput selected_rfid = null;

				if (rfids.Count <= 10)
				{
					while (selected_rfid == null)
					{
						var key_info = _wait_for_key_input_callback();
						if (key_info.Key == ConsoleKey.Escape)
							return false;
						if (key_info.KeyChar >= '0' && key_info.KeyChar <= '0' + rfids.Count)
							selected_rfid = rfids[key_info.KeyChar - '0'];
						else
						{
							Console.WriteLine("Invalid number pressed. Try again or press Esc to cancel");
						}
					}
				}
				else
				{
					while (selected_rfid == null)
					{
						string snumber = "";
						while (true)
						{
							var key_info = _wait_for_key_input_callback();
							if (key_info.Key == ConsoleKey.Escape)
								return false;
							if (key_info.Key == ConsoleKey.Enter)
								break;
							if (char.IsDigit(key_info.KeyChar))
								snumber += key_info.KeyChar;
							else
							{
								snumber = "";
								break;
							}
						}

						if (snumber == "")
						{
							Console.WriteLine("Invalid input. Try again or press Esc to cancel");
							continue;
						}

						if (!int.TryParse(snumber, out var number))
						{
							Console.WriteLine("Invalid input - not converted to int number. try again or press Esc to cancel");
							continue;
						}

						if (number < 0 && number >= rfids.Count)
						{
							Console.WriteLine($"Invalid input - out of range (0-{rfids.Count}). Try again or press Esc to cancel");
							continue;
						}

						selected_rfid = rfids[number];
					}
				}

				Console.WriteLine($"Sending \"punch\" for {selected_rfid.Description} ({selected_rfid.RFID})");
				RaiseOnRFIDRead(selected_rfid.RFID);
			}

			return false;
		}

		/// <summary>Returns the list of RFIDs which can be simulated</summary>
		protected abstract IEnumerable<RFIDInput>			GetRFIDs(IServiceProvider _service_provider);
	}
}
