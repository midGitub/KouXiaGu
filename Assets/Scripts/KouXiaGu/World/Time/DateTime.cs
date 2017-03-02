using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using ProtoBuf;

namespace KouXiaGu.World
{

    /// <summary>
    /// 简化版的 System.DateTime,但是实现原理不同;
    /// </summary>
    [Serializable, XmlType("DateTime"), ProtoContract]
    public struct DateTime : IEquatable<DateTime>, IComparable<DateTime>
    {

        /// <summary>
        /// 一年一月一日零时零分零秒;
        /// </summary>
        const long DEFAULT_TICKS = 0x1010100000000;

        /// <summary>
        /// 一年一月一日,默认的日历;
        /// </summary>
        public static DateTime Default
        {
            get { return new DateTime(DEFAULT_TICKS); }
        }


        public DateTime(SimplifiedDateTime time)
        {
            this.ticks = 0;
            this.ticks |= ((long)time.Ticks << 32);
        }

        public DateTime(SimplifiedDateTime time, byte hour, byte minute, byte second) : this(time)
        {
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
        }

        public DateTime(long ticks)
        {
            this.ticks = ticks;
        }

        public DateTime(short year, byte month, byte day, byte hour, byte minute, byte second)
        {
            this.ticks = 0;

            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
        }


        [SerializeField, ProtoMember(1)]
        long ticks;

        /// <summary>
        /// 周期数;
        /// 年占用最前的两个字节,其后到月占用一字节,日占用一字节,
        /// 时占用其后一字节,分占用一个字节,秒占用一字节,空余一个字节;
        /// </summary>
        [XmlAttribute("ticks")]
        public long Ticks
        {
            get { return ticks; }
            private set { ticks = value; }
        }


        /// <summary>
        /// int类型的周期数,仅有 年月日;
        /// </summary>
        public int SimplifiedDateTimeTicks
        {
            get { return (int)(Ticks >> 32); }
        }

        public short Year
        {
            get { return (short)(Ticks >> 48); }
            set { Ticks = (Ticks & 0xFFFFFFFFFFFF) | ((long)value << 48); }
        }

        public byte Month
        {
            get { return (byte)((Ticks & 0xFF0000000000) >> 40); }
            set { Ticks = (Ticks & -0xFF0000000001) | ((long)value << 40); }
        }

        public byte Day
        {
            get { return (byte)((Ticks & 0xFF00000000) >> 32); }
            set { Ticks = (Ticks & -0xFF00000001) | ((long)value << 32); }
        }

        public byte Hour
        {
            get { return (byte)((Ticks & 0xFF000000) >> 24); }
            set { Ticks = (Ticks & -0xFF000001) | ((long)value << 24); }
        }

        public byte Minute
        {
            get { return (byte)((Ticks & 0xFF0000) >> 16); }
            set { Ticks = (Ticks & -0xFF0001) | ((long)value << 16); }
        }

        public byte Second
        {
            get { return (byte)((Ticks & 0xFF00) >> 8); }
            set { Ticks = (Ticks & -0xFF01) | ((long)value << 8); }
        }


        const byte FIRSET_SECOND_IN_MINUTE = 0;
        const byte SECONDS_IN_MINUTE = 60;

        /// <summary>
        /// 增加一秒钟;
        /// </summary>
        public DateTime AddSecond()
        {
            Second++;

            if (Second >= SECONDS_IN_MINUTE)
            {
                AddMinute();
                Second = FIRSET_SECOND_IN_MINUTE;
            }

            return this;
        }


        const byte FIRSET_MINUTE_IN_HOUR = 0;
        const byte MINUTES_IN_HOUR = 60;

        /// <summary>
        /// 增加一分钟;
        /// </summary>
        public DateTime AddMinute()
        {
            Minute++;

            if (Minute >= MINUTES_IN_HOUR)
            {
                AddHour();
                Minute = FIRSET_MINUTE_IN_HOUR;
            }

            return this;
        }


        const byte FIRSET_HOUR_IN_DAY = 0;
        const byte HOURS_IN_DAY = 24;

        /// <summary>
        /// 增加一小时;
        /// </summary>
        public DateTime AddHour()
        {
            Hour++;

            if (Hour >= HOURS_IN_DAY)
            {
                AddDay();
                Hour = FIRSET_HOUR_IN_DAY;
            }

            return this;
        }


        /// <summary>
        /// 月份的第一天;
        /// </summary>
        const byte FIRSET_DAY_IN_MONTH = 1;

        /// <summary>
        /// 增加一天;
        /// </summary>
        public DateTime AddDay()
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
        public DateTime AddDay(int days)
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
            return ChineseCalendar.GetDaysInMonth(Year, Month);
        }


        /// <summary>
        /// 一年的第一个月;
        /// </summary>
        const byte FIRSET_MONTH_IN_YEAR = 1;

        /// <summary>
        /// 增加一个月;
        /// </summary>
        public DateTime AddMonth()
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
        public DateTime AddMonth(int months)
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
            return ChineseCalendar.GetMonthsInYear(Year);
        }


        /// <summary>
        /// 增加一年;
        /// </summary>
        public DateTime AddYear()
        {
            if (IsMaxYear())
                return this;

            Year++;
            return this;
        }

        /// <summary>
        /// 增加年数;
        /// </summary>
        public DateTime AddYear(short years)
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


        public int CompareTo(DateTime other)
        {
            if (this.Ticks == other.Ticks)
                return 0;

            long i = this.Ticks - other.Ticks;
            return i > 0 ? 1 : -1;
        }

        public bool Equals(DateTime other)
        {
            return other.Ticks == this.Ticks;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DateTime))
                return false;

            return Equals((DateTime)obj);
        }

        public override int GetHashCode()
        {
            return Ticks.GetHashCode();
        }

        public override string ToString()
        {
            return 
                "[Year:" + Year + ",Month:" + Month + ",Day:" + Day + ",Hour:" + Hour +
                ",Minute:" + Minute + ",Second:" + Second + ",Ticks:" + Ticks + "]";
        }


        public static bool operator ==(DateTime v1, DateTime v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(DateTime v1, DateTime v2)
        {
            return !v1.Equals(v2);
        }

        public static bool operator >(DateTime v1, DateTime v2)
        {
            return v1.Ticks > v2.Ticks;
        }

        public static bool operator <(DateTime v1, DateTime v2)
        {
            return v1.Ticks < v2.Ticks;
        }


        public static bool operator ==(DateTime v1, SimplifiedDateTime v2)
        {
            return v1.SimplifiedDateTimeTicks == v2.Ticks;
        }

        public static bool operator !=(DateTime v1, SimplifiedDateTime v2)
        {
            return v1.SimplifiedDateTimeTicks != v2.Ticks;
        }

        public static bool operator >(DateTime v1, SimplifiedDateTime v2)
        {
            return v1.SimplifiedDateTimeTicks > v2.Ticks;
        }

        public static bool operator <(DateTime v1, SimplifiedDateTime v2)
        {
            return v1.SimplifiedDateTimeTicks < v2.Ticks;
        }

    }

}
