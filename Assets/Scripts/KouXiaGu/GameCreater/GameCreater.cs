using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;
using System.Collections;
using System.Threading;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏创建;
    /// </summary>
    [DisallowMultipleComponent]
    public class GameCreater : MonoBehaviour
    {

        [SerializeField]
        private GameStatusReactiveProperty state;

        public GameStatus State { get { return state.Value; } private set { state.Value = value; } }
        public IReactiveProperty<GameStatus> StateReactive { get { return state; } }
        public bool CanCreateGame { get { return State == GameStatus.Ready; } }
        public bool CanSaveGame { get { return State == GameStatus.Running; } }

        private void Start()
        {

        }


    }


    public class LoadControl : ILoadRes
    {
        public LoadControl(
            IEnumerable<ILoadInCoroutine> loadInCoroutines, 
            IEnumerable<ILoadInThread> loadInThreads,
            bool isLoaded = false)
        {
            this.loadInCoroutines = loadInCoroutines;
            this.loadInThreads = loadInThreads;

            this.isLoaded = new ReactiveProperty<bool>(false);
            loadingCoroutines = new Dictionary<ILoadInCoroutine, IDisposable>(loadInCoroutines.Count());
            loadingThreads = new HashSet<ILoadInThread>();
            unloadingCoroutines = new HashSet<IUnloadInCoroutine>();
        }

        private static readonly Action<Unit> DefaultOnNext = null;

        private ArchiveExpand archive;
        private IEnumerable<string> resPath;

        private IEnumerable<ILoadInCoroutine> loadInCoroutines;
        private IEnumerable<ILoadInThread> loadInThreads;
        private ReactiveProperty<bool> isLoaded;
        private Dictionary<ILoadInCoroutine, IDisposable> loadingCoroutines;
        private HashSet<ILoadInThread> loadingThreads;
        private HashSet<IUnloadInCoroutine> unloadingCoroutines;

        public IReadOnlyReactiveProperty<bool> IsLoaded { get { return isLoaded; } }
        public bool IsRunning
        {
            get{ return loadingCoroutines.Count != 0 || loadingThreads.Count != 0 || unloadingCoroutines.Count != 0; }
        }

        ArchiveExpand ILoadRes.Archive { get { return archive; } }
        IEnumerable<string> ILoadRes.ResPath { get { return resPath; } }

        public void Load(ArchiveExpand archive, IEnumerable<string> resPath,
            Action<Exception> onError, Action onCompleted)
        {
            if (IsRunning)
                throw new Exception("正在进行!");
            if (isLoaded.Value)
                throw new Exception("游戏已经创建完毕!");

            this.archive = archive;
            this.resPath = resPath;

            StartInCoroutine(onError);
            StartInThread(onError);

            Observable.FromMicroCoroutine(LoadSchedule).Subscribe(DefaultOnNext, onCompleted);
        }

        private void StartInCoroutine(Action<Exception> onError)
        {
            foreach (var loadInCoroutine in loadInCoroutines)
            {
                Action loadingDoneCallBreak = () => loadingCoroutines.Remove(loadInCoroutine);
                Func<IEnumerator> coroutine = () => loadInCoroutine.Load(this, onError, loadingDoneCallBreak);

                IDisposable disposable =
                    Observable.FromMicroCoroutine(coroutine, false, FrameCountType.FixedUpdate).
                    Subscribe(DefaultOnNext, onError, loadingDoneCallBreak);

                loadingCoroutines.Add(loadInCoroutine, disposable);
            }
        }

        private void StartInThread(Action<Exception> onError)
        {
            foreach (var loadInThread in loadInThreads)
            {
                Action loadingDoneCallBreak = () => loadingThreads.Remove(loadInThread);
                WaitCallback waitCallback = _ => loadInThread.Load(this, onError, loadingDoneCallBreak);

                ThreadPool.QueueUserWorkItem(waitCallback);

                loadingThreads.Add(loadInThread);
            }
        }

        private IEnumerator LoadSchedule()
        {
            while (IsRunning)
            {
                yield return null;
            }
            isLoaded.Value = true;
            yield break;
        }


        public void Unload(Action<Exception> onError, Action onCompleted)
        {
            if (!isLoaded.Value)
                throw new Exception("尚未创建游戏!");

            var coroutine = loadInCoroutines.Cast<IUnloadInCoroutine>();
            var thread = loadInThreads.Cast<IUnloadInCoroutine>();

            HashSet<IUnloadInCoroutine> unloadInCoroutine = new HashSet<IUnloadInCoroutine>(coroutine);
            unloadInCoroutine.UnionWith(thread);

            StartUnload(unloadInCoroutine,  onError);
            Observable.FromMicroCoroutine(UnloadSchedule).Subscribe(DefaultOnNext, onCompleted);
        }

        private void StartUnload(IEnumerable<IUnloadInCoroutine> collection, Action<Exception> onError)
        {
            foreach (var item in collection)
            {
                Action unloadSuccess = () => unloadingCoroutines.Remove(item);
                Observable.FromMicroCoroutine(item.Unload).
                    Subscribe(DefaultOnNext, onError, unloadSuccess);
                unloadingCoroutines.Add(item);
            }
        }

        private IEnumerator UnloadSchedule()
        {
            while (IsRunning)
            {
                yield return null;
            }
            isLoaded.Value = false;
        }

    }


    public class SaveControl : ISaveRes
    {
        public SaveControl(
            IEnumerable<ISaveInCoroutine> saveInCoroutines,
            IEnumerable<ISaveInThread> saveInThreads)
        {
            this.saveInCoroutines = saveInCoroutines;
            this.saveInThreads = saveInThreads;

            isLoaded = new ReactiveProperty<bool>(false);
            savingCoroutines = new Dictionary<ISaveInCoroutine, IDisposable>();
            savingThreads = new HashSet<ISaveInThread>();
        }

        private static readonly Action<Unit> DefaultOnNext = null;

        private ArchiveExpand archive;
        private string path;

        IEnumerable<ISaveInCoroutine> saveInCoroutines;
        IEnumerable<ISaveInThread> saveInThreads;
        private ReactiveProperty<bool> isLoaded;
        private Dictionary<ISaveInCoroutine, IDisposable> savingCoroutines;
        private HashSet<ISaveInThread> savingThreads;

        public IReadOnlyReactiveProperty<bool> IsLoaded { get { return isLoaded; } }
        public bool IsRunning
        {
            get { return savingCoroutines.Count != 0 || savingThreads.Count != 0; }
        }

        ArchiveExpand ISaveRes.Archive { get { return archive; } }
        string ISaveRes.Path { get { return path; } }

        public void Save(ArchiveExpand archive, string path, Action<Exception> onError, Action onCompleted)
        {
            if (IsRunning)
                throw new Exception("正在进行!");
            if (isLoaded.Value)
                throw new Exception("游戏已经创建完毕!");

            this.archive = archive;
            this.path = path;

            StartInCoroutine(onError);
            StartInThread(onError);

            Observable.FromMicroCoroutine(SaveSchedule).Subscribe(DefaultOnNext, onCompleted);
        }

        private void StartInCoroutine(Action<Exception> onError)
        {
            foreach (var saveInCoroutine in saveInCoroutines)
            {
                Action loadingDoneCallBreak = () => savingCoroutines.Remove(saveInCoroutine);
                Func<IEnumerator> coroutine = () => saveInCoroutine.Save(this, onError, loadingDoneCallBreak);

                IDisposable disposable =
                    Observable.FromMicroCoroutine(coroutine, false, FrameCountType.FixedUpdate).
                    Subscribe(DefaultOnNext, onError, loadingDoneCallBreak);

                savingCoroutines.Add(saveInCoroutine, disposable);
            }
        }

        private void StartInThread(Action<Exception> onError)
        {
            foreach (var saveInThread in saveInThreads)
            {
                Action loadingDoneCallBreak = () => savingThreads.Remove(saveInThread);
                WaitCallback waitCallback = _ => saveInThread.Save(this, onError, loadingDoneCallBreak);

                ThreadPool.QueueUserWorkItem(waitCallback);

                savingThreads.Add(saveInThread);
            }
        }

        private IEnumerator SaveSchedule()
        {

            yield break;
        }

    }

}
