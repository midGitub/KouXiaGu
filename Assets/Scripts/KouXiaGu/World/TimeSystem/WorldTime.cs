using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.TimeSystem
{

    /// <summary>
    /// 游戏世界时间;
    /// </summary>
    public class WorldTime : ITimerUnit
    {
        public WorldTime()
        {
            Updater = TimeUpdater.Instance;
            updateCanceler = Updater.Subscribe(this);
        }

        /// <summary>
        /// 游戏开始时间;
        /// </summary>
        public WorldDateTime StartTime { get; internal set; }

        /// <summary>
        /// 当前游戏时间;
        /// </summary>
        public WorldDateTime CurrentTime { get; internal set; }

        /// <summary>
        /// 当前的日历信息;
        /// </summary>
        public ICalendar Calendar { get; private set; }

        /// <summary>
        /// 更新器;
        /// </summary>
        public TimeUpdater Updater { get; private set; }

        IDisposable updateCanceler;
        public bool IsPause { get; private set; }

        void ITimerUnit.AddUnit(int time)
        {
            if (!IsPause)
            {
                CurrentTime = CurrentTime.AddMinutes(time);
            }
        }

        void Start()
        {
            updateCanceler = TimeUpdater.Instance.Subscribe(this);
        }

        void Pause()
        {
            IsPause = true;
        }

        void Stop()
        {
            updateCanceler.Dispose();
            updateCanceler = null;
        }

        public static implicit operator WorldDateTime(WorldTime message)
        {
            return message.CurrentTime;
        }
    }
}
