using System;
using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Collections;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏状态控制;
    /// </summary>
    [DisallowMultipleComponent]
    public class Initializers : MonoBehaviour
    {
        private Initializers() { }

        [SerializeField]
        private GameStatusReactiveProperty state = new GameStatusReactiveProperty(GameStatus.Ready);

        [SerializeField]
        private BuildGame buildGame;

        public GameStatus State { get { return state.Value; } private set { state.Value = value; } }
        public IReadOnlyReactiveProperty<GameStatus> StateReactive { get { return state; } }

        public bool CanBuildGame
        {
            get { return State == GameStatus.Ready; }
        }
        public bool CanSaveGame
        {
            get { return State == GameStatus.Running; }
        }
        public bool CanQuitGame
        {
            get { return State == GameStatus.Running; }
        }

        public IAppendInitialize<IBuildGameInCoroutine, IBuildGameInThread> AppendBuildGame
        {
            get { return buildGame.AppendBuildGame; }
        }
        public IAppendInitialize<IArchiveInCoroutine, IArchiveInThread> AppendArchiveGame
        {
            get { return buildGame.AppendArchiveGame; }
        }
        public IAppendInitialize<IQuitInCoroutine, IQuitInThread> AppendQuitGame
        {
            get { return buildGame.AppendQuitGame; }
        }

        private void Awake()
        {
            buildGame.Awake();
        }

        private void Start()
        {
            Action onComplete = () => Debug.Log("读取成功!");
            Build(default(BuildGameData), onComplete);
        }

        public ICancelable Build(BuildGameData buildGameRes, Action onComplete = null, Action<Exception> onFail = null)
        {
            if (!CanBuildGame)
                throw new Exception("当前状态无法开始游戏!");

            Action<Exception> onBuildingFail = e => OnBuildingFail(e, onFail);
            Action onBuiltComplete = () => OnBuiltComplete(onComplete);

            OnBuilding();
            return buildGame.Build(buildGameRes, onBuiltComplete, onBuildingFail);
        }

        public ICancelable Save(ArchivedGroup archivedGroup, Action onComplete = null, Action<Exception> onFail = null)
        {
            if (!CanSaveGame)
                throw new Exception("当前状态无法保存游戏!");

            Action<Exception> onSavingFail = e => OnSavingFail(e, onFail);
            Action onSavedComplete = () => OnSavedComplete(onComplete);

            OnSaving();
            return buildGame.Save(archivedGroup, onSavedComplete, onSavingFail);
        }

        public ICancelable QuitToMain(Unit unit, Action onComplete = null, Action<Exception> onFail = null)
        {
            if (!CanQuitGame)
                throw new Exception("当前状态无法退出游戏!");

            Action<Exception> onQuittingFail = e => OnQuittingFail(e, onFail);
            Action onQuitComplete = () => OnQuittedComplete(onComplete);

            OnQuitting();
            return buildGame.Quit(onQuitComplete, onQuittingFail);
        }


        private void OnBuilding()
        {
            State = GameStatus.Creating;
        }
        private void OnBuiltComplete(Action onComplete)
        {
            State = GameStatus.Running;

            if (onComplete != null)
                onComplete();
        }
        private void OnBuildingFail(Exception error, Action<Exception> onFail)
        {
            Debug.LogError("创建游戏失败!");

            if (onFail != null)
                onFail(error);

            State = GameStatus.Ready;
        }


        private void OnSaving()
        {
            State = GameStatus.Saving;
        }
        private void OnSavedComplete(Action onComplete)
        {
            State = GameStatus.Running;

            if (onComplete != null)
                onComplete();
        }
        private void OnSavingFail(Exception error, Action<Exception> onFail)
        {
            Debug.LogError("保存游戏失败!");

            if (onFail != null)
                onFail(error);

            State = GameStatus.Running;
        }


        private void OnQuitting()
        {
            State = GameStatus.Quitting;
        }
        private void OnQuittedComplete(Action onComplete)
        {
            State = GameStatus.Ready;

            if (onComplete != null)
                onComplete();
        }
        private void OnQuittingFail(Exception error, Action<Exception> onFail)
        {
            Debug.LogError("退出游戏失败!");

            if (onFail != null)
                onFail(error);

            State = GameStatus.Running;
        }

#if UNITY_EDITOR

        [ContextMenu("设置所有从 GameController 获取")]
        private void Test_SetAllFromGameController()
        {
            GameObject gameController = GameObject.FindWithTag("GameController");

            AppendBuildGame.FindFromGameObject = true;
            AppendBuildGame.BaseGameObject = gameController;

            AppendArchiveGame.FindFromGameObject = true;
            AppendArchiveGame.BaseGameObject = gameController;

            AppendQuitGame.FindFromGameObject = true;
            AppendArchiveGame.BaseGameObject = gameController;
        }

#endif

    }

}
