using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{

    public interface IBuildInCoroutine : ICoroutineInit<BuildGameData> { }
    public interface IBuildInThread : IThreadInit<BuildGameData> { }

    public interface IQuitInCoroutine : ICoroutineInit<Unit> { }
    public interface IQuitInThread : IThreadInit<Unit> { }

    public interface IArchiveInCoroutine : ICoroutineInit<ArchivedGroup> { }
    public interface IArchiveInThread : IThreadInit<ArchivedGroup> { }


    public class Initializers : MonoBehaviour
    {
        private Initializers() { }

        [SerializeField]
        private GameStatusReactiveProperty state = new GameStatusReactiveProperty(GameStatus.Ready);

        [SerializeField]
        private FrameCountType CheckType = FrameCountType.Update;
        private readonly bool publishEveryYield = false;

        [SerializeField]
        private DataGame dataGame;

        [SerializeField]
        private GameInitialize buildInitializers;

        [SerializeField]
        private ArchiveInitialize archiveInitializers;

        [SerializeField]
        private QuitInitialize quitInitializers;

        /// <summary>
        /// 当前事件执行完成后调用,调用完后清空;
        /// </summary>
        public event Action OnComplete;

        public GameStatus State
        {
            get { return state.Value; }
            private set { state.Value = value; }
        }
        public IReadOnlyReactiveProperty<GameStatus> StateReactive
        {
            get { return state; }
        }

        public DataGame DataGame
        {
            get { return dataGame; }
        }
        public IAppendInitialize<IBuildInCoroutine, IBuildInThread> AppendBuildGame
        {
            get { return buildInitializers; }
        }
        public IAppendInitialize<IArchiveInCoroutine, IArchiveInThread> AppendArchiveGame
        {
            get { return archiveInitializers; }
        }
        public IAppendInitialize<IQuitInCoroutine, IQuitInThread> AppendQuitGame
        {
            get { return quitInitializers; }
        }


        private void Awake()
        {
            AddendInGameObejct();
        }


        public ICancelable Build(BuildGameData buildGameRes, Action onComplete = null)
        {
            CheckBuild();
            OnComplete += onComplete;
            OnBuilding();

            Func<IEnumerator> coroutine = () => buildInitializers.Start(buildGameRes, OnBuiltComplete, OnBuildingFail);
            Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).Subscribe();
            return buildInitializers;
        }

        public ICancelable Save(ArchivedGroup archivedGroup, Action onComplete = null)
        {
            CheckSave();
            OnComplete += onComplete;
            OnSaving();

            Action coroutineComplete = () => dataGame.OnSavedComplete(archivedGroup, OnSavedComplete);
            Func<IEnumerator> coroutine = () => archiveInitializers.Start(archivedGroup, coroutineComplete, OnSavingFail);
            Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).Subscribe();
            return archiveInitializers;
        }

        public ICancelable Quit(Action onComplete = null)
        {
            CheckQuit();
            OnComplete += onComplete;
            OnQuitting();

            Func<IEnumerator> coroutine = () => quitInitializers.Start(Unit.Default, OnQuittedComplete, OnQuittingFail);
            Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).Subscribe();
            return quitInitializers;
        }

        public void CheckBuild()
        {
            if (State != GameStatus.Ready)
                throw new Exception("当前状态无法开始游戏!");
        }
        public void CheckSave()
        {
            if (State != GameStatus.Running)
                throw new Exception("当前状态无法保存游戏!");
        }
        public void CheckQuit()
        {
            if (State != GameStatus.Running)
                throw new Exception("当前状态无法退出游戏!");
        }

        private void OnEventComplete()
        {
            if (OnComplete != null)
            {
                OnComplete.Invoke();
                OnComplete = null;
            }
        }

        private void OnBuilding()
        {
            State = GameStatus.Creating;
        }
        private void OnBuiltComplete()
        {
            State = GameStatus.Running;
            OnEventComplete();
        }
        private void OnBuildingFail(List<Exception> errorList)
        {
            State = GameStatus.Ready;
            OnEventComplete();
        }


        private void OnSaving()
        {
            State = GameStatus.Saving;
        }
        private void OnSavedComplete()
        {
            State = GameStatus.Running;
            OnEventComplete();
        }
        private void OnSavingFail(List<Exception> errorList)
        {
            State = GameStatus.Running;
            OnEventComplete();
        }


        private void OnQuitting()
        {
            State = GameStatus.Quitting;
        }
        private void OnQuittedComplete()
        {
            State = GameStatus.Ready;
            OnEventComplete();
        }
        private void OnQuittingFail(List<Exception> errorList)
        {
            State = GameStatus.Running;
            OnEventComplete();
        }


        /// <summary>
        /// 将本GameObejct上所有的子物体,包括本身都获取到初始化接口(不包括未激活的脚本);
        /// </summary>
        private void AddendInGameObejct()
        {
            AddendBuild();
            AddendArchive();
            AddQuit();
        }

        private void AddendBuild()
        {
            Addend(buildInitializers);
        }

        private void AddendArchive()
        {
            Addend(archiveInitializers);
        }

        private void AddQuit()
        {
            Addend(quitInitializers);
        }

        private void Addend<T, T2>(IAppendInitialize<T, T2> appendInitialize)
        {
            T[] TArray = GetInitInterface<T>();
            T2[] T2Array = GetInitInterface<T2>();

            foreach (var item in TArray)
            {
                appendInitialize.Add(item);
            }
            foreach (var item in T2Array)
            {
                appendInitialize.Add(item);
            }
        }

        private T[] GetInitInterface<T>()
        {
            return GetComponentsInChildren<T>();

        }

        [Serializable]
        private class GameInitialize : InitHelper<IBuildInCoroutine, IBuildInThread, BuildGameData>
        {
            private GameInitialize() : base() { }
        }

        [Serializable]
        private class QuitInitialize : InitHelper<IQuitInCoroutine, IQuitInThread, Unit>
        {
            public QuitInitialize() : base() { }
        }

        [Serializable]
        private sealed class ArchiveInitialize : InitHelper<IArchiveInCoroutine, IArchiveInThread, ArchivedGroup>
        {
            private ArchiveInitialize() : base() { }
        }

    }

}
