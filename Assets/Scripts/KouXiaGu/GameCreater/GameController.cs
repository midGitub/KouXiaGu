using System;
using UnityEngine;
using UniRx;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏状态控制;
    /// </summary>
    [DisallowMultipleComponent]
    public class GameController : MonoBehaviour
    {

        [SerializeField]
        private GameStatusReactiveProperty state = new GameStatusReactiveProperty(GameStatus.Ready);

        [SerializeField]
        private FrameCountType CheckType = FrameCountType.FixedUpdate;

        private readonly bool publishEveryYield = false;

        [SerializeField]
        private CreateControl createControl;

        [SerializeField]
        private ArchiveControl archiveControl;

        [SerializeField]
        private QuitControl quitControl;

        public GameStatus State { get { return state.Value; } private set { state.Value = value; } }
        public IReadOnlyReactiveProperty<GameStatus> StateReactive { get { return state; } }

        public bool CanCreateGame { get { return State == GameStatus.Ready; } }
        public bool CanSaveGame { get { return State == GameStatus.Running; } }
        public bool CanQuitGame { get { return State == GameStatus.Running; } }

        private void Start()
        {
            Create(null);
        }

        public void Create(ICreateGameResource loadRes)
        {
            if (!CanCreateGame)
                throw new Exception();

            Observable.FromMicroCoroutine(createControl.Start(loadRes, OnCreatingError, OnCreated, OnCreatingFail), publishEveryYield, CheckType).
                Subscribe();
            OnCreating();
        }

        public void StopCreate()
        {
            createControl.Dispose();
        }

        public void Save(IArchived saveRes)
        {
            if (!CanSaveGame)
                throw new Exception();

            Observable.FromMicroCoroutine(archiveControl.Start(saveRes, OnSavingError, OnSaved, OnSavingFail), publishEveryYield, CheckType).
                Subscribe();
            OnSaving();
        }

        public void QuitGame()
        {
            if (!CanQuitGame)
                throw new Exception();

            Observable.FromMicroCoroutine(quitControl.Start(Unit.Default, OnQuittingError,OnQuitted,OnQuittinggFail), publishEveryYield, CheckType).
               Subscribe();
            OnQuitting();
        }


        private void OnCreating()
        {
            State = GameStatus.Creating;
        }
        private void OnCreated()
        {
            State = GameStatus.Running;
        }
        private void OnCreatingError(Exception error)
        {
            Debug.LogError(error);
            StopCreate();
        }
        private void OnCreatingFail(Exception error)
        {
            Debug.LogError("创建游戏失败!");
            State = GameStatus.Ready;
        }


        private void OnSaving()
        {
            State = GameStatus.Saving;
        }
        private void OnSaved()
        {
            State = GameStatus.Running;
        }
        private void OnSavingError(Exception error)
        {
            Debug.LogError(error);
        }
        private void OnSavingFail(Exception error)
        {
            Debug.LogError("保存游戏失败!");
            State = GameStatus.Running;
        }


        private void OnQuitting()
        {
            State = GameStatus.Quitting;
        }
        private void OnQuitted()
        {
            State = GameStatus.Ready;
        }
        private void OnQuittingError(Exception error)
        {
            Debug.LogError(error);
        }
        private void OnQuittinggFail(Exception error)
        {
            Debug.LogError("退出游戏失败!");
            State = GameStatus.Running;
        }

        [ContextMenu("显示所有存档")]
        private void Test_Archive()
        {
            string strLog = "存在的存档:";
              var items = archiveControl.GetSmallArchiveds();
            foreach (var item in items)
            {
                SmallArchived smallArchived = item.SmallArchived;

                strLog +=
                    "\n名称:" + smallArchived.Name +
                    "\n路径:" + item.ArchivedPath;
            }
            Debug.Log(strLog);
        }

        [ContextMenu("保存存档")]
        private void Test_SaveRandomArchive()
        {
            IArchived archived = archiveControl.GetNewArchived();
            archiveControl.SaveInDisk(archived);
        }

    }

}
