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
                    instance = new GameObject("UnityThreadDispatcher", typeof(UnityThreadDispatcher)).
                        GetComponent<UnityThreadDispatcher>();
                }
            }
            return instance;
        }


        Deliverer onUpdateTracker;
        Deliverer onFixedUpdateTracker;

        public int UpdateObserverCount
        {
            get { return onUpdateTracker.observerCount; }
        }

        public int FixedUpdateObserverCount
        {
            get { return onFixedUpdateTracker.observerCount; }
        }

        void Awake()
        {
            onUpdateTracker = new Deliverer();
            onFixedUpdateTracker = new Deliverer();
        }

        void Update()
        {
            onUpdateTracker.Track();
        }

        void FixedUpdate()
        {
            onFixedUpdateTracker.Track();
        }

        internal IDisposable SubscribeUpdate(IUnityThreadEvent item)
        {
            return onUpdateTracker.Subscribe(item);
        }

        internal IDisposable SubscribeFixedUpdate(IUnityThreadEvent item)
        {
            return onFixedUpdateTracker.Subscribe(item);
        }

        class Deliverer : Deliverer<IUnityThreadEvent>
        {
            protected override void Operate(IUnityThreadEvent observer)
            {
                try
                {
                    observer.OnNext();
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            }
        }

    }

}
