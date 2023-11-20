using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zoka.Paddle.ConsoleInput;

namespace ConsoleInput
{
	/// <summary>Handler of commands inputed through the console.</summary>
	public class ConsoleInputHandler
	{
		private readonly List<ICommandLineHandler>			m_CommandLineHandlers = new List<ICommandLineHandler>();

		/// <summary>Will register any ICommandLineHandler registered in the IServiceProvider as ICommandLineHandler</summary>
		/// <remarks>The implementation of the ICommandLineHandler must be registered as services.AddSingleton&lt;ICommandLineHandler, ImplType&gt;()</remarks>
		public void											RegisterCommandLineHandlers(IServiceProvider _service_provider)
		{
			var handlers = _service_provider.GetServices<ICommandLineHandler>();
			m_CommandLineHandlers.AddRange(handlers);
		}


		/// <summary>Will listen to user input, and when user enters the command (by confirming by enter) it pass processing to the appropriate ICommandLineHandler.</summary>
		/// <remarks>Blocking call. Function does not finish until any of the ICommandLineHandlers.StartProcessing returns true.</remarks>
		public void											StartProcessing(IServiceProvider _service_provider)
		{
			while (true)
			{
				Console.WriteLine($"Available commands:");
				foreach (var command_line_handler in m_CommandLineHandlers)
				{
					Console.Write($" {command_line_handler.Command}");
					if (command_line_handler.ShortCommand != null)
					{
						Console.Write($" or {command_line_handler.ShortCommand}");
					}
					Console.WriteLine($" - {command_line_handler.HelpText}");
				}

				var cmd = Console.ReadLine();
				var cmd_handler = m_CommandLineHandlers.FirstOrDefault(h => string.Compare(h.Command, cmd, StringComparison.InvariantCultureIgnoreCase) == 0);
				if (cmd_handler == null)
					cmd_handler = m_CommandLineHandlers.FirstOrDefault(h => string.Compare(h.ShortCommand, cmd, StringComparison.InvariantCultureIgnoreCase) == 0);
				if (cmd_handler != null)
				{
					var res = cmd_handler.StartProcessing(Console.ReadLine, Console.ReadKey, _service_provider);
					if (res)
					{
						Console.WriteLine($"Command {cmd_handler.Command} requested end of console input listening.");
						break;
					}
				}
				else
				{
					Console.WriteLine($"Invalid command entered.");
				}
			}
		}
	}
}
