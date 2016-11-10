using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{

    [Serializable]
    public class GameCreater : ILoadRes
    {
        public GameCreater()
        {
            runningCoroutines = new Dictionary<ILoadInCoroutine, IDisposable>();
            runningThreads = new HashSet<ILoadInThread>();
        }

        [SerializeField]
        private GameObject BaseComponents;
        private ArchiveExpand archive;
        private IEnumerable<string> resPath;

        private Dictionary<ILoadInCoroutine, IDisposable> runningCoroutines;
        private HashSet<ILoadInThread> runningThreads;

        public bool IsRunning
        {
            get { return runningCoroutines.Count != 0 || runningThreads.Count != 0; }
        }
        private IEnumerable<ILoadInCoroutine> LoadInCoroutineComponents
        {
            get { return BaseComponents.GetComponentsInChildren<ILoadInCoroutine>(); }
        }
        private IEnumerable<ILoadInThread> LoadInThreadComponents
        {
            get { return BaseComponents.GetComponentsInChildren<ILoadInThread>(); }
        }

        ArchiveExpand ILoadRes.Archive { get { return archive; } }
        IEnumerable<string> ILoadRes.ResPath { get { return resPath; } }


        public IEnumerator Load(ILoadRes loadRes)
        {
            throw new NotImplementedException();
        }

        public void Load1(IObserver<Unit> observer, ArchiveExpand archive, IEnumerable<string> resPath)
        {
            if (IsRunning)
                throw new Exception();

            this.archive = archive;
            this.resPath = resPath;

            StartInCoroutine(observer, this);
            StartInThread(observer, this);
        }

        private void StartInCoroutine(IObserver<Unit> observer, ILoadRes loadRes)
        {
            foreach (var loadInCoroutineComponent in LoadInCoroutineComponents)
            {
                StartInCoroutine(loadInCoroutineComponent, observer, loadRes);
            }
        }

        private void StartInCoroutine(ILoadInCoroutine loadInCoroutineComponent, IObserver<Unit> observer, ILoadRes loadRes)
        {
            Action loadingDoneCallBreak = () => runningCoroutines.Remove(loadInCoroutineComponent);
            Func<IEnumerator> coroutine = () => loadInCoroutineComponent.Load(loadRes);

            IDisposable disposable =
                Observable.FromMicroCoroutine(coroutine, false, FrameCountType.FixedUpdate).
                Subscribe(observer.OnNext, observer.OnError, loadingDoneCallBreak);

            runningCoroutines.Add(loadInCoroutineComponent, disposable);
        }

        private void StartInThread(IObserver<Unit> observer, ILoadRes loadRes)
        {
            foreach (var loadInThreadComponent in LoadInThreadComponents)
            {
                StartInThread(loadInThreadComponent, observer, loadRes);
            }
        }

        private void StartInThread(ILoadInThread loadInThreadComponent, IObserver<Unit> observer, ILoadRes loadRes)
        {
            Action loadingDoneCallBreak = () => runningThreads.Remove(loadInThreadComponent);
            WaitCallback waitCallback = _ => loadInThreadComponent.Load(loadRes);

            runningThreads.Add(loadInThreadComponent);
            ThreadPool.QueueUserWorkItem(waitCallback);
        }

        public void Unload(IObserver<Unit> observer)
        {

        }

    }

}
