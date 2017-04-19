using System;
using KouXiaGu.Rx;
using UnityEngine;
using XLua;

namespace KouXiaGu.World
{

    /// <summary>
    /// 世界时间初始化信息;
    /// </summary>
    [Serializable]
    public class WorldTimeInfo
    {
        /// <summary>
        /// 开始时间;
        /// </summary>
        public DateTime StartTime;

        /// <summary>
        /// 当前时间;
        /// </summary>
        public DateTime CurrentTime;

        /// <summary>
        /// 一个小时间隔多少次 FixedUpdate() ?
        /// </summary>
        [Range(0, 60)]
        public int HourInterval;
    }


    public class TimeManager : IXiaGuObservable<DateTime>
    {

        public TimeManager(WorldTimeInfo info)
        {
            Info = info;
            timeTracker = new ListTracker<DateTime>();
            InitCalendar();
        }

        public WorldTimeInfo Info { get; private set; }
        TrackerBase<DateTime> timeTracker;
        TimeUpdater updater;

        public DateTime CurrentTime
        {
            get { return Info.CurrentTime; }
            private set { Info.CurrentTime = value; }
        }

        public bool IsTimeUpdating
        {
            get { return updater != null && updater.IsRunning; }
        }

        /// <summary>
        /// 时间间隔长度;
        /// </summary>
        public int HourInterval
        {
            get { return Info.HourInterval; }
            set { Info.HourInterval = value; }
        }

        public IDisposable Subscribe(IXiaGuObserver<DateTime> observer)
        {
            return ((IXiaGuObservable<DateTime>)this.timeTracker).Subscribe(observer);
        }

        /// <summary>
        /// 立即推送当前时间到观察者;
        /// </summary>
        void TrackTime()
        {
            timeTracker.Track(CurrentTime);
        }

        void InitCalendar()
        {
            try
            {
                ICalendar Calendar = CalendarFormLuaScript.Read();
                DateTime.CurrentCalendar = Calendar;
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public void StartTimeUpdating()
        {
            updater = TimeUpdater.Create(this, true);
        }

        public void StopTimeUpdating()
        {
            if (updater != null)
            {
                updater.IsRunning = false;
            }
        }

        void TimeUpdaterDestroy()
        {
            updater = null;
        }

        /// <summary>
        /// 时间更新器;
        /// </summary>
        [DisallowMultipleComponent]
        class TimeUpdater : MonoBehaviour
        {
            static TimeUpdater instance;

            static bool isCreated
            {
                get { return instance != null; }
            }

            internal static TimeUpdater Create(TimeManager manager, bool isRunning)
            {
                if (isCreated)
                    return instance;

                var gameObject = new GameObject("TimeUpdater", typeof(TimeUpdater));
                instance = gameObject.GetComponent<TimeUpdater>();
                instance.manager = manager;
                instance.IsRunning = isRunning;
                return instance;
            }

            TimeUpdater()
            {
            }

            TimeManager manager;
            int currenMinute;

            public bool IsRunning
            {
                get { return transform != null && enabled; }
                set { enabled = value; }
            }

            public DateTime CurrentTime
            {
                get { return manager.CurrentTime; }
                private set { manager.CurrentTime = value; }
            }

            public int HourInterval
            {
                get { return manager.HourInterval; }
            }

            void FixedUpdate()
            {
                currenMinute++;
                if (currenMinute > HourInterval)
                {
                    currenMinute = 0;
                    CurrentTime = CurrentTime.AddHour();
                    manager.TrackTime();
                }
            }

            void OnDestroy()
            {
                instance = null;
                manager.TimeUpdaterDestroy();
            }

        }

    }

}
