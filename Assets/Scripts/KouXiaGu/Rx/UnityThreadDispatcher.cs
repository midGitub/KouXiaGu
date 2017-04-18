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
        public static UnityThreadDispatcher Instance { get; private set; }

        public static bool IsInitialized
        {
            get { return Instance != null; }
        }

        LinkedListTracker onUpdateTracker;
        LinkedListTracker onFixedUpdateTracker;

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
            Instance = this;
            onUpdateTracker = new LinkedListTracker();
            onFixedUpdateTracker = new LinkedListTracker();
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


        class LinkedListTracker
        {
            public LinkedListTracker()
            {
                currentNode = null;
                observers = new LinkedList<IUnityThreadEvent>();
            }

            LinkedList<IUnityThreadEvent> observers;
            LinkedListNode<IUnityThreadEvent> currentNode;

            public int observerCount
            {
                get { return observers.Count; }
            }

            public IDisposable Subscribe(IUnityThreadEvent observer)
            {
                var node = observers.AddLast(observer);
                return new Unsubscriber(this, node);
            }

            public void Track()
            {
                currentNode = observers.First;

                while (currentNode != null)
                {
                    var observer = currentNode.Value;
                    currentNode = currentNode.Next;
                    Next(observer);
                }
            }

            void Next(IUnityThreadEvent observer)
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

            class Unsubscriber : IDisposable
            {
                public Unsubscriber(LinkedListTracker tracker, LinkedListNode<IUnityThreadEvent> node)
                {
                    isUnsubscribed = false;
                    this.tracker = tracker;
                    this.node = node;
                }

                bool isUnsubscribed;
                LinkedListTracker tracker;
                LinkedListNode<IUnityThreadEvent> node;

                LinkedList<IUnityThreadEvent> observers
                {
                    get { return tracker.observers; }
                }

                LinkedListNode<IUnityThreadEvent> currentNode
                {
                    get { return tracker.currentNode; }
                    set { tracker.currentNode = value; }
                }

                public void Dispose()
                {
                    if (!isUnsubscribed)
                    {
                        if (node == currentNode)
                        {
                            currentNode = currentNode.Next;
                        }

                        observers.Remove(node);
                        isUnsubscribed = true;
                    }
                }
            }
        }

    }

}
