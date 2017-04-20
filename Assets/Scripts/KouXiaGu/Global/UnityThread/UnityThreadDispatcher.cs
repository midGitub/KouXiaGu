using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using UniRx;

namespace KouXiaGu
{

    public static class UnityThread
    {
        static UnityThreadDispatcher dispatcher
        {
            get { return UnityThreadDispatcher.Instance; }
        }

        //public static IObservable<object> 

    }

    /// <summary>
    /// 在Unity线程进行的操作;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UnityThreadDispatcher : MonoBehaviour
    {
        static UnityThreadDispatcher instance;

        public static UnityThreadDispatcher Instance
        {
            get { return instance ?? Initialize(); }
        }

        public static bool IsInitialized
        {
            get { return instance != null; }
        }

        static UnityThreadDispatcher Initialize()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UnityThreadDispatcher>();
                if (instance == null)
                {
                    instance = new GameObject(typeof(UnityThreadDispatcher).Name, typeof(UnityThreadDispatcher)).
                        GetComponent<UnityThreadDispatcher>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }


        LinkedListTracker<object> onUpdateTracker;
        LinkedListTracker<object> onFixedUpdateTracker;

        public int UpdateObserverCount
        {
            get { return onUpdateTracker.ObserverCount; }
        }

        public int FixedUpdateObserverCount
        {
            get { return onFixedUpdateTracker.ObserverCount; }
        }

        void Awake()
        {
            onUpdateTracker = new LinkedListTracker<object>();
            onFixedUpdateTracker = new LinkedListTracker<object>();
        }

        void Update()
        {
            onUpdateTracker.Track(this);
        }

        void FixedUpdate()
        {
            onFixedUpdateTracker.Track(this);
        }

        void OnDestroy()
        {
            onUpdateTracker.TrackCompleted();
            onFixedUpdateTracker.TrackCompleted();
        }

        /// <summary>
        /// 订阅到 Update 更新,只会更新观察者的 OnNext() 和 OnCompleted();
        /// </summary>
        public IDisposable SubscribeUpdate(IObserver<object> item)
        {
            return onUpdateTracker.Subscribe(item);
        }

        /// <summary>
        /// 订阅到 FixedUpdate 更新,只会更新观察者的 OnNext() 和 OnCompleted();
        /// </summary>
        public IDisposable SubscribeFixedUpdate(IObserver<object> item)
        {
            return onFixedUpdateTracker.Subscribe(item);
        }

    }

}
