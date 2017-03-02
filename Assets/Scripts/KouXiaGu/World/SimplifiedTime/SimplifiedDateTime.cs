using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.SimplifiedTime
{

    /// <summary>
    /// 简化版的 System.DateTime,但是实现原理不同;
    /// 仅有年月日;
    /// </summary>
    public struct SimplifiedDateTime : IEquatable<SimplifiedDateTime>, IComparable<SimplifiedDateTime>
    {

        /// <summary>
        /// 一年一月一日;
        /// </summary>
        const int DEFAULT_TICKS = 0x10101;

        /// <summary>
        /// 一年一月一日,默认的日历;
        /// </summary>
        public static SimplifiedDateTime Default
        {
            get { return new SimplifiedDateTime(Calendar.Default, DEFAULT_TICKS); }
        }



        public SimplifiedDateTime(Calendar Calendar)
        {
            this.Calendar = Calendar;
            this.Ticks = DEFAULT_TICKS;
        }

        public SimplifiedDateTime(Calendar Calendar, int ticks)
        {
            this.Calendar = Calendar;
            this.Ticks = ticks;
        }

        public SimplifiedDateTime(Calendar Calendar, short year, byte month, byte day)
        {
            this.Calendar = Calendar;
            this.Ticks = DEFAULT_TICKS;
            this.Year = year;
            this.Month = month;
            this.Day = day;
        }


        /// <summary>
        /// 周期数;年占用前两个字节,其后到月占用一字节,日占用一字节;
        /// </summary>
        public int Ticks { get; set; }

        /// <summary>
        /// 日历;
        /// </summary>
        public Calendar Calendar { get; private set; }


        /// <summary>
        /// 年; -32,768 到 32,767
        /// </summary>
        public short Year
        {
            get { return (short)(Ticks >> 16); }
            set { Ticks = (Ticks & 0xFFFF) | ((int)value << 16); }
        }

        /// <summary>
        /// 月; 1 至 n;
        /// </summary>
        public byte Month
        {
            get { return (byte)((Ticks & 0xFF00) >> 8); }
            set { Ticks = (Ticks & -0xFF01) | (value << 8); }
        }

        /// <summary>
        /// 日; 1 至 n;
        /// </summary>
        public byte Day
        {
            get { return (byte)(Ticks & 0xFF); }
            set { Ticks = (Ticks & -0x100) | value; }
        }


        /// <summary>
        /// 月份的第一天;
        /// </summary>
        const byte FIRSET_DAY_IN_MONTH = 1;

        /// <summary>
        /// 增加一天;
        /// </summary>
        public SimplifiedDateTime AddDay()
        {
            Day++;
            byte daysInMonth = GetDaysInMonth();

            if (Day > daysInMonth)
            {
                AddMonth();
                Day = FIRSET_DAY_IN_MONTH;
            }

            return this;
        }

        /// <summary>
        /// 增加天数;
        /// </summary>
        public SimplifiedDateTime AddDay(int days)
        {
            if (days < 0)
                throw new ArgumentOutOfRangeException();

            days += Day;

            for (byte daysInMonth = GetDaysInMonth();
                days > daysInMonth;
                daysInMonth = GetDaysInMonth())
            {
                days -= daysInMonth;
                AddMonth();
            }

            Day = Convert.ToByte(days);
            return this;
        }

        byte GetDaysInMonth()
        {
            return Calendar.GetDaysInMonth(Year, Month);
        }


        /// <summary>
        /// 一年的第一个月;
        /// </summary>
        const byte FIRSET_MONTH_IN_YEAR = 1;

        /// <summary>
        /// 增加一个月;
        /// </summary>
        public SimplifiedDateTime AddMonth()
        {
            Month++;
            byte monthInYear = GetMonthsInYear();

            if (Month > monthInYear)
            {
                AddYear();
                Month = FIRSET_MONTH_IN_YEAR;
            }

            return this;
        }

        /// <summary>
        /// 增加月数;
        /// </summary>
        public SimplifiedDateTime AddMonth(int months)
        {
            if (months < 0)
                throw new ArgumentOutOfRangeException();

            months += Month;

            for (byte monthInYear = GetMonthsInYear();
                months > monthInYear;
                monthInYear = GetMonthsInYear())
            {
                months -= monthInYear;
                AddYear();
            }

            Month = Convert.ToByte(months);
            return this;
        }

        byte GetMonthsInYear()
        {
            return Calendar.GetMonthsInYear(Year);
        }


        /// <summary>
        /// 增加一年;
        /// </summary>
        public SimplifiedDateTime AddYear()
        {
            if (IsMaxYear())
                return this;

            Year++;
            return this;
        }

        /// <summary>
        /// 增加年数;
        /// </summary>
        public SimplifiedDateTime AddYear(short years)
        {
            if (IsMaxYear())
                return this;

            Year += years;
            return this;
        }

        /// <summary>
        /// 是否年数记录已经到头了;
        /// </summary>
        bool IsMaxYear()
        {
            return this.Ticks > 0x7FFF0000;
        }


        public int CompareTo(SimplifiedDateTime other)
        {
            return this.Ticks - other.Ticks;
        }

        public bool Equals(SimplifiedDateTime other)
        {
            return this.Ticks == other.Ticks;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SimplifiedDateTime))
                return false;

            return Equals((SimplifiedDateTime)obj);
        }

        public override string ToString()
        {
            return "[Year:" + Year + ",Month:" + Month + ",Day:" + Day + ",Ticks:" + Ticks + "]";
        }

        public override int GetHashCode()
        {
            return this.Ticks;
        }


        public static bool operator ==(SimplifiedDateTime v1, SimplifiedDateTime v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(SimplifiedDateTime v1, SimplifiedDateTime v2)
        {
            return !v1.Equals(v2);
        }

        public static bool operator >(SimplifiedDateTime v1, SimplifiedDateTime v2)
        {
            return v1.Ticks > v2.Ticks;
        }

        public static bool operator <(SimplifiedDateTime v1, SimplifiedDateTime v2)
        {
            return v1.Ticks < v2.Ticks;
        }

    }

}
