using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Initialization;
using UnityEngine;


namespace KouXiaGu.World
{

    /// <summary>
    /// 游戏时间系统初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public class TimeInitializer : MonoBehaviour, IStartOperate, IRecoveryOperate, IArchiveOperate
    {

        /// <summary>
        /// 游戏开始时间;
        /// </summary>
        public static DateTime StartTime;


        [SerializeField]
        WorldTimer timer;

        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public Exception Ex { get; private set; }

        void ResetState()
        {
            IsCompleted = false;
            IsFaulted = false;
            Ex = null;
        }

        Action IStartOperate.Initialize()
        {
            ResetState();
            try
            {
                timer.CurrentDateTime = StartTime;

                IsCompleted = true;
                return OnInitialized;
            }
            catch (Exception e)
            {
                IsFaulted = true;
                Ex = e;
                return null;
            }
        }

        /// <summary>
        /// 当初始化完成后(开始游戏时)调用;
        /// </summary>
        void OnInitialized()
        {
            timer.IsRunning = true;
        }


        Action IRecoveryOperate.Initialize(ArchiveFile archive)
        {
            ResetState();
            throw new NotImplementedException();
        }

        void IArchiveOperate.SaveState(ArchiveFile archive)
        {
            ResetState();
            throw new NotImplementedException();
        }

    }

}
