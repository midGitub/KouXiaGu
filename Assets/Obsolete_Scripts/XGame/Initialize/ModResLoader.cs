using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace XGame
{

    /// <summary>
    /// 资源控制器,控制在游戏过程中可能只需要读取一次的资源,在新游戏仍然可以复用的资源;
    /// 通过挂载IModLoad,且在同一个组件之下,会自动调用对应方法;
    /// </summary>
    [DisallowMultipleComponent]
    public class ModResLoader : ModRes
    {
        protected ModResLoader() { }

        /// <summary>
        /// 当所有资源读取完毕时调用;
        /// </summary>
        [SerializeField]
        private UnityEvent m_OnComplete;

        /// <summary>
        /// 游戏读取的协程;
        /// </summary>
        private Coroutine m_LoadingCoroutine;

        /// <summary>
        /// 读取进度;
        /// </summary>
        public Schedule Schedule { get; private set; }

        /// <summary>
        /// 是否已经初始化完成?
        /// </summary>
        public bool IsDone { get; private set; }

        /// <summary>
        /// 是否正在读取资源?
        /// </summary>
        public bool IsLoading{ get { return m_LoadingCoroutine != null; } }

        /// <summary>
        /// 当所有资源读取完毕时调用;
        /// </summary>
        public UnityEvent OnComplete { get { return m_OnComplete; } }

        /// <summary>
        /// 游戏状态;
        /// </summary>
        private StatusType GameState
        {
            get { return StateController.GetInstance.GameState; }
            set { StateController.GetInstance.GameState = value; }
        }

        protected void Awake()
        {
            IsDone = false;
        }

        private void Start()
        {
            LoadModRes();
        }

        /// <summary>
        /// 是否允许读取Mod资源;
        /// </summary>
        private void IsLoadingModRes()
        {
            if (IsLoading || IsDone || GameState != StatusType.Ready)
                throw new Exception("游戏已经读取完毕或者正在读取!");
        }

        /// <summary>
        /// 根据给定顺序读取Mod资源;
        /// </summary>
        /// <param name="modInfos"></param>
        public void LoadModRes(IEnumerable<ModInfo> modInfos)
        {
            IsLoadingModRes();

            GameState = StatusType.ModLoading;
            AsyncLoadModRes(modInfos);
        }

        /// <summary>
        /// 根据随机顺序读取所有Mod资源;
        /// </summary>
        public void LoadModRes()
        {
            IsLoadingModRes();

            GameState = StatusType.ModLoading;
            IEnumerable<ModInfo> modInfos = GetModInfos();
            AsyncLoadModRes(modInfos);
        }

        /// <summary>
        /// 开始按顺序读取游戏Mod资源;
        /// </summary>
        private void AsyncLoadModRes(IEnumerable<ModInfo> modInfos)
        {
            IModLoad[] modules = GetComponentsInChildren<IModLoad>();
            Func<ModInfo, IModLoad, IEnumerator> func = (modInfo, load) => load.Load(modInfo);
            int loadTotal = modInfos.Count() * modules.Length;
            Schedule schedule = new Schedule(loadTotal);
            Action callBreak = LoadingDone;
            var loadAsync = AsyncHelper.OrderLoadAsync(modInfos, modules, func, schedule, callBreak);

            m_LoadingCoroutine = StartCoroutine(loadAsync);
        }

        /// <summary>
        /// 读取完毕时调用;
        /// </summary>
        private void LoadingDone()
        {
            IsDone = true;
            m_LoadingCoroutine = null;
            GameState = StatusType.WaitStart;
            OnComplete.Invoke();
        }

    }

}
