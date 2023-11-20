using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
	/// <summary> </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Creates a string from the sequence by concatenating the result
		/// of the specified string selector function for each element.
		/// </summary>
		public static string ToConcatenatedString<T>(this IEnumerable<T> source,
			Func<T, string> stringSelector)
		{
			return source.ToConcatenatedString(stringSelector, String.Empty);
		}

		/// <summary>
		/// Creates a string from the sequence by concatenating the result
		/// of the specified string selector function for each element.
		/// </summary>
		/// <param name="source"></param>
		///<param name="separator">The string which separates each concatenated item.</param>
		/// <param name="stringSelector"></param>
		public static string ToConcatenatedString<T>(this IEnumerable<T> source,
			Func<T, string> stringSelector,
			string separator)
		{
			var b = new StringBuilder();
			bool needsSeparator = false; // don't use for first item

			foreach (var item in source)
			{
				if (needsSeparator)
					b.Append(separator);

				b.Append(stringSelector(item));
				needsSeparator = true;
			}

			return b.ToString();
		}

		/// <summary>Returns whether the first collection is fully equal to other collection</summary>
		/// <remarks>
		///		Taken from https://stackoverflow.com/a/630278/1038496
		///		Two collections are equivalent if they have the same elements in the same quantity, but in any order.   
		///		Elements are equal if their values are equal, not if they refer to the same object.
		/// </remarks>
		public static bool									IsEqualTo<T>(this IEnumerable<T> _a, IEnumerable<T> _b)
		{
			return !_a.Except(_b).Any() && !_b.Except(_a).Any();
		}


		/// <summary>Will return the enumerator which returns none items</summary>
		public static IEnumerator<T>						EmptyEnumerator<T>()
		{
			yield break;
		}
		
		/// <summary>Will return the enumerator which returns none items</summary>
		public static IEnumerator							EmptyEnumerator()
		{
			yield break;
		}

		/// <summary>Will return the enumerable containing just single (passed) item</summary>
		public static IEnumerable<T>						Yield<T>(this T _item)
		{
			yield return _item;
		}

		/// <summary>Will return the array containing single passed item</summary>
		public static T[]									AsArray<T>(this T _item)
		{
			return new T[1] { _item };
		}
	}
}
