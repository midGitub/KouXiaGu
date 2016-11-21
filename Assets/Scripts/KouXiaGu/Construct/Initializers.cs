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

    [DisallowMultipleComponent]
    public class Initializers : MonoBehaviour
    {

        [SerializeField]
        GameStatusReactiveProperty state = new GameStatusReactiveProperty(GameStatus.Ready);

        [SerializeField]
        FrameCountType CheckType = FrameCountType.Update;
        readonly bool publishEveryYield = false;

        [SerializeField]
        private DataGame dataGame;

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

        public IDisposable Build(BuildGameData buildGameRes, Action onComplete = null)
        {
            CheckBuild();
            OnBuilding();

            Func<IEnumerator> coroutine = () => Constructer.Start(buildGameRes);
            return Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).
                Subscribe(null, OnBuildingFail, OnBuiltComplete);
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
            State = GameStatus.Ready;
            Debug.Log(error);
        }

    }

}
