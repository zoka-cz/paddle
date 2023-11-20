using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;

namespace Zoka.CSCommon
{
	/// <summary>The timespan which allows to add more than 24 hours</summary>
	[TypeConverter(typeof(TimeSpanHMConverter))]
	public struct TimeSpanHM
	{
		private int											m_Hours;
		private int											m_Minutes;

		/// <summary>Sets or gets the hours</summary>
		public int											Hours 
		{
			get { return m_Hours; }
			set { m_Hours = value; }
		}

		/// <summary>Sets or gets minutes</summary>
		public int											Minutes
		{
			get { return m_Minutes; }
			set { m_Minutes = value; }
		}

		/// <summary>Return the total minutes (for hours and minutes)</summary>
		public int											TotalMinutes
		{
			get { return m_Hours * 60 + m_Minutes; }
			set { m_Hours = value / 60; m_Minutes = value % 60; }
		}

		/// <summary>Will return whether the TimeSpanHM is moment (it has zero length)</summary>
		public bool											IsMoment
		{
			get { return TotalMinutes == 0; }
		}

		/// <summary>Constructor setting both values</summary>
		public TimeSpanHM(int _hours, int _minutes)
		{
			m_Hours = _hours;
			m_Minutes = _minutes;
		}

		/// <summary>Constructor setting time span as total number of minutes (i.e. hours are counted from the value)</summary>
		public TimeSpanHM(int _total_minutes)
		{
			m_Hours = 0; m_Minutes = 0;
			TotalMinutes = _total_minutes;
		}

		/// <summary>Constructor setting value from TimeSpan</summary>
		public TimeSpanHM(TimeSpan _time_span)
		{
			m_Hours = 0; m_Minutes = 0;
			TotalMinutes = (int)_time_span.TotalMinutes;
		}

		/// <summary>The explicit conversion</summary>
		public static implicit operator TimeSpanHM(TimeSpan val)
		{
			return new TimeSpanHM(val);
		}

		/// <summary>The explicit string conversion</summary>
		public static implicit operator TimeSpanHM(string val)
		{
			return Parse(val).Value;
		}

		/// <summary>The explicit operator from DateTime (takes just hours and minutes)</summary>
		public static explicit operator TimeSpanHM(DateTime _val)
		{
			return new TimeSpanHM(_val.Hour, _val.Minute);
		}

		/// <summary>Explicit conversion to the timespan</summary>
		public static explicit operator TimeSpan(TimeSpanHM val)
		{
			return new TimeSpan(TimeSpan.TicksPerMinute * val.TotalMinutes);
		}

		/// <summary>Explicit conversion to the DateTime</summary>
		public static explicit operator DateTime(TimeSpanHM val)
		{
			return val.ToDateTime(DateTime.Now.Date);
		}

		/// <summary>Will return the sum of this and other instances of TimeSpanHM</summary>
		public static TimeSpanHM							operator+(TimeSpanHM _first, TimeSpanHM _second)
		{
			return new TimeSpanHM(_first.TotalMinutes + _second.TotalMinutes);
		}

		/// <summary>Will return the subtraction of this and other instances of TimeSpanHM</summary>
		public static TimeSpanHM							operator-(TimeSpanHM _first, TimeSpanHM _second)
		{
			return new TimeSpanHM(_first.TotalMinutes - _second.TotalMinutes);
		}

		/// <summary>Will return negative value of passed instance</summary>
		public static TimeSpanHM							operator-(TimeSpanHM _val_to_negate)
		{
			return new TimeSpanHM(-_val_to_negate.TotalMinutes);
		}

		/// <summary>Compare operator</summary>
		public static bool									operator > (TimeSpanHM _first, TimeSpanHM _second)
		{
			return _first.TotalMinutes > _second.TotalMinutes;
		}

		/// <summary>Compare operator</summary>
		public static bool									operator >= (TimeSpanHM _first, TimeSpanHM _second)
		{
			return _first.TotalMinutes >= _second.TotalMinutes;
		}

