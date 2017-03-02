using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.SimplifiedTime
{

    /// <summary>
    /// 仅记录年 月 日 时 分 秒;
    /// </summary>
    public struct DateTime : IEquatable<DateTime>, IComparable<DateTime>
    {


        const byte FIRSET_MINUTE_IN_HOUR = 0;
        const byte FIRSET_HOUR_IN_DAY = 0;

        /// <summary>
        /// 一年一月一日零时零分零秒;
        /// </summary>
        const long DEFAULT_TICKS = 0x1010100000000;


        public DateTime(SimplifiedDateTime time)
        {
            this.Ticks = 0;
            this.Ticks |= ((long)time.Ticks << 32);
        }

        public DateTime(SimplifiedDateTime time, byte hour, byte minute, byte second) : this(time)
        {
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
        }

        public DateTime(short year, byte month, byte day, byte hour, byte minute, byte second) : this()
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
        }


        /// <summary>
        /// 周期数;
        /// 年占用最前的两个字节,其后到月占用一字节,日占用一字节,
        /// 时占用其后一字节,分占用一个字节,秒占用一字节,空余一个字节;
        /// </summary>
        public long Ticks { get; private set; }

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
