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

        private static IBuildGameData buildGameData;

        /// <summary>
        /// 获取到游戏创建接口;
        /// </summary>
        public static IBuildGameData BuildGameData
        {
            get { return buildGameData ?? (buildGameData = GetBuildGameDataObject()); }
        }

        public GameStatus State
        {
            get { return state.Value; }
            private set { state.Value = value; }
        }
        public IReadOnlyReactiveProperty<GameStatus> StateReactive
        {
            get { return state; }
        }

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

        [ContextMenu("测试!开始游戏!")]
        private void Test_Start()
        {
            Action onComplete = () => Debug.Log("读取成功!");
            Build(buildGame.GetBuildGameData(), onComplete);
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
            foreach (Exception item in error.Data.Values)
            {
                Debug.LogError("创建游戏失败!\n" + item);
            }

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

        private static IBuildGameData GetBuildGameDataObject()
        {
            return GameObject.FindWithTag("GameController").GetComponent<Initializers>().buildGame;
        }

#if UNITY_EDITOR

        //[ContextMenu("设置所有从 GameController 获取")]
        //private void Test_SetAllFromGameController()
        //{
        //    GameObject gameController = GameObject.FindWithTag("GameController");

        //    AppendBuildGame.FindFromGameObject = true;
        //    AppendBuildGame.BaseGameObject = gameController;

        //    AppendArchiveGame.FindFromGameObject = true;
        //    AppendArchiveGame.BaseGameObject = gameController;

        //    AppendQuitGame.FindFromGameObject = true;
        //    AppendQuitGame.BaseGameObject = gameController;
        //}

#endif

    }

}
