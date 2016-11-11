using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{

    public interface ICoroutineInitialize<T>
    {
        IEnumerator Initialize(T item, ICancelable cancelable, Action<Exception> onError);
    }

    public interface IThreadInitialize<T>
    {
        void Initialize(T item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak);
    }

    [Serializable]
    public abstract class InitializeBase<Coroutine, Thread, T> : ICancelable
        where Coroutine : ICoroutineInitialize<T>
        where Thread : IThreadInitialize<T>
    {

        public InitializeBase()
        {
            runningCoroutines = new HashSet<Coroutine>();
            runningThreads = new HashSet<Thread>();
        }

        [SerializeField]
        private FrameCountType updateType = FrameCountType.FixedUpdate;

        [SerializeField]
        private bool needStop = false;

        private HashSet<Coroutine> runningCoroutines;
        private HashSet<Thread> runningThreads;

        public FrameCountType UpdateType
        {
            get { return updateType; }
            set { updateType = value; }
        }
        public bool IsDisposed
        {
            get { return needStop; }
        }
        public bool IsRunning
        {
            get { return runningCoroutines.Count != 0 || runningThreads.Count != 0; }
        }
        protected abstract IEnumerable<Coroutine> LoadInCoroutineComponents { get; }
        protected abstract IEnumerable<Thread> LoadInThreadComponents { get; }

        public IEnumerator Start(T item, Action<Exception> onError, Action onInitialized, Action onFail)
        {
            if (IsRunning)
                onError(new Exception("已经在初始化中!"));

            StartAllCoroutine(item, onError);
            StartAllThread(item, onError);

            while (IsRunning)
                yield return null;

            if (IsDisposed)
            {
                needStop = false;
                onFail(); //因为中途停止导致初始化失败!
                yield break;
            }
            else
            {
                onInitialized();
            }
        }

        public void Dispose()
        {
            if (IsRunning)
            {
                needStop = true;
            }
        }

        protected IEnumerator GetCoroutine(Coroutine loadInCoroutineComponent, T item,
            ICancelable cancelable, Action<Exception> onError)
        {
            return loadInCoroutineComponent.Initialize(item, cancelable, onError);
        }

        protected WaitCallback GetThreadThread(Thread loadInThreadComponent, T item,
            ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            return _ => loadInThreadComponent.Initialize(item, cancelable, onError, runningDoneCallBreak);
        }

        protected void StartAllCoroutine(T item, Action<Exception> onError)
        {
            foreach (var loadInCoroutineComponent in LoadInCoroutineComponents)
            {
                StartInCoroutine(loadInCoroutineComponent, item, onError);
            }
        }

        protected void StartInCoroutine(Coroutine loadInCoroutineComponent, T item, Action<Exception> onError)
        {
            Action runningDoneCallBreak = () => runningCoroutines.Remove(loadInCoroutineComponent);
            IEnumerator coroutine = GetCoroutine(loadInCoroutineComponent, item, this, onError);

            Observable.FromMicroCoroutine(coroutine, false, UpdateType).Subscribe(null, onError, runningDoneCallBreak);

            runningCoroutines.Add(loadInCoroutineComponent);
        }

        protected void StartAllThread(T item, Action<Exception> onError)
        {
            foreach (var loadInThreadComponent in LoadInThreadComponents)
            {
                StartInThread(loadInThreadComponent, item, onError);
            }
        }

        protected void StartInThread(Thread loadInThreadComponent, T item, Action<Exception> onError)
        {
            Action loadingDoneCallBreak = () => runningThreads.Remove(loadInThreadComponent);
            WaitCallback waitCallback = GetThreadThread(loadInThreadComponent, item, this, onError, loadingDoneCallBreak);

            runningThreads.Add(loadInThreadComponent);
            ThreadPool.QueueUserWorkItem(waitCallback);
        }

    }

}
