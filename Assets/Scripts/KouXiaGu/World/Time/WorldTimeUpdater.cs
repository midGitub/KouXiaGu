using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    /// <summary>
    /// 时间更新器;
    /// </summary>
    public class WorldTimeUpdater : IUnityThreadBehaviour<Action>, IDisposable
    {
        public WorldTimeUpdater(WorldTime time)
        {
            if (time == null)
                throw new ArgumentNullException("time");

            timeManager = time;
            settings = TimeSettings.Instance;
        }

        WorldTime timeManager;
        TimeSettings settings;
        IDisposable updateCanceler;
        float currenMinute;

        public bool IsTimeUpdating
        {
            get { return updateCanceler != null; }
        }

        object IUnityThreadBehaviour<Action>.Sender
        {
            get { return "世界时间更新"; }
        }

        Action IUnityThreadBehaviour<Action>.Action
        {
            get { return Next; }
        }

        void Next()
        {
            currenMinute += settings.TimeScale;
            if (currenMinute >= settings.HourLength)
            {
                currenMinute -= settings.HourLength;
                DateTime time = timeManager.CurrentTime;
                time = time.AddHour();
                timeManager.SetCurrentTime(time);
            }
        }

        public void Start()
        {
            if (updateCanceler == null)
            {
                updateCanceler = UnityThreadDispatcher.Instance.SubscribeFixedUpdate(this);
            }
        }

        public void Stop()
        {
            if (updateCanceler != null)
            {
                updateCanceler.Dispose();
                updateCanceler = null;
            }
        }

        void IDisposable.Dispose()
        {
            Stop();
        }
    }
}
