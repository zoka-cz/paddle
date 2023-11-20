using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoka.CSCommon
{
	/// <summary>Extensions for DateTime structure</summary>
	public static class DateTimeExtensions
	{
		/// <summary>Will return new instance of DateTime with new date and original time</summary>
		public static DateTime								SetDate(this DateTime _original, DateTime _other_date)
		{
			return new DateTime(_other_date.Year, _other_date.Month, _other_date.Day, _original.Hour, _original.Minute, _original.Second, _original.Millisecond, _original.Kind);
		}

		/// <summary>Will return new instance of DateTime with original date and new time</summary>
		public static DateTime								SetTime(this DateTime _original, DateTime _other_time)
		{
			return new DateTime(_original.Year, _original.Month, _original.Day, _other_time.Hour, _other_time.Minute, _other_time.Second, _other_time.Millisecond, _original.Kind);
		}

		/// <summary>Will join two DateTime structures containing Date and Time into one instance</summary>
		public static DateTime								JoinDateAndTime(DateTime _date, DateTime _time)
		{
			return new DateTime(_date.Year, _date.Month, _date.Day, _time.Hour, _time.Minute, _time.Second, _time.Millisecond);
		}

		/// <summary>Will create the DateTime with just specifying time. Date is not important.</summary>
		public static DateTime								CreateTime(int _hours, int _minutes, int _seconds = 0, int _milliseconds = 0)
		{
			return new DateTime(1, 1, 1, _hours, _minutes, _milliseconds);
		}

		/// <summary>Will get the time from the datetime (unifies the date to non-sense)</summary>
		public static DateTime								GetTime(this DateTime _inst)
		{
			return new DateTime(1, 1, 1, _inst.Hour, _inst.Minute, _inst.Second, _inst.Millisecond);
		}

		/// <summary>
		///		Will return the date of the year, where the year is normalized, so it can be compared to another date, whether it is the same date within any year
		/// </summary>
		/// <remarks>
		///		Exmaple: given 25.3.2015 and 25.3.2000, compared using results of this function are same
		/// </remarks>
		public static DateTime								GetDateInYear(this DateTime _inst)
		{
			return new DateTime(2000 /* must be leap year */, _inst.Month, _inst.Day);
		}

		/// <summary>Will return age of person with given _birthdate valid today</summary>
		public static int									AgeToday(this DateTime _inst, DateTime _birthdate)
		{
			return Age(DateTime.Today, _birthdate);
		}

		/// <summary>Will return the age of person valid to the date passed</summary>
		public static int									Age(DateTime _on_date, DateTime _birthdate)
		{
			int age = _on_date.Year - _birthdate.Year;
			if (_birthdate > _on_date.Date.AddYears(-age))
				age--;
			return age;
		}

		/// <summary>Will return the count of weeks in the year according to the calendar week rule</summary>
		public static int									GetWeeksInYear(int _year, CalendarWeekRule _calendar_week_rule)
		{
			System.Globalization.DateTimeFormatInfo dfi = System.Globalization.DateTimeFormatInfo.CurrentInfo;
			var date1 = new DateTime(_year, 12, 31);
			var cal = dfi.Calendar;
			return cal.GetWeekOfYear(date1,  _calendar_week_rule, dfi.FirstDayOfWeek);
		}

		/// <summary>Will return the copy of passed datetime with seconds (and ms) cut to zero</summary>
		public static DateTime								CutSeconds(DateTime _other)
		{
			var cutted_seconds = new DateTime(_other.Year, _other.Month, _other.Day, _other.Hour, _other.Minute, 0, _other.Kind);
			return cutted_seconds;
		}

		/// <summary>Will return the copy of passed datetime with seconds (and ms) cut to zero, or null if passed value is null</summary>
		public static DateTime?								CutSeconds(DateTime? _other)
		{
			if (!_other.HasValue)
				return null;
			var cutted_seconds = new DateTime(_other.Value.Year, _other.Value.Month, _other.Value.Day, _other.Value.Hour, _other.Value.Minute, 0, _other.Value.Kind);
			return cutted_seconds;
		}

		/// <summary>Will return the start of the week</summary>
		public static DateTime								GetWeekStart(this DateTime _tm, DayOfWeek _week_start_by = DayOfWeek.Monday)
		{
			while (_tm.DayOfWeek != _week_start_by)
			{
				_tm = _tm.AddDays(-1);
			}
			return _tm.Date;
		}

		/// <summary>Will return the date which is end of the week in which the date is present</summary>
		public static DateTime								GetWeekEnd(this DateTime _tm, DayOfWeek _week_ends_by = DayOfWeek.Sunday)
		{
			while (_tm.DayOfWeek != _week_ends_by)
			{
				_tm = _tm.AddDays(1);
			}
			return _tm.Date;
		}

		/// <summary>Will return the start day of the passed year and month</summary>
		public static DateTime								GetMonthStart(int _year, int _month)
		{
			return new DateTime(_year, _month, 1);
		}

		/// <summary>Will return the date of the first day of the passed DateTime</summary>
		public static DateTime								GetMonthStart(this DateTime tm)
		{
			return GetMonthStart(tm.Year, tm.Month);
		}

		/// <summary>Will return the date of the first day of the passed DateTime (or null if the tm is null)</summary>
		public static DateTime?								GetMonthStart(this DateTime? tm)
		{
			return tm?.GetMonthStart();
		}

		/// <summary>Will return the end day of the passed year and month</summary>
		public static DateTime								GetMonthEnd(int _year, int _month)
		{
			return new DateTime(_year, _month, 1).AddMonths(1).AddDays(-1);
		}

		/// <summary>Will return the date of the last day of the passed DateTime</summary>
		public static DateTime								GetMonthEnd(this DateTime tm)
		{
			return GetMonthEnd(tm.Year, tm.Month);
		}

		/// <summary>Will return the date of the last day of the passed DateTime (or null if the tm is null)</summary>
		public static DateTime?								GetMonthEnd(this DateTime? tm)
		{
			return tm?.GetMonthEnd();
		}

		/// <summary>Will return the start of the day with 0:00 set</summary>
		public static DateTime								GetDayStart(this DateTime tm)
		{
			return tm.Date;
		}

		/// <summary>Will return the start of the day with 0:00 set</summary>
		public static DateTime?								GetDayStart(this DateTime? tm)
		{
			return tm?.GetDayStart();
		}

		/// <summary>Will return date and time pointing just one tick before the next day</summary>
		public static DateTime								GetDayEnd(this DateTime tm)
		{
			return tm.Date.AddDays(1).AddTicks(-1);
		}

		/// <summary>Will return date and time pointing just one tick before the next day</summary>
		public static DateTime?								GetDayEnd(this DateTime? tm)
		{
			return tm?.Date.AddDays(1).AddTicks(-1);
		}


		/// <summary>Will return the first day of the specified year</summary>
		public static DateTime								GetYearStartDate(int _year)
		{
			return new DateTime(_year, 1, 1);
		}

		/// <summary>Will return whether the date is beginning of the month</summary>
		public static bool									IsDateBeginOfMonth(this DateTime tm)
		{
			return tm.Day == 1;
		}


		/// <summary>Will return whether the date is beginning of the month</summary>
		public static bool									IsDateBeginOfMonth(this DateTime? tm)
		{
			return tm.HasValue && tm.IsDateBeginOfMonth();
		}

		/// <summary>Will return whether the date is end of the month</summary>
		public static bool									IsDateEndOfMonth(this DateTime tm)
		{
			return tm.Day == DateTime.DaysInMonth(tm.Year, tm.Month);
		}

		/// <summary>Will return whether the date is end of the month</summary>
		public static bool									IsDateEndOfMonth(this DateTime? tm)
		{
			return tm.HasValue && tm.Value.IsDateEndOfMonth();
		}

		/// <summary>Will return the earliest datetime from passed values</summary>
		public static DateTime								GetMinDateTime(params DateTime [] _date_times)
		{
			return _date_times.Min();
		}
		
		/// <summary>Will return the date of the easter sunday for the passed year</summary>
		public static DateTime								EasterSunday(int _for_year)
		{
			int day = 0;
			int month = 0;

			int g = _for_year % 19;
			int c = _for_year / 100;
			int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
			int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

			day = i - ((_for_year + (int)(_for_year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
			month = 3;

			if (day > 31)
			{
				month++;
				day -= 31;
			}

			return new DateTime(_for_year, month, day);
		}


		/// <summary>Will return the date of the Bess und Bettag day</summary>
		public static DateTime								BessUndBettag(int _for_year)
		{
			var date = new DateTime(_for_year, 11, 22);
			while (date.DayOfWeek != DayOfWeek.Wednesday)
			{
				date = date.AddDays(-1);
			}

			return date;
		}

		/// <summary>Will return the latest datetime from passed values</summary>
		public static DateTime								GetMaxDateTime(params DateTime[] _date_times)
		{
			return _date_times.Max();
		}

		/// <summary>Will return whether the DayOfWeek is working day</summary>
		public static bool									IsWorkingDay(this DayOfWeek _day_of_week)
		{
			return !_day_of_week.In(DayOfWeek.Saturday, DayOfWeek.Sunday);
		}

		/// <summary>Will return whether the DayOfWeek is weekend day</summary>
		public static bool									IsWeekend(this DayOfWeek _day_of_week)
		{
			return _day_of_week.In(DayOfWeek.Saturday, DayOfWeek.Sunday);
		}

		/// <summary>Will return the count of working days (all days except Saturday, Sunday) in month</summary>
		public static int									GetWorkingDaysCountInMonth(this DateTime _month)
		{
			var act_date = _month.GetMonthStart();
			int working_days = 0;
			while (act_date <= _month.GetMonthEnd())
			{
				if (act_date.DayOfWeek.IsWorkingDay())
					working_days++;
				act_date = act_date.AddDays(1);
			}
			return working_days;
		}

	}
}
