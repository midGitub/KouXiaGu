using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.World.TimeSystem
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

        IDisposable updateCanceler;
        public WorldDateTime StartTime { get; internal set; }
        public WorldDateTime CurrentTime { get; internal set; }
        public TimeUpdater Updater { get; private set; }
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
