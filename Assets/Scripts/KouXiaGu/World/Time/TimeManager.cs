using System;

using UniRx;
using UnityEngine;

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


    public class TimeManager : IObservable<DateTime>, IObserver<IWorldScene>
    {

        public static IAsyncOperation<TimeManager> Create(WorldTimeInfo info, IObservable<IWorldScene> world)
        {
            return new Operation<TimeManager>(() => new TimeManager(info, world));
        }


        public TimeManager(WorldTimeInfo info, IObservable<IWorldScene> world)
        {
            InitCalendar();
            Info = info;
            timeTracker = new ListTracker<DateTime>();
            timeUpdater = new TimeUpdater(this);
            world.Subscribe(this);
        }

        public WorldTimeInfo Info { get; private set; }
        TrackerBase<DateTime> timeTracker;
        TimeUpdater timeUpdater;

        public DateTime CurrentTime
        {
            get { return Info.CurrentTime; }
            private set { Info.CurrentTime = value; }
        }

        public bool IsTimeUpdating
        {
            get { return timeUpdater.IsSubscribed; }
        }

        /// <summary>
        /// 时间间隔长度;
        /// </summary>
        public int HourInterval
        {
            get { return Info.HourInterval; }
            set { Info.HourInterval = value; }
        }

        public IDisposable Subscribe(IObserver<DateTime> observer)
        {
            return ((IObservable<DateTime>)this.timeTracker).Subscribe(observer);
        }

        public void StartTimeUpdating()
        {
            if(!timeUpdater.IsSubscribed)
                timeUpdater.SubscribeFixedUpdate();
        }

        public void StopTimeUpdating()
        {
            timeUpdater.Dispose();
        }

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

        public void OnNext(IWorldScene item)
        {
            StartTimeUpdating();
        }

        public void OnError(Exception error) { }
        public void OnCompleted() { }

        /// <summary>
        /// 时间更新器;
        /// </summary>
        [DisallowMultipleComponent]
        class TimeUpdater : UnityThreadEvent
        {
            public TimeUpdater(TimeManager manager)
            {
                this.manager = manager;
            }

            TimeManager manager;
            int currenMinute;

            public DateTime CurrentTime
            {
                get { return manager.CurrentTime; }
                private set { manager.CurrentTime = value; }
            }

            public int HourInterval
            {
                get { return manager.HourInterval; }
            }

            public override void OnNext()
            {
                currenMinute++;
                if (currenMinute > HourInterval)
                {
                    currenMinute = 0;
                    CurrentTime = CurrentTime.AddHour();
                    manager.TrackTime();
                }
            }

        }

    }

}
