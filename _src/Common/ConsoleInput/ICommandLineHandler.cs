using System;
using System.Collections.Generic;
using System.Text;

namespace Zoka.Paddle.ConsoleInput
{
	/// <summary>Handles the command received from the console input</summary>
	public interface ICommandLineHandler
	{
		/// <summary>The command which, when received, calls StartProcessing of the implementing class.</summary>
		/// <remarks>The command is case insensitive.</remarks>
		string Command { get; }

		/// <summary>The short for command</summary>
		string ShortCommand { get; }

		/// <summary>The one line help for this command.</summary>
		/// <remarks>E.g. "Write quit for quiting the application"</remarks>
		string HelpText { get; }

		/// <summary>Called, when Command is typed in the console. This function may perform multiple tasks.</summary>
		/// <param name="_wait_for_line_input_callback">Callback which waits until user inputs full line, which is returned</param>
		/// <param name="_wait_for_key_input_callback">Callback which waits until user press one key, which is returned</param>
		/// <param name="_service_provider">Service provider</param>
		/// <returns>true, when the console loop should be finished, false otherwise</returns>
		bool StartProcessing(Func<string> _wait_for_line_input_callback, Func<ConsoleKeyInfo> _wait_for_key_input_callback, IServiceProvider _service_provider);
	}
}
