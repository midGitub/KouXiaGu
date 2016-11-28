using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{

    public interface IStartGameEvent : IConstruct<BuildGameData> { }
    public interface IArchiveEvent : IConstruct<ArchivedGroup> { }
    public interface IQuitGameEvent : IConstruct<QuitGameData> { }

    [DisallowMultipleComponent]
    public class Initializers : MonoBehaviour
    {

        [SerializeField]
        GameStatusReactiveProperty state = new GameStatusReactiveProperty(GameStatus.Ready);

        [SerializeField]
        FrameCountType CheckType = FrameCountType.Update;
        readonly bool publishEveryYield = false;

        public GameStatus State
        {
            get { return state.Value; }
            private set { state.Value = value; }
        }
        public IReadOnlyReactiveProperty<GameStatus> StateReactive
        {
            get { return state; }
        }

        public IDisposable Build(BuildGameData buildGameRes, Action onComplete = null)
        {
            CheckBuild();
            OnBuilding();

            Func<IEnumerator> coroutine = () => Constructer.Start(buildGameRes);
            return Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).
                Subscribe(null, OnBuildingFail, () => OnBuiltComplete(onComplete));
        }

        public IDisposable Save(ArchivedGroup archivedGroup, Action onComplete = null)
        {
            CheckSave();
            OnSaving();

            Func<IEnumerator> coroutine = () => Constructer.Start(archivedGroup);
            return Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).
               Subscribe(null, OnSavingFail, () => OnSavedComplete(onComplete));
        }

        public IDisposable Quit(QuitGameData quitGameData, Action onComplete = null)
        {
            CheckQuit();
            OnQuitting();

            Func<IEnumerator> coroutine = () => Constructer.Start(quitGameData);
            return Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).
               Subscribe(null, OnQuittingFail, () => OnQuittedComplete(onComplete));
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

        void OnBuilding()
        {
            State = GameStatus.Creating;
        }
        void OnBuiltComplete(Action onComplete)
        {
            State = GameStatus.Running;
            if (onComplete != null)
                onComplete();
        }
        void OnBuildingFail(Exception error)
        {
            State = GameStatus.Ready;
            Debug.Log(error);
        }

        void OnSaving()
        {
            State = GameStatus.Saving;
        }
        void OnSavedComplete(Action onComplete)
        {
            State = GameStatus.Running;
            if (onComplete != null)
                onComplete();
        }
        void OnSavingFail(Exception error)
        {
            State = GameStatus.Running;
            Debug.Log(error);
        }

        void OnQuitting()
        {
            State = GameStatus.Quitting;
        }
        void OnQuittedComplete(Action onComplete)
        {
            State = GameStatus.Ready;
            if (onComplete != null)
                onComplete();
        }
        void OnQuittingFail(Exception error)
        {
            State = GameStatus.Running;
            Debug.Log(error);
        }

    }

}