		/// <summary>Compare operator</summary>
		public static bool									operator < (TimeSpanHM _first, TimeSpanHM _second)
		{
			return _first.TotalMinutes < _second.TotalMinutes;
		}

		/// <summary>Compare operator</summary>
		public static bool									operator <= (TimeSpanHM _first, TimeSpanHM _second)
		{
			return _first.TotalMinutes <= _second.TotalMinutes;
		}

		/// <summary>Compare operator</summary>
		public static bool									operator == (TimeSpanHM _first, TimeSpanHM _second)
		{
			return _first.TotalMinutes == _second.TotalMinutes;
		}

		/// <summary>Compare operator</summary>
		public static bool									operator != (TimeSpanHM _first, TimeSpanHM _second)
		{
			return _first.TotalMinutes != _second.TotalMinutes;
		}

		/// <summary>Multiple operator</summary>
		public static TimeSpanHM							operator * (TimeSpanHM _first, int _multiple_by)
		{
			return new TimeSpanHM(_first.TotalMinutes * _multiple_by);
		}

		/// <summary>Multiple operator</summary>
		public static TimeSpanHM operator *(int _multiple_by, TimeSpanHM _time_span)
		{
			return new TimeSpanHM(_time_span.TotalMinutes * _multiple_by);
		}

		/// <summary>Percentage</summary>
		public TimeSpanHM									PercentageOfHM(int _percentage)
		{
			return new TimeSpanHM((TotalMinutes * _percentage) / 100);
		}

			/// <summary>Equals override</summary>
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		/// <summary>The hash code</summary>
		public override int									GetHashCode()
		{
			return TotalMinutes;
		}

		/// <summary>Converts to string</summary>
		public override string ToString()
		{
			var prefix = Hours < 0 || Minutes < 0 ? "-" : "";
			return prefix + Math.Abs(Hours).ToString("d2") + ":" + Math.Abs(Minutes).ToString("d2");
		}

		/// <summary>Will convert value to the DateTime as Time on the passed day if possible</summary>
		/// <remarks>The Hours must be in 0-23 range, otherwise the exception is thrown</remarks>
		/// <exception cref="ArgumentOutOfRangeException">Thrown in case the Hours property is out of 0-23 range</exception>
		public DateTime										ToDateTime(DateTime _on_day)
		{
			if (Hours < 0 || Hours > 23)
				throw new ArgumentOutOfRangeException("Hours");
			return new DateTime(_on_day.Year, _on_day.Month, _on_day.Day, this.Hours, this.Minutes, 0);
		}

		/// <summary>Will parse the string value to the TimeSpanHM struct</summary>
		public static TimeSpanHM?							Parse(string _val)
		{
			if (string.IsNullOrEmpty(_val))
				return null;

            var multiplicator = 1;
            if (_val[0] == '-') {
                multiplicator = -1;
                _val = _val.Substring(1);
            }
            
			var tokens = _val.Split(':');
			if (tokens.Length != 2)
				throw new FormatException();

			int hours = Int32.Parse(tokens[0]);
			int minutes = Int32.Parse(tokens[1]);

            int total = ((hours * 60) + minutes) * multiplicator;
            return new TimeSpanHM(total);
		}

	}

	/// <summary> </summary>
	public class TimeSpanHMConverter : TypeConverter
	{
		
		/// <summary> </summary>
		public override	bool								CanConvertFrom(ITypeDescriptorContext _context, Type _source_type)
		{
			if (_source_type == typeof(string))
				return true;

			return base.CanConvertFrom(_context, _source_type);
		}
		/// <summary> </summary>
		public override object								ConvertFrom(ITypeDescriptorContext _context, CultureInfo _culture, object _value)
		{
			if (_value is string)
				return TimeSpanHM.Parse(_value as string);

			return base.ConvertFrom(_context, _culture, _value);
		}
		/// <summary> </summary>
		public override object								ConvertTo(ITypeDescriptorContext _context, CultureInfo _culture, object _value, Type _destination_type)
		{
			if (_destination_type == typeof(string))
				return ((TimeSpanHM)_value).ToString();

			return base.ConvertTo(_context, _culture, _value, _destination_type);
		}
	}
}
