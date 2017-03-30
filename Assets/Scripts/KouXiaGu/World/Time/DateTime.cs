using System;
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

        static DateTime()
        {
            CurrentCalendar = new ChineseCalendar();
        }

        /// <summary>
        /// 一年一月一日零时零分零秒;
        /// </summary>
        internal const long DefaultTicks = 0x0;

        /// <summary>
        /// 当前使用的日历;
        /// </summary>
        public static ICalendar CurrentCalendar { get; private set; }

        /// <summary>
        /// 一年一月一日,默认的日历;
        /// </summary>
        public static DateTime Default
        {
            get { return new DateTime(DefaultTicks); }
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

        
        const byte FirstSecondInMinute = 0;
        const byte SecondsInMinute = 60;

        /// <summary>
        /// 增加一秒钟;
        /// </summary>
        public DateTime AddSecond()
        {
            Second++;

            if (Second >= SecondsInMinute)
            {
                AddMinute();
                ResetSecond();
            }

            return this;
        }

        void ResetSecond()
        {
            Second = FirstSecondInMinute;
        }


        const byte FirstMinuteInHour = 0;
        const byte MinutesInHour = 60;

        /// <summary>
        /// 增加一分钟;
        /// </summary>
        public DateTime AddMinute()
        {
            Minute++;

            if (Minute >= MinutesInHour)
            {
                AddHour();
                ResetMinute();
            }

            return this;
        }

        void ResetMinute()
        {
            Minute = FirstMinuteInHour;
        }

        public DateTime AddMinutes(int minutes)
        {
            if (minutes < 0)
                throw new ArgumentOutOfRangeException();

            minutes += Minute;
            int hours = Math.DivRem(minutes, MinutesInHour, out minutes);
            AddHours(hours);
            Minute = Convert.ToByte(minutes);

            return this;
        }

        const byte FirstHourInDay = 0;
        const byte HoursInDay = 24;

        /// <summary>
        /// 增加一小时;
        /// </summary>
        public DateTime AddHour()
        {
            Hour++;

            if (Hour >= HoursInDay)
            {
                AddDay();
                ResetHour();
            }

            return this;
        }

        void ResetHour()
        {
            Hour = FirstHourInDay;
        }

        public DateTime AddHours(int hours)
        {
            if (hours < 0)
                throw new ArgumentOutOfRangeException();

            hours += Hour;
            int day = Math.DivRem(hours, HoursInDay, out hours);
            AddDays(day);
            Hour = Convert.ToByte(hours);

            return this;
        }


        /// <summary>
        /// 月份的第一天;
        /// </summary>
        const byte FirstDayInMonth = 0;

        /// <summary>
        /// 增加一天;
        /// </summary>
        public DateTime AddDay()
        {
            Day++;
            byte daysInMonth = GetDaysInMonth();

            if (Day >= daysInMonth)
            {
                AddMonth();
                ResetDay();
            }

            return this;
        }

        void ResetDay()
        {
            Day = FirstDayInMonth;
        }

        /// <summary>
        /// 增加天数;
        /// </summary>
        public DateTime AddDays(int days)
        {
            if (days < 0)
                throw new ArgumentOutOfRangeException();

            days += Day;

            for (byte daysInMonth = GetDaysInMonth();
                days >= daysInMonth;
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
            return CurrentCalendar.GetDaysInMonth(Year, Month);
        }


        /// <summary>
        /// 一年的第一个月;
        /// </summary>
        const byte FirstMonthInYear = 0;

        /// <summary>
        /// 增加一个月;
        /// </summary>
        public DateTime AddMonth()
        {
            Month++;
            byte monthInYear = GetMonthsInYear();

            if (Month >= monthInYear)
            {
                AddYear();
                ResetMonth();
            }

            return this;
        }

        void ResetMonth()
        {
            Month = FirstMonthInYear;
        }

        /// <summary>
        /// 增加月数;
        /// </summary>
        public DateTime AddMonths(int months)
        {
            if (months < 0)
                throw new ArgumentOutOfRangeException();

            months += Month;

            for (byte monthInYear = GetMonthsInYear();
                months >= monthInYear;
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
            return CurrentCalendar.GetMonthsInYear(Year);
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
        public DateTime AddYears(short years)
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
            bool isMaxYear = this.Ticks >= 0x7FFF000000000000;
            return isMaxYear;
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
            return unchecked((int)Ticks) ^ (int)(Ticks >> 32);
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

    }

}
