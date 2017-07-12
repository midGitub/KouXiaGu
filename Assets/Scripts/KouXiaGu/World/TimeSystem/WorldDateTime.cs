using System;
using System.Xml.Serialization;
using UnityEngine;
using ProtoBuf;

namespace KouXiaGu.World.TimeSystem
{

    /// <summary>
    /// 简化版的 System.DateTime,但是实现原理不同;
    /// </summary>
    [Serializable, XmlType("DateTime"), ProtoContract]
    public struct WorldDateTime : IEquatable<WorldDateTime>, IComparable<WorldDateTime>
    {
        static WorldDateTime()
        {
            CurrentCalendar = new ChineseCalendar();
        }

        /// <summary>
        /// 一年一月一日,默认的日历;
        /// </summary>
        public static WorldDateTime Default
        {
            get { return new WorldDateTime(0x0); }
        }

        /// <summary>
        /// 当前使用的日历;
        /// </summary>
        public static ICalendar CurrentCalendar { get; private set; }

        /// <summary>
        /// 提供设置自定义的日历;
        /// </summary>
        internal static void SetCalendar(ICalendar calendar)
        {
            if (calendar == null)
                throw new ArgumentNullException("calendar");

            CurrentCalendar = calendar;
        }


        public WorldDateTime(long ticks)
        {
            this.ticks = ticks;
        }

