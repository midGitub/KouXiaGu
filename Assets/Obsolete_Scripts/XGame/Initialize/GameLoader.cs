using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace XGame.Running
{

    /// <summary>
    /// 游戏读取保存类;
    /// 通过挂载 IGameLoad 或者 IGameArchive ,且在同一个组件之下,会自动调用对应方法;
    /// </summary>
    [DisallowMultipleComponent]
    public class GameLoader : Archiver
    {
        protected GameLoader() { }

        [Header("游戏读取:")]

        [SerializeField]
        private string waitForLoad;

        /// <summary>
        /// 当游戏开始时调用;
        /// </summary>
        [SerializeField]
        private UnityEvent onGameStartEvent;

        /// <summary>
        /// 同一个GameObject之下游戏初始化接口;
        /// </summary>
        private IGameLoad[] m_GameLoadArray;

        /// <summary>
        /// 读取进度;
        /// </summary>
        public Schedule Schedule { get; private set; }

        //状态的协程;
        private Coroutine m_SaveGameCoroutine;
        private Coroutine m_LoadGameCoroutine;
        private Coroutine m_QuitGameCoroutine;

        //类状态;
        public bool IsSavingGame{ get { return m_SaveGameCoroutine != null; } }
        public bool IsLoadingGame{ get { return m_LoadGameCoroutine != null; } }
        public bool IsQuitingGame { get { return m_QuitGameCoroutine != null; } }

        /// <summary>
        /// 是否从存档开始游戏?
        /// </summary>
        public bool IsStartFromSaveFile { get { return waitForLoad != ""; } }

        /// <summary>
        /// 当游戏开始时调用;
        /// </summary>
        public UnityEvent OnGameStartEvent { get { return onGameStartEvent; } }

        private Archiver archiver { get { return this; } }

        /// <summary>
        /// 设置到存档位置,若设置了此项,则开始游戏时根据这个存档位置读取游戏;
        /// </summary>
        public string WaitForLoadFilePath
        {
            get { return waitForLoad; }
            set { waitForLoad = value; }
        }

        /// <summary>
        /// 游戏状态;
        /// </summary>
        private StatusType GameState
        {
            get { return StateController.GetInstance.GameState; }
            set { StateController.GetInstance.GameState = value; }
        }


        private void Awake()
        {
            m_GameLoadArray = GetComponentsInChildren<IGameLoad>();
        }

        /// <summary>
        /// 开始游戏;
        /// </summary>
        [ContextMenu("开始游戏")]
        public void StartGame()
        {
            if (GameState != StatusType.WaitStart)
                throw new Exception("无法开始游戏!模组还未读取完毕,或者游戏正在进行中;");

            GameState = StatusType.GameLoading;
            StartGame(m_GameLoadArray);
        }

        private void StartGame(IGameLoad[] gameLoads)
        {
            Func<IGameLoad, IEnumerator> func;
            GameSaveInfo gameSaveInfo;
            GameSaveData gameSaveData;
            IEnumerator loadEnumerator;
            Schedule = new Schedule(gameLoads.Length);

            if (IsStartFromSaveFile)
            {
#if UNITY_EDITOR
                Debug.Log("从存档开始游戏;");
#endif
                archiver.Load(waitForLoad, out gameSaveInfo, out gameSaveData);
                func = gameLoad => (gameLoad is IGameArchive) ? (gameLoad as IGameArchive).OnLoad(gameSaveInfo, gameSaveData) : gameLoad.OnStart();
                loadEnumerator = AsyncHelper.OrderLoadAsync(gameLoads, func, Schedule, GameLoadingDone);
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("从信息初始化游戏;");
#endif
                func = gameLoad => gameLoad.OnStart();
                loadEnumerator = AsyncHelper.OrderLoadAsync(gameLoads, func, Schedule, GameLoadingDone);
            }

            m_LoadGameCoroutine = StartCoroutine(loadEnumerator);
        }

        private void GameLoadingDone()
        {
            m_LoadGameCoroutine = null;
            GameState = StatusType.GameRunning;
            onGameStartEvent.Invoke();
        }


        /// <summary>
        /// 保存游戏当前状态;
        /// </summary>
        [ContextMenu("保存游戏当前状态")]
        public void SaveGame()
        {
            if (GameState != StatusType.GameRunning)
                throw new Exception("游戏不在进行状态,无法保存游戏;");

            GameState = StatusType.Saving;
            IEnumerable<IGameArchive> gameArchives = m_GameLoadArray.Where(gameLoad => gameLoad is IGameArchive).Cast<IGameArchive>();
            SaveGame(gameArchives);
        }

        private void SaveGame(IEnumerable<IGameArchive> gameArchives)
        {
            GameSaveInfo gameSaveInfo = new GameSaveInfo();
            GameSaveData gameSaveData = new GameSaveData();
            Action action = () => SavingGameDone(gameSaveInfo, gameSaveData);
            Func<IGameArchive, IEnumerator> func = gameArchive => gameArchive.OnSave(gameSaveInfo, gameSaveData);
            Schedule = new Schedule(gameArchives.Count());
            IEnumerator loadEnumerator = AsyncHelper.OrderLoadAsync(gameArchives, func, Schedule, action);
            m_SaveGameCoroutine = StartCoroutine(loadEnumerator);
        }

        private void SavingGameDone(GameSaveInfo gameSaveInfo, GameSaveData gameSaveData)
        {
            archiver.Save(gameSaveInfo, gameSaveData);
            m_SaveGameCoroutine = null;
            GameState = StatusType.GameRunning;
        }


        /// <summary>
        /// 退出到主界面;
        /// </summary>
        [ContextMenu("退出到主界面;")]
        public void QuitMainScreen()
        {
            if(GameState <= StatusType.WaitStart)
                throw new Exception("无法退出到主菜单,因为游戏还未运行;");

            GameState = StatusType.QuittingGame;
            QuitMainScreen(m_GameLoadArray);
        }

        public void QuitMainScreen(IGameLoad[] gameLoads)
        {
            Func<IGameLoad, IEnumerator> func = gameLoad => gameLoad.OnStart();
            Schedule = new Schedule(gameLoads.Length);
            IEnumerator loadEnumerator = AsyncHelper.OrderLoadAsync(gameLoads, func, Schedule, QuitingGameDone);
            m_QuitGameCoroutine = StartCoroutine(loadEnumerator);
        }

        private void QuitingGameDone()
        {
            m_QuitGameCoroutine = null;
            GameState = StatusType.WaitStart;
        }

#if UNITY_EDITOR

        //[ContextMenu("开始游戏 =>GameRunning")]
        //private void Test_StartGame()
        //{
        //    if (GameState != StatusType.GameReady)
        //        throw new Exception("游戏还未准备完成,无法开始游戏!");

        //    GameState = StatusType.GameRunning;
        //    onGameStartEvent.Invoke();
        //}

        [ContextMenu("设置读取最新存档;")]
        private void Test_LoadNewSaveFile()
        {
            var saveInfos = archiver.GetSavesInfo();
            var pair = Archiver.AscendingOrderOfTime(saveInfos).First();
            WaitForLoadFilePath = pair.Key;
        }

        //[ContextMenu("快速开始游戏;")]
        //private void Test_QuickStartGame()
        //{

        //}

#endif


    }

}
