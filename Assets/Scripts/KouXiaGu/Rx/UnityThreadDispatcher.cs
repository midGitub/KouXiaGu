using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Rx
{

    public interface IUnityThreadEvent
    {
        void OnNext();
        void OnError(Exception ex);
    }

    public abstract class UnityDispatcher : IUnityThreadEvent, IDisposable
    {
        static UnityThreadDispatcher instance
        {
            get { return UnityThreadDispatcher.Instance; }
        }

        IDisposable disposer;

        public abstract void OnNext();
        public abstract void OnError(Exception ex);

        public IDisposable SubscribeUpdate()
        {
            disposer = instance.SubscribeUpdate(this);
            return this;
        }

        public IDisposable SubscribeFixedUpdate()
        {
            disposer = instance.SubscribeFixedUpdate(this);
            return this;
        }

        public void Dispose()
        {
            disposer.Dispose();
        }
    }


    public class UnityThreadDelegate : UnityDispatcher
    {
        public UnityThreadDelegate(Action onNext, Action<Exception> onError)
        {
            if (onNext == null)
                throw new ArgumentNullException("action");

            this.onNext = onNext;
            this.onError = onError;
        }

        Action onNext;
        Action<Exception> onError;

        public override void OnNext()
        {
            onNext();
        }

        public override void OnError(Exception ex)
        {
            onError(ex);
        }
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