        public WorldDateTime(short year, byte month, byte day, byte hour, byte minute, byte second)
        {
            ticks = 0;
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
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
        /// 年份: -32,768 到 32,767
        /// </summary>
        public short Year
        {
            get { return (short)(Ticks >> 48); }
            set { Ticks = (Ticks & 0xFFFFFFFFFFFF) | ((long)value << 48); }
        }

        /// <summary>
        /// 月份;
        /// </summary>
        public byte Month
        {
            get { return (byte)((Ticks & 0xFF0000000000) >> 40); }
            set { Ticks = (Ticks & -0xFF0000000001) | ((long)value << 40); }
        }

        /// <summary>
        /// 天:
        /// </summary>
        public byte Day
        {
            get { return (byte)((Ticks & 0xFF00000000) >> 32); }
            set { Ticks = (Ticks & -0xFF00000001) | ((long)value << 32); }
        }

        /// <summary>
        /// 小时; 0 到 23
        /// </summary>
        public byte Hour
        {
            get { return (byte)((Ticks & 0xFF000000) >> 24); }
            set { Ticks = (Ticks & -0xFF000001) | ((long)value << 24); }
        }

        /// <summary>
        /// 分钟; 0 到 59
        /// </summary>
        public byte Minute
        {
            get { return (byte)((Ticks & 0xFF0000) >> 16); }
            set { Ticks = (Ticks & -0xFF0001) | ((long)value << 16); }
        }

        /// <summary>
        /// 秒钟; 0 到 59
        /// </summary>
        public byte Second
        {
            get { return (byte)((Ticks & 0xFF00) >> 8); }
            set { Ticks = (Ticks & -0xFF01) | ((long)value << 8); }
        }

        public ICalendar Calendar
        {
            get { return CurrentCalendar; }
        }

        internal const byte SecondsInMinute = 60;
        internal const byte MinutesInHour = 60;
        internal const byte HoursInDay = 24;

        public WorldDateTime AddSecond()
        {
            Second++;
            if (Second >= SecondsInMinute)
            {
                AddMinute();
                Second = 0;
            }
            return this;
        }

        public WorldDateTime AddSeconds(int seconds)
        {
            if (seconds < 0)
                throw new ArgumentOutOfRangeException();

            seconds += Second;
            int minutes = Math.DivRem(seconds, SecondsInMinute, out seconds);
            AddMinutes(minutes);
            Second = Convert.ToByte(seconds);
            return this;
        }

        public WorldDateTime AddMinute()
        {
            Minute++;
            if (Minute >= MinutesInHour)
            {
                AddHour();
                Minute = 0;
            }
            return this;
        }

        public WorldDateTime AddMinutes(int minutes)
        {
            if (minutes < 0)
                throw new ArgumentOutOfRangeException();

            minutes += Minute;
            int hours = Math.DivRem(minutes, MinutesInHour, out minutes);
            AddHours(hours);
            Minute = Convert.ToByte(minutes);
            return this;
        }

        public WorldDateTime AddHour()
        {
            Hour++;
            if (Hour >= HoursInDay)
            {
                AddDay();
                Hour = 0;
            }
            return this;
        }

        public WorldDateTime AddHours(int hours)
        {
            if (hours < 0)
                throw new ArgumentOutOfRangeException();

            hours += Hour;
            int day = Math.DivRem(hours, HoursInDay, out hours);
            AddDays(day);
            Hour = Convert.ToByte(hours);
            return this;
        }

        public WorldDateTime AddDay()
        {
            Day++;
            int daysInMonth = GetDaysInMonth();
            if (Day >= daysInMonth)
            {
                AddMonth();
                Day = 0;
            }
            return this;
        }

        public WorldDateTime AddDays(int days)
        {
            if (days < 0)
                throw new ArgumentOutOfRangeException();

            days += Day;
            for (int daysInMonth = GetDaysInMonth(); days >= daysInMonth; daysInMonth = GetDaysInMonth())
            {
                days -= daysInMonth;
                AddMonth();
            }
            Day = Convert.ToByte(days);
            return this;
        }

        int GetDaysInMonth()
        {
            return CurrentCalendar.GetDaysInMonth(Year, Month);
        }

        public WorldDateTime AddMonth()
        {
            Month++;
            int monthInYear = GetMonthsInYear();
            if (Month >= monthInYear)
            {
                AddYear();
                Month = 0;
            }
            return this;
        }

        public WorldDateTime AddMonths(int months)
        {
            if (months < 0)
                throw new ArgumentOutOfRangeException();

            months += Month;
            for (int monthInYear = GetMonthsInYear(); months >= monthInYear; monthInYear = GetMonthsInYear())
            {
                months -= monthInYear;
                AddYear();
            }
            Month = Convert.ToByte(months);
            return this;
        }

        int GetMonthsInYear()
        {
            return CurrentCalendar.GetMonthsInYear(Year);
        }

        public WorldDateTime AddYear()
        {
            if (IsMaxYear())
                return this;

            Year++;
            return this;
        }

        public WorldDateTime AddYears(short years)
        {
            if (IsMaxYear())
                return this;

            Year += years;
            return this;
        }

        /// <summary>
        /// 是否年数记录已经到头了;
        /// </summary>
        public bool IsMaxYear()
        {
            bool isMaxYear = Ticks >= 0x7FFF000000000000;
            return isMaxYear;
        }

        public bool IsLeapYear()
        {
            return Calendar.IsLeapYear(Year);
        }

        public bool IsLeapMonth()
        {
            return Calendar.IsLeapMonth(Year, Month);
        }

        public int GetChineseZodiac()
        {
            return Calendar.GetChineseZodiac(Year);
        }

        public int GetMonth(int year, int month, out bool isLeapMonth)
        {
            return Calendar.GetMonth(year, month, out isLeapMonth);
        }

        public int CompareTo(WorldDateTime other)
        {
            if (Ticks == other.Ticks)
                return 0;

            long i = Ticks - other.Ticks;
            return i > 0 ? 1 : -1;
        }

        public bool Equals(WorldDateTime other)
        {
            return other.Ticks == Ticks;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WorldDateTime))
                return false;

            return Equals((WorldDateTime)obj);
        }

        public override int GetHashCode()
        {
            return unchecked((int)Ticks) ^ (int)(Ticks >> 32);
        }

        public string ToTimeString()
        {
            return (Year + 1) + "/" + (Month + 1) + "/" + (Day + 1) + " " + (Hour) + ":" + (Minute) + ":" + (Second);
        }

        public override string ToString()
        {
            return
                "[Year:" + Year + ",Month:" + Month + ",Day:" + Day + ",Hour:" + Hour +
                ",Minute:" + Minute + ",Second:" + Second + ",Ticks:" + Ticks + "]";
        }


        public static bool operator ==(WorldDateTime v1, WorldDateTime v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(WorldDateTime v1, WorldDateTime v2)
        {
            return !v1.Equals(v2);
        }

        public static bool operator >(WorldDateTime v1, WorldDateTime v2)
        {
            return v1.Ticks > v2.Ticks;
        }

        public static bool operator <(WorldDateTime v1, WorldDateTime v2)
        {
            return v1.Ticks < v2.Ticks;
        }
    }
}
