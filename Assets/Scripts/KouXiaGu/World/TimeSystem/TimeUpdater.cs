using System;
using UnityEngine;

namespace KouXiaGu.World.TimeSystem
{

    /// <summary>
    /// 负责游戏时间更新;
    /// </summary>
    [DisallowMultipleComponent]
    public class TimeUpdater : SceneSington<TimeUpdater>
    {
        public TimeUpdater()
        {
        }

        /// <summary>
        /// 分钟长度;多少次 FixedUpdate 为一分钟;
        /// </summary>
        [SerializeField, Range(1, 200)]
        int MinuteLength = 10;

        /// <summary>
        /// 时间缩放比例,默认为1;
        /// </summary>
        [SerializeField]
        int TimeScale = 1;

        [SerializeField]
        bool isTimeUpdating;
        int currenMinute;
        IObserverCollection<ITimerUnit> observers;

        public bool IsTimeUpdating
        {
            get { return isTimeUpdating; }
            set { isTimeUpdating = value; }
        }

        void FixedUpdate()
        {
            currenMinute++;
            if (currenMinute >= MinuteLength)
            {
                currenMinute = 0;
                if (isTimeUpdating && observers != null)
                {
                    foreach (var observer in observers.EnumerateObserver())
                    {
                        observer.AddUnit(TimeScale);
                    }
                }
            }
        }

        public IDisposable Subscribe(ITimerUnit observer)
        {
            if (observers == null)
            {
                observers = new ObserverLinkedList<ITimerUnit>();
            }
            return observers.Subscribe(observer);
        }
    }
}
