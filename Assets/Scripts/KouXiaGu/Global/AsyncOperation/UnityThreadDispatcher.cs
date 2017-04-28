using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    public interface IUnityThreadBehaviour<T>
    {
        object Sender { get; }
        T Action { get; }
    }

    public abstract class UnityThreadBehaviour : IUnityThreadBehaviour<Action>
    {
        public UnityThreadBehaviour(object sender)
        {
            Sender = sender;
        }

        public object Sender { get; private set; }

        public Action Action
        {
            get { return OnNext; }
        }

        protected abstract void OnNext();

        public override string ToString()
        {
            return "[Sender:" + Sender.ToString() + "]";
        }
    }

    /// <summary>
    /// 在Unity线程进行的操作;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UnityThreadDispatcher : UnitySington<UnityThreadDispatcher>
    {
        UnityThreadDispatcher()
        {
        }

        ActionCollection onUpdate;
        ActionCollection onFixedUpdate;
        ActionCollection onLateUpdate;

        public IEnumerable<IUnityThreadBehaviour<Action>> UpdateObservers
        {
            get { return onUpdate.Observers; }
        }

        public int UpdateObserverCount
        {
            get { return onUpdate.ObserverCount; }
        }

        public IEnumerable<IUnityThreadBehaviour<Action>> FixedUpdateObservers
        {
            get { return onFixedUpdate.Observers; }
        }

        public int FixedUpdateObserverCount
        {
            get { return onFixedUpdate.ObserverCount; }
        }

        public IEnumerable<IUnityThreadBehaviour<Action>> LateUpdateObservers
        {
            get { return onLateUpdate.Observers; }
        }

        public int LateUpdateObserverCount
        {
            get { return onLateUpdate.ObserverCount; }
        }

        void Awake()
        {
            onUpdate = new ActionCollection();
            onFixedUpdate = new ActionCollection();
            onLateUpdate = new ActionCollection();
        }

        void Update()
        {
            onUpdate.Next();
        }

        void LateUpdate()
        {
            onLateUpdate.Next();
        }

        void FixedUpdate()
        {
            onFixedUpdate.Next();
        }

        public IDisposable SubscribeUpdate(IUnityThreadBehaviour<Action> behaviour)
        {
            return onUpdate.Subscribe(behaviour);
        }

        public IDisposable SubscribeLateUpdate(IUnityThreadBehaviour<Action> behaviour)
        {
            return onLateUpdate.Subscribe(behaviour);
        }

        public IDisposable SubscribeFixedUpdate(IUnityThreadBehaviour<Action> behaviour)
        {
            return onFixedUpdate.Subscribe(behaviour);
        }


        abstract class ActionCollectionBase<T> : ObserverCollection<T>
        {
            protected void OnError(T item, Exception ex)
            {
                Debug.LogError("UnityThreadDispatcher:" + item.ToString() + ex);
            }
        }

        class ActionCollection : ActionCollectionBase<IUnityThreadBehaviour<Action>>
        {
            public void Next()
            {
                foreach (var item in EnumerateObserver())
                {
                    try
                    {
                        item.Action();
                    }
                    catch (Exception ex)
                    {
                        OnError(item, ex);
                    }
                }
            }
        }
    }


    public static class UnityThreadBehaviourExtensions
    {
        static UnityThreadDispatcher unityThreadDispatcher
        {
            get { return UnityThreadDispatcher.Instance; }
        }

        public static IDisposable SubscribeUpdate(this IUnityThreadBehaviour<Action> behaviour)
        {
            return unityThreadDispatcher.SubscribeUpdate(behaviour);
        }

        public static IDisposable SubscribeFixedUpdate(this IUnityThreadBehaviour<Action> behaviour)
        {
            return unityThreadDispatcher.SubscribeFixedUpdate(behaviour);
        }
    }

}
