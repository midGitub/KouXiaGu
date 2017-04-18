using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Rx;
using UnityEngine;

namespace KouXiaGu
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
                    DontDestroyOnLoad(instance.gameObject);
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

        internal IDisposable SubscribeUpdate(UnityThreadEvent item)
        {
            return onUpdateTracker.Subscribe(item);
        }

        internal IDisposable SubscribeFixedUpdate(UnityThreadEvent item)
        {
            return onFixedUpdateTracker.Subscribe(item);
        }

        class Deliverer : Deliverer<UnityThreadEvent>
        {
            protected override void Operate(UnityThreadEvent observer)
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
