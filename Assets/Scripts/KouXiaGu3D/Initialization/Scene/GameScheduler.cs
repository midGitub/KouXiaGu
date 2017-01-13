using System;
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

        static event Action onGameInitializedEvent;

        /// <summary>
        /// 在游戏初始化完成后调用,调用后既清空;
        /// </summary>
        public event Action OnGameInitializedEvent
        {
            add { onGameInitializedEvent += value; }
            remove { onGameInitializedEvent -= value; }
        }


        /// <summary>
        /// 提供外部设置,在游戏初始化完成后调用;
        /// </summary>
        [SerializeField]
        UnityEvent onGameInitialized;

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

        void StartGame()
        {
            var operates = GetComponentsInChildren<IStartOperate>();
            foreach (var operate in operates)
            {
                operate.Initialize();
            }
            StartWait(operates);
        }

        void StartGameFromArchive()
        {
            var operates = GetComponentsInChildren<IRecoveryOperate>();
            foreach (var operate in operates)
            {
                operate.Initialize(Archived);
            }
            StartWait(operates);
        }

        protected override void OnComplete(IOperateAsync operater)
        {
            return;
        }

        protected override void OnCompleteAll()
        {
            if (onGameInitializedEvent != null)
            {
                onGameInitializedEvent();
                onGameInitializedEvent = null;
            }

            onGameInitialized.Invoke();

            enabled = false;
        }

        protected override void OnFail(IOperateAsync operater)
        {
            Debug.LogError(operater.Ex);
        }


        void OnDestroy()
        {
            Archived = null;
        }

    }

}
