using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{

    public interface IBuildGameInCoroutine : ICoroutineInitialize<BuildGameData> { }
    public interface IBuildGameInThread : IThreadInitialize<BuildGameData> { }

    public interface IQuitInCoroutine : ICoroutineInitialize<Unit> { }
    public interface IQuitInThread : IThreadInitialize<Unit> { }

    public interface IArchiveInCoroutine : ICoroutineInitialize<ArchivedGroup> { }
    public interface IArchiveInThread : IThreadInitialize<ArchivedGroup> { }


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


        public GameStatus State
        {
            get { return state.Value; }
            private set { state.Value = value; }
        }
        public IReadOnlyReactiveProperty<GameStatus> StateReactive
        {
            get { return state; }
        }

        public IAppendInitialize<IBuildGameInCoroutine, IBuildGameInThread> AppendBuildGame
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

        public ICancelable Build(BuildGameData buildGameRes)
        {
            CheckBuild();
            OnBuilding();
            Func<IEnumerator> coroutine = () => buildInitializers.Start(buildGameRes, OnBuiltComplete, OnBuildingFail);
            Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).Subscribe();
            return buildInitializers;
        }

        public ICancelable Save(ArchivedGroup archivedGroup)
        {
            CheckSave();
            OnSaving();

            Action onComplete = () => dataGame.OnSavedComplete(archivedGroup, OnSavedComplete, OnSavingFail);
            Func<IEnumerator> coroutine = () => archiveInitializers.Start(archivedGroup, onComplete, OnSavingFail);
            Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).Subscribe();
            return archiveInitializers;
        }

        public ICancelable Quit()
        {
            CheckQuit();
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

        private void OnBuilding()
        {
            State = GameStatus.Creating;
        }
        private void OnBuiltComplete()
        {
            State = GameStatus.Running;
        }
        private void OnBuildingFail(Exception error)
        {
            foreach (Exception item in error.Data.Values)
            {
                Debug.LogError("创建游戏失败!\n" + item);
            }
            State = GameStatus.Ready;
        }


        private void OnSaving()
        {
            State = GameStatus.Saving;
        }
        private void OnSavedComplete()
        {
            State = GameStatus.Running;
        }
        private void OnSavingFail(Exception error)
        {
            foreach (Exception item in error.Data.Values)
            {
                Debug.LogError("保存游戏失败!" + item);
            }
            State = GameStatus.Running;
        }


        private void OnQuitting()
        {
            State = GameStatus.Quitting;
        }
        private void OnQuittedComplete()
        {
            State = GameStatus.Ready;
        }
        private void OnQuittingFail(Exception error)
        {
            Debug.LogError("退出游戏失败!");

            State = GameStatus.Running;
        }


        [Serializable]
        private class GameInitialize : InitializeAppend<IBuildGameInCoroutine, IBuildGameInThread, BuildGameData>
        {
            private GameInitialize() : base() { }
        }

        [Serializable]
        private class QuitInitialize : InitializeAppend<IQuitInCoroutine, IQuitInThread, Unit>
        {
            public QuitInitialize() : base() { }
        }

        [Serializable]
        private sealed class ArchiveInitialize : InitializeAppend<IArchiveInCoroutine, IArchiveInThread, ArchivedGroup>
        {
            private ArchiveInitialize() : base() { }
        }

    }

}
