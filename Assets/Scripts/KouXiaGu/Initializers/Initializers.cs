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
        private FrameCountType CheckType = FrameCountType.Update;

        private readonly bool publishEveryYield = false;

        [SerializeField]
        private GameInitialize buildGame;
        [SerializeField]
        private ArchiveInitialize archive;
        [SerializeField]
        private QuitInitialize quit;

        public GameStatus State { get { return state.Value; } private set { state.Value = value; } }
        public IReadOnlyReactiveProperty<GameStatus> StateReactive { get { return state; } }

        public bool CanBuildGame { get { return State == GameStatus.Ready; } }
        public bool CanSaveGame { get { return State == GameStatus.Running; } }
        public bool CanQuitGame { get { return State == GameStatus.Running; } }

        public IAppendComponent<IBuildGameInCoroutine, IBuildGameInThread> BuildGame { get { return buildGame; } }
        public IAppendComponent<IArchiveInCoroutine, IArchiveInThread> Archive { get { return archive; } }
        public IAppendComponent<IQuitInCoroutine, IQuitInThread> Quit { get { return quit; } }

        private void Awake()
        {
            buildGame.Awake();
            archive.Awake();
            quit.Awake();
        }

        public ICancelable Build(BuildGameData buildGameRes, Action onComplete = null, Action<Exception> onFail = null)
        {
            if (!CanBuildGame)
                throw new Exception("当前状态无法开始游戏!");

            Action<Exception> onBuildingFail = e => OnBuildingFail(e, onFail);
            Action onBuiltComplete = () => OnBuiltComplete(onComplete);

            Func<IEnumerator> coroutine = () => buildGame.Start(buildGameRes, onBuiltComplete, onBuildingFail);
            Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).Subscribe();
            OnBuilding();
            return buildGame;
        }

        public ICancelable Save(ArchivedGroup archivedGroup, Action onComplete = null, Action<Exception> onFail = null)
        {
            if (!CanSaveGame)
                throw new Exception("当前状态无法保存游戏!");

            Action<Exception> onSavingFail = e => OnSavingFail(e, onFail);
            Action onSavedComplete = () => OnSavedComplete(onComplete);

            Func<IEnumerator> coroutine = () => archive.Start(archivedGroup, onSavedComplete, onSavingFail);
            Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).Subscribe();
            OnSaving();
            return archive;
        }

        public ICancelable QuitToMain(Unit unit, Action onComplete = null, Action<Exception> onFail = null)
        {
            if (!CanQuitGame)
                throw new Exception("当前状态无法退出游戏!");

            Action<Exception> onQuittingFail = e => OnQuittingFail(e, onFail);
            Action onQuitComplete = () => OnQuittedComplete(onComplete);

            Func<IEnumerator> coroutine = () => quit.Start(Unit.Default, onQuitComplete, onQuittingFail);
            Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).Subscribe();
            OnQuitting();
            return quit;
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

            buildGame.FindFromGameObject = true;
            buildGame.BaseComponents = gameController;

            archive.FindFromGameObject = true;
            archive.BaseComponents = gameController;

            quit.FindFromGameObject = true;
            quit.BaseComponents = gameController;
        }

#endif

    }

}
