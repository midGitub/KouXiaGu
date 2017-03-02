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
        public Timer(DateTime time)
        {
            this.currentTime = time;
            trigger = new Trigger<SimplifiedDateTime>();
        }


        /// <summary>
        /// 时间信息;
        /// </summary>
        DateTime currentTime;

        /// <summary>
        /// 时间信息;
        /// </summary>
        public SimplifiedDateTime CurrentSimplifiedDateTime
        {
            get { return new SimplifiedDateTime(currentTime); }
        }

        /// <summary>
        /// 完整的时间信息,秒数为0;
        /// </summary>
        public DateTime CurrentDateTime
        {
            get { return currentTime; }
            private set { currentTime = value; }
        }

        internal void FixedUpdate()
        {
            currentTime.AddHour();
            Triggering();
        }


        /// <summary>
        /// 时间触发器;
        /// </summary>
        Trigger<SimplifiedDateTime> trigger;

        /// <summary>
        /// 当时间发生变化时推送;
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
            trigger.Triggering(CurrentSimplifiedDateTime);
        }

        /// <summary>
        /// 设置为新的时间;
        /// </summary>
        public void SetCurrentDateTime(DateTime currentDateTime)
        {
            currentTime = currentDateTime;
            Triggering();
        }

    }

}
