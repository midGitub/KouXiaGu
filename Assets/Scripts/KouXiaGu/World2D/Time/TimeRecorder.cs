using System.Collections;
using UnityEngine;
using UniRx;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 游戏时间;
    /// 记录游戏时间;
    /// </summary>
    public class TimeRecorder : UnitySingleton<TimeRecorder>, IStartGameEvent, IArchiveEvent, IQuitGameEvent
    {
        TimeRecorder() { }

        /// <summary>
        /// 是否更新时间?
        /// </summary>
        [SerializeField]
        bool activate = true;

        /// <summary>
        /// 时间更新比率;
        /// </summary>
        [SerializeField]
        float timeRate = 1;

        /// <summary>
        /// 游戏经过的单位时间;
        /// </summary>
        [SerializeField]
        uLongReactiveProperty time = new uLongReactiveProperty(1);

        /// <summary>
        /// 时间更新协程返回参数;
        /// </summary>
        WaitForSeconds timeUpdateYieldInstruction;

        /// <summary>
        /// 时间更新协程;
        /// </summary>
        Coroutine timeUpdateCoroutine;

        /// <summary>
        /// 是否更新时间?
        /// </summary>
        public bool Activate
        {
            get { return activate; }
            set { activate = value; }
        }

        /// <summary>
        /// 游戏经过的单位时间;
        /// </summary>
        public ulong Time
        {
            get { return time.Value; }
        }

        /// <summary>
        /// 监视时间变化;
        /// </summary>
        public IReactiveProperty<ulong> ObservableTime
        {
            get { return time; }
        }

        /// <summary>
        /// 游戏时间缩放;
        /// </summary>
        public float TimeScale
        {
            get { return timeRate; }
            set { SetTimeScale(value); }
        }

        void Awake()
        {
            timeUpdateYieldInstruction = new WaitForSeconds(timeRate);
        }

        /// <summary>
        /// 游戏时间更新协程;
        /// </summary>
        IEnumerator TimeUpdateCoroutine()
        {
            while (true)
            {
                if (activate)
                    time.Value++;

                yield return timeUpdateYieldInstruction;
            }
        }

        /// <summary>
        /// 设置时间缩放;
        /// </summary>
        void SetTimeScale(float timeScale)
        {
            timeUpdateYieldInstruction = new WaitForSeconds(timeScale);
            RestartTimeUpdateCoroutine();
        }

        /// <summary>
        /// 重新开始时间更新协程;
        /// </summary>
        void RestartTimeUpdateCoroutine()
        {
            if (timeUpdateCoroutine != null)
            {
                StopCoroutine(timeUpdateCoroutine);
            }
            timeUpdateCoroutine = StartCoroutine(TimeUpdateCoroutine());
        }


        IEnumerator IConstruct2<BuildGameData>.Prepare(BuildGameData item)
        {
            ArchivedTime archivedTime = item.ArchivedData.Archived.Time;
            time.Value = archivedTime.Time;
            yield break;
        }

        IEnumerator IConstruct2<BuildGameData>.Construction(BuildGameData item)
        {
            RestartTimeUpdateCoroutine();
            yield break;
        }

        IEnumerator IConstruct1<ArchivedGroup>.Construction(ArchivedGroup item)
        {
            ArchivedTime archivedTime = item.Archived.Time;
            archivedTime.Time = time.Value;
            yield break;
        }

        IEnumerator IConstruct1<QuitGameData>.Construction(QuitGameData item)
        {
            StopCoroutine(timeUpdateCoroutine);
            yield break;
        }

    }

}
