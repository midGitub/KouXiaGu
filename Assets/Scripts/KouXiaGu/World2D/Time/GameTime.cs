﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 游戏时间;
    /// 记录游戏时间;
    /// </summary>
    public class GameTime : UnitySingleton<GameTime>, IStartGameEvent, IArchiveEvent, IQuitGameEvent
    {
        GameTime() { }

        /// <summary>
        /// 时间是否更新?
        /// </summary>
        [SerializeField]
        bool activate = true;

        /// <summary>
        /// 游戏时间缩放;
        /// </summary>
        [SerializeField]
        float timeScale = 1;

        /// <summary>
        /// 游戏经过的时间单位;
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

        public bool Activate
        {
            get { return activate; }
            set { activate = value; }
        }

        /// <summary>
        /// 游戏经过的时间单位;
        /// </summary>
        public ulong Time
        {
            get { return time.Value; }
        }

        /// <summary>
        /// 监视时间变化;
        /// </summary>
        public IObservable<ulong> ObservableTime
        {
            get { return time; }
        }

        /// <summary>
        /// 游戏时间缩放;
        /// </summary>
        public float TimeScale
        {
            get { return timeScale; }
            set { SetTimeScale(value); }
        }

        void Awake()
        {
            timeUpdateYieldInstruction = new WaitForSeconds(timeScale);
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
