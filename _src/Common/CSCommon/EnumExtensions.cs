using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
	/// <summary>The extensions for Enum class</summary>
	public static class EnumExtensions
	{
		/// <summary>Will cast integer to the EnumValue with value check</summary>
		public static TEnumType								FromInt<TEnumType>(int _val) where TEnumType: struct, IConvertible
		{
			if (!typeof(TEnumType).IsEnum)
				throw new ArgumentException();
			if (Enum.IsDefined(typeof(TEnumType), _val))
				return (TEnumType)Enum.ToObject(typeof(TEnumType), _val);
			else
				throw new ArgumentOutOfRangeException();
		}

		/// <summary>Will cast nullable integer to the EnumValue with value check</summary>
		public static TEnumType?							FromNullableInt<TEnumType>(int? _val) where TEnumType: struct, IConvertible
		{
			if (!_val.HasValue)
				return null;

			return FromInt<TEnumType>(_val.Value);
		}

		/// <summary>Will return the localized value of enum from the resource type passed, if it exists. Otherwise it returns the enum value name.</summary>
		public static string								LocalizedValue<TEnumType>(this TEnumType _enum_value, Type _resource_type) where TEnumType: struct, IConvertible
		{
			var enum_name = _enum_value.ToString();

			try
			{
				var res_mgr = new Resources.ResourceManager(_resource_type);
				var enum_localized = res_mgr.GetString(enum_name);
				if (enum_localized != null)
					return enum_localized;
			}
			catch { }

			return enum_name;
		}
	}
}
