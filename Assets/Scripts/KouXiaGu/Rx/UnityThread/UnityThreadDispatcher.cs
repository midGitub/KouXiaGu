using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Rx
{

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


        LinkedListTracker<UnityThreadDispatcher> onUpdateTracker;
        LinkedListTracker<UnityThreadDispatcher> onFixedUpdateTracker;

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
            onUpdateTracker = new LinkedListTracker<UnityThreadDispatcher>();
            onFixedUpdateTracker = new LinkedListTracker<UnityThreadDispatcher>();
        }

        void Update()
        {
            onUpdateTracker.Track(this);
        }

        void FixedUpdate()
        {
            onFixedUpdateTracker.Track(this);
        }

        /// <summary>
        /// 订阅到 Update 更新,只会更新观察者的 OnNext();
        /// </summary>
        public IDisposable SubscribeUpdate(IXiaGuObserver<UnityThreadDispatcher> item)
        {
            return onUpdateTracker.Subscribe(item);
        }

        /// <summary>
        /// 订阅到 FixedUpdate 更新,只会更新观察者的 OnNext();
        /// </summary>
        public IDisposable SubscribeFixedUpdate(IXiaGuObserver<UnityThreadDispatcher> item)
        {
            return onFixedUpdateTracker.Subscribe(item);
        }

    }

}
