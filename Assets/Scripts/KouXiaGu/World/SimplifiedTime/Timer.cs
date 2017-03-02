using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Rx;

namespace JiongXiaGu.SimplifiedTime
{

    /// <summary>
    /// 时间记录;
    /// </summary>
    public class Timer : IObservable<SimplifiedDateTime>
    {

        const byte FIRSET_MINUTE_IN_HOUR = 0;
        const byte FIRSET_HOUR_IN_DAY = 0;

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="time">起始时间;</param>
        /// <param name="minutesInHour">一小时有多少分钟;</param>
        public Timer(SimplifiedDateTime time, byte minutesInHour)
        {
            this.currentTime = time;
            this.minutesInHour = minutesInHour;
            this.CurrentMinute = FIRSET_MINUTE_IN_HOUR;
            trigger = new Trigger<SimplifiedDateTime>();
        }

        public Timer(SimplifiedDateTime time, byte hoursInDay, byte minutesInHour) : this(time, minutesInHour)
        {
            this.hoursInDay = hoursInDay;
            CurrentHour = FIRSET_HOUR_IN_DAY;
        }


        /// <summary>
        /// 时间信息;
        /// </summary>
        SimplifiedDateTime currentTime;

        /// <summary>
        /// 当前分钟; 数值: 1 - secondsInHour
        /// </summary>
        public byte CurrentMinute { get; set; }

        /// <summary>
        /// 当前的小时数;数值: 0 - 23;
        /// </summary>
        public byte CurrentHour { get; set; }

        /// <summary>
        /// 时间信息;
        /// </summary>
        public SimplifiedDateTime CurrentSimplifiedDateTime
        {
            get { return currentTime; }
            private set { currentTime = value; }
        }

        /// <summary>
        /// 完整的时间信息,秒数为0;
        /// </summary>
        public DateTime CurrentDateTime
        {
            get { return new DateTime(currentTime, CurrentHour, CurrentMinute, 0); }
        }


        byte hoursInDay = 24;

        public byte HoursInDay
        {
            get { return hoursInDay; }
            set { hoursInDay = value; }
        }

        ushort minutesInHour = 20;

        public ushort MinutesInHour
        {
            get { return minutesInHour; }
            set { minutesInHour = value; }
        }

        internal void FixedUpdate()
        {
            CurrentMinute++;
            if (CurrentMinute >= minutesInHour)
            {
                CurrentMinute = FIRSET_MINUTE_IN_HOUR;

                CurrentHour++;
                if (CurrentHour >= hoursInDay)
                {
                    CurrentHour = FIRSET_HOUR_IN_DAY;

                    currentTime.AddDay();
                    Triggering();
                }
            }
        }


        /// <summary>
        /// 时间触发器;
        /// </summary>
        Trigger<SimplifiedDateTime> trigger;

        /// <summary>
        /// 当时间 天数 发生变化时推送;
        /// </summary>
        public IDisposable Subscribe(IObserver<SimplifiedDateTime> observer)
        {
            return this.trigger.Subscribe(observer);
        }

        /// <summary>
        /// 立即推送当前时间到观察者;
        /// </summary>
        public void Triggering()
        {
            trigger.Triggering(currentTime);
        }

        /// <summary>
        /// 设置为新的时间;
        /// </summary>
        public void SetCurrentDateTime(DateTime currentDateTime)
        {
            currentTime.Ticks = currentDateTime.SimplifiedDateTimeTicks;
            CurrentHour = currentDateTime.Hour;
            CurrentMinute = currentDateTime.Minute;

            Triggering();
        }

    }

}
