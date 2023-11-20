using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
	/// <summary> </summary>
	public static class ExceptionExtension
	{
		/// <summary> </summary>
		private static void WriteExceptionDetails(Exception exception, StringBuilder builderToFill, int level)
		{
			var indent = new string(' ', level);

			if (level > 0)
			{
				builderToFill.AppendLine(indent + "=== INNER EXCEPTION ===");
			}

			Action<string> append = (prop) => {
				var propInfo = exception.GetType().GetProperty(prop);
				var val = propInfo != null ? propInfo.GetValue(exception, null) : null;
				if (val != null)
				{
					builderToFill.AppendFormat("{0}{1}: {2}{3}", indent, prop, val.ToString(), Environment.NewLine);
				}
			};

			append("Message");
			append("HResult");
			append("HelpLink");
			append("Source");
			append("StackTrace");
			append("TargetSite");

			foreach (System.Collections.DictionaryEntry de in exception.Data)
			{
				builderToFill.AppendFormat("{0} {1} = {2}{3}", indent, de.Key, de.Value, Environment.NewLine);
			}

			if (exception.InnerException != null)
			{
				WriteExceptionDetails(exception.InnerException, builderToFill, ++level);
			}
		}

		/// <summary> </summary>
		public static string								ToStringAllExceptionDetails(this Exception exception) 
		{
			try
			{
				StringBuilder builderToFill = new StringBuilder();
				WriteExceptionDetails(exception, builderToFill, 0);
				return builderToFill.ToString();
			}
			catch (Exception ex)
			{
				return "Error serializing exception." + ex.Message;
			}
		}
	}
}
