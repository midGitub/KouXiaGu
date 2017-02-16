using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 游戏阶段的初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public class GameScheduler : OperateWaiter
    {
        GameScheduler() { }


        /// <summary>
        /// 指定初始化存档,当前游戏的存档;
        /// </summary>
        public static ArchiveFile Archived { get; set; }

        /// <summary>
        /// 从存档初始化?
        /// </summary>
        public static bool IsFromArchived
        {
            get { return Archived != null; }
        }

        /// <summary>
        /// 提供外部设置,在游戏初始化完成后调用;
        /// </summary>
        [SerializeField]
        Action onGameInitializedEvent;

        /// <summary>
        /// 在游戏初始化完成后调用,在Update之前加入有效;
        /// </summary>
        public event Action OnGameInitializedEvent
        {
            add { onGameInitializedEvent += value; }
            remove { onGameInitializedEvent -= value; }
        }

        void Awake()
        {
            if (IsFromArchived)
            {
                StartGameFromArchive();
            }
            else
            {
                StartGame();
            }
        }

        /// <summary>
        /// 直接开始游戏;
        /// </summary>
        void StartGame()
        {
            var operaters = GetComponentsInChildren<IStartOperate>();
            var errorList = new List<IStartOperate>();

            for (int i = 0; i < operaters.Length; i++)
            {
                var operater = operaters[i];
                try
                {
                    Action callBack = operater.Initialize();
                    onGameInitializedEvent += callBack;
                }
                catch (Exception e)
                {
                    Debug.LogError("跳过等待" + operater + ",因为在初始化时遇到异常:\n" + e);
                    OnFail(operater, e);
                    errorList.Add(operater);
                }
            }

            StartWait(operaters.
                Where(item => !errorList.Contains(item)).
                Cast<IOperateAsync>().ToList());
        }

        /// <summary>
        /// 从存档开始游戏;
        /// </summary>
        void StartGameFromArchive()
        {
            var operaters = GetComponentsInChildren<IRecoveryOperate>();
            var errorList = new List<IRecoveryOperate>();

            foreach (var operater in operaters)
            {
                try
                {
                    operater.Initialize(Archived);
                }
                catch (Exception e)
                {
                    Debug.LogError("跳过等待" + operater + ",因为从存档初始化时遇到异常:\n" + e);
                    OnFail(operater, e);
                    errorList.Add(operater);
                }
            }

            StartWait(operaters.
              Where(item => !errorList.Contains(item)).
              Cast<IOperateAsync>().ToList());
        }

        protected override void OnComplete(IOperateAsync operater)
        {
            return;
        }

        /// <summary>
        /// 当所有完成时调用;
        /// </summary>
        protected override void OnCompleteAll()
        {
            if (onGameInitializedEvent != null)
                onGameInitializedEvent();
            enabled = false;
        }

        /// <summary>
        /// 当出现异常是调用;
        /// </summary>
        protected override void OnFail(IOperateAsync operater, Exception e)
        {
            Debug.LogError(e);
        }


        void OnDestroy()
        {
            Archived = null;
        }

    }

}
