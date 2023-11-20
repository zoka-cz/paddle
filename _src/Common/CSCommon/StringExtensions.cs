using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
	/// <summary>Extensions to the string</summary>
	public static class StringExtensions
	{
		/// <summary>Will safely trim the string, even if the reference is null</summary>
		/// <param name="_string_to_trim">The string which should be trimmed</param>
		/// <returns>Trimmed string, or null if input string was null</returns>
		public static string								SafeTrim(string _string_to_trim)
		{
			if (_string_to_trim == null)
				return null;

			return _string_to_trim.Trim();
		}

		/// <summary>Will remove single starting and/or trailing character only if it is one of the passed _chars_to_be_trimmed</summary>
		public static string								TrimCharacters(this string _string_to_trim, IEnumerable<char> _chars_to_be_trimmed)
		{
			int start_idx = 0, len = _string_to_trim.Length;

			if (string.IsNullOrEmpty(_string_to_trim))
				return _string_to_trim;
			if (_chars_to_be_trimmed.Any(c => c == _string_to_trim[0]))
			{
				start_idx = 1;
				len --;
			}
			if (start_idx < _string_to_trim.Length && _chars_to_be_trimmed.Any(c => c == _string_to_trim[_string_to_trim.Length - 1]))
			{
				len--;
			}

			return _string_to_trim.Substring(start_idx, len);
		}

		/// <summary>Will replace the character at the specified index</summary>
		public static string								ReplaceAt(this string input, int index, char _new_char)
		{
			if (input == null)
			{
				throw new ArgumentNullException(nameof(input));
			}
			var chars = input.ToCharArray();
			chars[index] = _new_char;
			return new string(chars);
		}

		/// <summary>Will parse the string in CSV format and return the tokens splitted</summary>
		/// <param name="_str">The string which should be splitted</param>
		/// <returns>The tokens extracted from the string</returns>
		public static string []								SplitCSV(this string _str)
		{
			Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);

			var matches = csvSplit.Matches(_str);
			string [] result = new string[matches.Count];
			for (int q = 0; q < matches.Count; q++)
				result[q] = matches[q].Value.TrimStart(',');

			return result;
		}

		/// <summary>Will join the tokens into one line CSV with all formatting</summary>
		/// <param name="_tokens">The tokens to join</param>
		/// <param name="_separator">Separator of tokens (, comma by default)</param>
		/// <returns>The all tokens joined according to CSV rules</returns>
		public static string								JoinCSV(string [] _tokens, char _separator = ',')
		{
			StringBuilder sb = new StringBuilder();

			for (int q = 0; q < _tokens.Length; q++)
			{
				string token;

				if (_tokens[q] == null)
					_tokens[q] = "";

				if (_tokens[q].Contains('"'))
					token = _tokens[q].Replace("\"", "\\\"");
				else
					token = _tokens[q];
				if (token.Contains(_separator))
					sb.AppendFormat("\"{0}\"", token);
				else
					sb.Append(token);

				if (q < _tokens.Length - 1)
					sb.Append(_separator);
			}

			return sb.ToString();
		}

		/// <summary>Will join the passed values with the comma</summary>
		public static string								JoinWithComma(object [] _values, char _separator = ',')
		{
			var sb = new StringBuilder();
			for (int i = 0; i < _values.Length; i++)
			{
				sb.Append(_values[i].ToString());
				if (i != _values.Length - 1)
				{
					sb.Append(_separator);
					sb.Append(" ");
				}
			}
			return sb.ToString();
		}

		/// <summary>Will convert the hex string to the byte array</summary>
		public static byte[]								StringToByteArray(this string hex)
		{
			int NumberChars = hex.Length / 2;
			byte[] bytes = new byte[NumberChars];
			using (var sr = new System.IO.StringReader(hex))
			{
				for (int i = 0; i < NumberChars; i++)
					bytes[i] =
					  Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
			}
			return bytes;
		}

		/// <summary>Will convert the byte array to the string</summary>
		public static string								ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
				hex.AppendFormat("{0:x2}", b);
			return hex.ToString();
		}

		/// <summary>Will truncate the string to the max length or if shorter, returns original string</summary>
		public static string								TruncateLongString(this string _str, int _max_length)
		{
			return _str?.Substring(0, Math.Min(_str.Length, _max_length));
		}
	}
}
