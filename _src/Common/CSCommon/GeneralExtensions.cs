using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
	/// <summary>General extenstions of C#</summary>
	public static class GeneralExtensions
	{
		/// <summary>Will return whether the value of _item is any of the values from _items</summary>
		public static bool									In<T>(this T _item, params T[] _items)
		{
			if (_items == null)
				throw new ArgumentNullException(nameof(_items));

			return _items.Contains(_item);
		}

		/// <summary>Will return whether the value of _item is any of the values from _items using passed comparer</summary>
		public static bool									In<T>(this T _item, IEqualityComparer<T> _comparer, params T[] _items)
		{
			if (_items == null)
				throw new ArgumentNullException(nameof(_items));

			return _items.Contains(_item, _comparer);
		}

	}
}
