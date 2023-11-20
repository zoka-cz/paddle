using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoka.CSCommon.TypeConverters
{
	/// <summary>The class which implementation should provide format string to the InvariantDataTimeConverter in attribute TypeConverter</summary>
	/// <remarks>The solution taken from https://stackoverflow.com/a/32694825/1038496</remarks>
	public interface IDateTimeConverterFormatProvider
	{
		/// <summary>The format of the DateTime as specified in the parameter to ToString function of DateTime</summary>
		string Format { get; }
	}

	/// <summary>Implementation of IDateTimeConverterFormatProvider which returns format sortable ("O")</summary>
	public class SortableDateTimeFormatProvider : IDateTimeConverterFormatProvider
	{
		/// <inheritdoc />
		public string Format => "O";
	}

	/// <summary>Implementation of IDateTimeConverterFormatProvider which returns format of time ("HH:mm")</summary>
	public class HoursMinutesFormatProvider : IDateTimeConverterFormatProvider
	{
		/// <inheritdoc />
		public string Format => "HH:mm";
	}

	/// <summary>Implementation of IDateTimeConverterFormatProvider which returns format of time ("HH:mm:ss")</summary>
	public class HoursMinutesSecondsFormatProvider : IDateTimeConverterFormatProvider
	{
		/// <inheritdoc />
		public string Format => "HH:mm:ss";
	}

	/// <summary>The converter of the datetime using culture invariant form (ISO6801)</summary>
	/// <remarks>
	///		TFormatProvider allows to specify the format using the generic parameter.
	///		E.g.
	///		[TypeConverter(typeof(InvariantDataTimeConverter&lt;HoursMinutesFormatProvider&gt;))]
	///		public DateTime	SomeProperty { get; set; }
	///		This will use format "HH:mm" for conversions to and from string.
	/// </remarks>
	public class InvariantDataTimeConverter<TFormatProvider> : DateTimeConverter where TFormatProvider : IDateTimeConverterFormatProvider, new()
	{
		private readonly string								m_DateTimeFormat;

		/// <summary>Constructor</summary>
		public InvariantDataTimeConverter()
		{
			m_DateTimeFormat = new TFormatProvider().Format;
		}

		/// <summary>Returns whether this class may convert from _source_type</summary>
		public override bool CanConvertFrom(ITypeDescriptorContext _context, Type _source_type)
		{
			if (_source_type == typeof(string))
				return true;

			return base.CanConvertFrom(_context, _source_type);
		}

		/// <summary>Returns whether this class may convert into the _destination type</summary>
		public override bool CanConvertTo(ITypeDescriptorContext _context, Type _destination_type)
		{
			if (_destination_type == typeof(DateTime) || _destination_type == typeof(DateTime?))
				return true;

			return base.CanConvertTo(_context, _destination_type);
		}

		/// <summary>Will convert string _value into the DateTime? type</summary>
		public override object ConvertFrom(ITypeDescriptorContext _context, CultureInfo _culture, object _value)
		{
			if (_value == null || (_value is string && string.IsNullOrWhiteSpace((string)_value)))
				return (DateTime?)null;

			if (_value is string)
			{
				DateTime res;
				if (DateTime.TryParseExact((string)_value, m_DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out res))
					return res;
			}
			return base.ConvertFrom(_context, _culture, _value);
		}

		/// <summary>Will convert DateTime? value into the string</summary>
		public override object ConvertTo(ITypeDescriptorContext _context, CultureInfo _culture, object _value, Type _destination_type)
		{
			if (_destination_type == typeof(string) && (_value == null || _value is DateTime))
			{
				if (_value == null)
					return "";

				return ((DateTime)_value).ToString(m_DateTimeFormat);
			}
			return base.ConvertTo(_context, _culture, _value, _destination_type);
		}

		/// <summary></summary>
		public override bool IsValid(ITypeDescriptorContext _context, object _value)
		{
			return base.IsValid(_context, _value);
		}
	}
}
