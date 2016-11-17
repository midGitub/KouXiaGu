
///define DETAILED_DEBUG
#define DETAILED_DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{

    public interface ICoroutineInit<T>
    {
        IEnumerator Initialize(T item, ICancelable cancelable, Action<Exception> onError, Action onComplete);
    }

    public interface IThreadInit<T>
    {
        void Initialize(T item, ICancelable cancelable, Action<Exception> onError, Action onComplete);
    }

    public interface IAppendInitialize<T, T2>
    {
        void Add(T2 item);
        void Add(T item);
        bool Contains(T2 item);
        bool Contains(T item);
        bool Remove(T2 item);
        bool Remove(T item);
    }

    [Serializable]
    public class InitHelper<Coroutine, Thread, T> : ICancelable, IAppendInitialize<Coroutine, Thread>
        where Coroutine : ICoroutineInit<T>
        where Thread : IThreadInit<T>
    {
        public InitHelper()
        {
            runningCoroutines = new HashSet<Coroutine>();
            runningThreads = new HashSet<Thread>();
            coroutines = new HashSet<Coroutine>();
            threads = new HashSet<Thread>();
            exceptionList = new List<Exception>();
        }

        [Header("初始化方式")]
        [SerializeField]
        private FrameCountType updateType = FrameCountType.FixedUpdate;

        [SerializeField]
        private bool needStop = false;

        private HashSet<Coroutine> runningCoroutines;
        private HashSet<Thread> runningThreads;

        private HashSet<Coroutine> coroutines;
        private HashSet<Thread> threads;

        private List<Exception> exceptionList;

        public FrameCountType UpdateType
        {
            get { return updateType; }
            set { updateType = value; }
        }

        public bool IsDisposed
        {
            get { return needStop; }
            private set { needStop = value; }
        }

        public IEnumerator Start(T item, Action onComplete, Action<List<Exception>> onError)
        {
            if (IsRunning())
            {
                exceptionList.Add(new Exception("已经在初始化中!"));
                onError(exceptionList);
                yield break;
            }
            exceptionList.Clear();
            Start(item, OnRunningError);

            while (IsRunning())
                yield return null;

            Reset(onComplete, onError);
        }

        private void Reset(Action onComplete, Action<List<Exception>> onError)
        {
            if (IsDisposed || exceptionList.Count != 0)
            {
                foreach (Exception item in exceptionList)
                {
                    Debug.LogError("创建游戏失败!\n" + item);
                }
                IsDisposed = false;
                onError(exceptionList);
            }
            else
            {
#if DETAILED_DEBUG
                Debug.Log("操作成功!");
#endif
                onComplete();
            }
            runningCoroutines.Clear();
            runningThreads.Clear();
        }

        public void Dispose()
        {
            if (IsRunning())
                IsDisposed = true;
        }

        private void OnRunningError(Exception e)
        {
            Dispose();
            exceptionList.Add(e);
        }

        public bool IsRunning()
        {
            return runningCoroutines.Count !=0 || runningThreads.Count != 0;
        }

        private void Start(T item , Action<Exception> onError)
        {
            StartThreads(item, onError);
            StartCoroutines(item, onError);
        }


        private void StartCoroutines(T item, Action<Exception> onError)
        {
            foreach (var component in coroutines)
            {
                StartCoroutine(component, item, onError);
            }
        }

        protected void StartCoroutine(Coroutine coroutine, T item, Action<Exception> onError)
        {
            Action onComplete;
            runningCoroutines.Add(coroutine);

#if DETAILED_DEBUG
            string coroutineName = coroutine.GetType().FullName;
            Debug.Log(coroutineName + "在协程开始初始化;");
            onComplete = delegate
            {
                runningCoroutines.Remove(coroutine);
                Debug.Log(coroutineName + "在协程初始化完成;");
            };
#else
            onComplete = () => runningCoroutines.Remove(coroutine);
#endif
            Func<IEnumerator> coroutineMethod =() => GetCoroutine(coroutine, item, this, onError, onComplete);

            Observable.FromMicroCoroutine(coroutineMethod, false, UpdateType).Subscribe(null, onError, onComplete);
        }


        private void StartThreads(T item, Action<Exception> onError)
        {
            foreach (var thread in threads)
            {
                StartThread(thread, item, onError);
            }
        }

        protected void StartThread(Thread thread, T item, Action<Exception> onError)
        {
            Action onComplete;
            runningThreads.Add(thread);

#if DETAILED_DEBUG
            string threadName = thread.GetType().FullName;
            Debug.Log(threadName + "在线程开始初始化;");
            onComplete = delegate
            {
                runningThreads.Remove(thread);
                Debug.Log(threadName + "在线程初始化完成;");
            };
#else
            onComplete = () => runningThreads.Remove(thread);
#endif
            WaitCallback waitCallback = GetThreadThread(thread, item, this, onError, onComplete);

            ThreadPool.QueueUserWorkItem(waitCallback);
        }


        protected IEnumerator GetCoroutine(Coroutine coroutine, T item,
            ICancelable cancelable, Action<Exception> onError, Action onComplete)
        {
            return coroutine.Initialize(item, cancelable, onError, onComplete);
        }

        protected WaitCallback GetThreadThread(Thread thread, T item,
            ICancelable cancelable, Action<Exception> onError, Action onComplete)
        {
            return _ => thread.Initialize(item, cancelable, onError, onComplete);
        }


        public void Add(Coroutine item)
        {
            coroutines.Add(item);
        }
        public bool Remove(Coroutine item)
        {
            return coroutines.Remove(item);
        }
        public bool Contains(Coroutine item)
        {
            return coroutines.Contains(item);
        }

        public void Add(Thread item)
        {
            threads.Add(item);
        }
        public bool Remove(Thread item)
        {
            return threads.Remove(item);
        }
        public bool Contains(Thread item)
        {
            return threads.Contains(item);
        }

    }

}
