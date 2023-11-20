using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoka.CSCommon
{
	/// <summary> 
	/// Following picture displays all the possible overlaps with all masks and finite values
	/// 
    ///       |-------------------|          compare to this one
	///       
    ///           |---------|                OtherContainedWithin								OtherContainedWithin
    ///       |----------|                   OtherContainedWithin | EqualStart					OtherContainedWithinEqualStart
    ///               |-----------|          OtherContainedWithin | EqualEnd					OtherContainedWithinEqualEnd
    ///       |-------------------|          OtherContainedWithin | EqualStart | EqualEnd		EqualRanges
    /// |------------|                       OtherPartlyContainedWithin | OverlapsStart			OtherPartlyContainedWithOverlappingStart
    ///               |---------------|      OtherPartlyContainedWithin | OverlapsEnd			OtherPartlyContainedWithOverlappingEnd
    /// |-------------------------|          OtherOverlaps | OverlapsStart						OtherOverlapsWithStart
    ///       |-----------------------|      OtherOverlaps | OverlapsEnd						OtherOverlapsWithEnd
    /// |------------------------------|     OtherOverlaps | OverlapsStart | OverlapsEnd		OtherOverlapsFully
	/// |---|                                OtherEndsBefore									OtherEndsBefore
	///                             |---|    OtherStartsAfter									OtherStartsAfter
	/// </summary>
	public enum ERangeOverlappingMasks
	{
		/// <summary>Other range is contained within this</summary>
		OtherContainedWithin								= 0x00000001,
		/// <summary>Other range partly contained within this</summary>
		OtherPartlyContainedWithin							= 0x00000002,
		/// <summary>Other range overlaps with this range fully</summary>
		OtherOverlaps										= 0x00000004,
		/// <summary>Other range starts and end before this</summary>
		OtherEndsBefore										= 0x00000008,
		/// <summary>Other range starts and end after this</summary>
		OtherStartsAfter									= 0x00000010,

		/// <summary>Both ranges have equal starts</summary>
		EqualStarts											= 0x00010000,
		/// <summary>Both ranges have equal ends</summary>
		EqualEnds											= 0x00020000,
		/// <summary>Other range overlaps start of this (starts before this)</summary>
		OverlapsStart										= 0x00040000,
		/// <summary>Other range overlaps end of this (ends after this)</summary>
		OverlapsEnd											= 0x00080000,
	}
	/// <summary> 
	/// Following picture displays all the possible overlaps with all masks and finite values
	/// 
	///       |-------------------|          compare to this one
	///       
	///           |---------|                OtherContainedWithin								OtherContainedWithin
	///       |----------|                   OtherContainedWithin | EqualStart					OtherContainedWithinEqualStart
	///               |-----------|          OtherContainedWithin | EqualEnd					OtherContainedWithinEqualEnd
	///       |-------------------|          OtherContainedWithin | EqualStart | EqualEnd		EqualRanges
	/// |------------|                       OtherPartlyContainedWithin | OverlapsStart			OtherPartlyContainedWithOverlappingStart
	///               |---------------|      OtherPartlyContainedWithin | OverlapsEnd			OtherPartlyContainedWithOverlappingEnd
	/// |-------------------------|          OtherOverlaps | OverlapsStart						OtherOverlapsWithStart
	///       |-----------------------|      OtherOverlaps | OverlapsEnd						OtherOverlapsWithEnd
	/// |------------------------------|     OtherOverlaps | OverlapsStart | OverlapsEnd		OtherOverlapsFully
	/// |---|                                OtherEndsBefore									OtherEndsBefore
	///                             |---|    OtherStartsAfter									OtherStartsAfter
	/// </summary>
	public enum ERangeOverlapping
	{
		/// <summary>Other range is fully contained within this</summary>
		OtherContainedWithin								= 0x00000001,
		/// <summary>Other range starts as this but end earlier</summary>
		OtherContainedWithinEqualStart						= 0x00010001,
		/// <summary>Other range starts after this and end same as this</summary>
		OtherContainedWithinEqualEnd						= 0x00020001,
		/// <summary>Both ranges are equal</summary>
		EqualRanges											= 0x00030001,
		/// <summary>Other starts before this and ends before this</summary>
		OtherPartlyContainedWithOverlappingStart			= 0x00040002,
		/// <summary>Other range starts after this starts but last longer</summary>
		OtherPartlyContainedWithOverlappingEnd				= 0x00080002,
		/// <summary>Other range starts before this and ends as this</summary>
		OtherOverlapsWithStart								= 0x00040004,
		/// <summary>Both ranges starts same but other ends later</summary>
		OtherOverlapsWithEnd								= 0x00080004,
		/// <summary>Other range start before this and ends after this</summary>
		OtherOverlapsFully									= 0x000C0004,
		/// <summary>Other starts and ends before this</summary>
		OtherEndsBefore										= 0x00000008,
		/// <summary>Other range starts and ends after this</summary>
		OtherStartsAfter									= 0x00000010,
	}

	/// <summary>This class represents the date time range specified by start and end of the range</summary>
	public class DateTimeRange
	{
		#region Public properties

		private DateTime									m_RangeStart;
		/// <summary>The start of the range</summary>
		public DateTime										RangeStart 
		{ 
			get { return m_RangeStart; }
		}

		private DateTime									m_RangeEnd;
		/// <summary>The end of the range</summary>
		public DateTime										RangeEnd 
		{ 
			get { return m_RangeEnd; }
		}

		#endregion // Public properties

		#region Construction

		/// <summary>Constructor</summary>
		/// <remarks>if _range_start if later than _range_end, these values are swapped</remarks>
		public DateTimeRange(DateTime _range_start, DateTime _range_end)
		{
			if (_range_start > _range_end)
			{
				DateTime tmp = _range_start;
				_range_start = _range_end;
				_range_end = tmp;
			}

			m_RangeStart = _range_start;
			m_RangeEnd = _range_end;
		}

		#endregion // Construction

		#region Calculated properties

		/// <summary>Will return the length of the DateTimeRange as TimeSpan</summary>
		public TimeSpan										Length
		{ 
			get
			{
				return RangeEnd.Subtract(RangeStart);
			}
		}

		#endregion // Calculated properties

		#region Public functions

		/// <summary>
		///	Will compare two ranges and return the way how the ranges overlaps together (see picture below)
		///       |-------------------|          compare to this one
		///       
		///           |---------|                OtherContainedWithin								OtherContainedWithin
		///       |----------|                   OtherContainedWithin | EqualStart					OtherContainedWithinEqualStart
		///               |-----------|          OtherContainedWithin | EqualEnd					OtherContainedWithinEqualEnd
		///       |-------------------|          OtherContainedWithin | EqualStart | EqualEnd		EqualRanges
		/// |------------|                       OtherPartlyContainedWithin | OverlapsStart			OtherPartlyContainedWithOverlappingStart
		///               |---------------|      OtherPartlyContainedWithin | OverlapsEnd			OtherPartlyContainedWithOverlappingEnd
		/// |-------------------------|          OtherOverlaps | OverlapsStart						OtherOverlapsWithStart
		///       |-----------------------|      OtherOverlaps | OverlapsEnd						OtherOverlapsWithEnd
		/// |------------------------------|     OtherOverlaps | OverlapsStart | OverlapsEnd		OtherOverlapsFully
		/// |---|                                OtherEndsBefore									OtherEndsBefore
		///                             |---|    OtherStartsAfter									OtherStartsAfter
		/// </summary>
		/// <param name="_other">The other range compared with this range</param>
		/// <returns>
		///		The way how the ranges overlaps. 
		///		Returned as int, but return value is from ERangeOverlapping enumeration and can be decomposed using ERangeOverlappingMasks enumeration
		/// </returns>
		public int											Overlaps(DateTimeRange _other)
		{
			if (_other.RangeStart < this.RangeStart)
			{
				if (_other.RangeEnd <= this.RangeStart)
					return (int)ERangeOverlapping.OtherEndsBefore;
				else if (_other.RangeEnd < this.RangeEnd)
					return (int)ERangeOverlapping.OtherPartlyContainedWithOverlappingStart;
				else if (_other.RangeEnd == this.RangeEnd)
					return (int)ERangeOverlapping.OtherOverlapsWithStart;
				else
					return (int)ERangeOverlapping.OtherOverlapsFully;
			} 
			else if (_other.RangeStart > this.RangeStart)
			{
				if (_other.RangeStart >= this.RangeEnd)
					return (int)ERangeOverlapping.OtherStartsAfter;
				else if (_other.RangeEnd < this.RangeEnd)
					return (int)ERangeOverlapping.OtherContainedWithin;
				else if (_other.RangeEnd == this.RangeEnd)
					return (int)ERangeOverlapping.OtherContainedWithinEqualEnd;
				else
					return (int)ERangeOverlapping.OtherPartlyContainedWithOverlappingEnd;
			}
			else
			{
				if (_other.RangeEnd < this.RangeEnd)
					return (int)ERangeOverlapping.OtherContainedWithinEqualStart;
				else if (_other.RangeEnd == this.RangeEnd)
					return (int)ERangeOverlapping.EqualRanges;
				else
					return (int)ERangeOverlapping.OtherOverlapsWithEnd;
			}
		}

		/// <summary>Will return the range in which this range overlaps with another</summary>
		/// <param name="_other">The other date time range to find overlapping with this one</param>
		/// <returns>The range in which the both ranges are overlapping, or null if these two ranges do not overlap</returns>
		public DateTimeRange								OverlappingRange(DateTimeRange _other)
		{
			var overlapping_type = Overlaps(_other);
			if ((overlapping_type & (int)ERangeOverlappingMasks.OtherEndsBefore) > 0 ||
				(overlapping_type & (int)ERangeOverlappingMasks.OtherStartsAfter) > 0)
				return null; // not overlapping

			var start = this.RangeStart > _other.RangeStart ? this.RangeStart : _other.RangeStart;
			var end = this.RangeEnd > _other.RangeEnd ? this.RangeEnd : _other.RangeEnd;

			return new DateTimeRange(start, end);
		}

		#endregion // Public functions

		#region Comparers

		/// <summary>The comparer for comparing the DateTimeRanges by their length</summary>
		public class DateTimeRangeLengthComparer : IComparer<DateTimeRange>
		{
			#region IComparer<DateTimeRange> Members

			/// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
			/// <param name="x">The first object to compare.</param>
			/// <param name="y">The second object to compare.</param>
			/// <returns>
			///		A signed integer that indicates the relative values of x and y, as shown in the following table.
			/// 	Value							Meaning 
			/// 	------------------------------------------------------
			/// 	Less than zero					x is shorter than y.
			/// 	Zero							x has same length as y.
			/// 	Greater than zero				x is longer than y.
			/// </returns>
			public int Compare(DateTimeRange x, DateTimeRange y)
			{
				return (int)(x.Length.Ticks - y.Length.Ticks);
			}

			#endregion
		}

		/// <summary>The comparer for comparing DateTimeRanges by starting in the time</summary>
		public class DateTimeRangeStartComparer : IComparer<DateTimeRange>
		{
			#region IComparer<DateTimeRange> Members

			/// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
			/// <param name="x">The first object to compare.</param>
			/// <param name="y">The second object to compare.</param>
			/// <returns>
			///		A signed integer that indicates the relative values of x and y, as shown in the following table.
			/// 	Value							Meaning 
			/// 	------------------------------------------------------
			/// 	Less than zero					x starts or at least ends earlier y.
			/// 	Zero							both ranges starts and ends at same times
			/// 	Greater than zero				x starts or at least ends later then y.
			/// </returns>
			public int Compare(DateTimeRange x, DateTimeRange y)
			{
				if (x.RangeStart < y.RangeStart)
					return -1;
				else if (x.RangeStart > y.RangeStart)
					return 1;
				else
				{
					return (int)(x.RangeEnd.Ticks - y.RangeEnd.Ticks);
				}
			}

			#endregion
		}

		#endregion // Comparers
	}
}
