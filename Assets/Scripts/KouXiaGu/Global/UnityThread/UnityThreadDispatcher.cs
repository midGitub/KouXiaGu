using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu
{

    public interface IUnityThreadBehaviour<T>
    {
        object Sender { get; }
        T Action { get; }
    }

    public class UnityThreadBehaviour<T> : IUnityThreadBehaviour<T>
    {
        public UnityThreadBehaviour(object sender, T action)
        {
            Sender = sender;
            Action = action;
        }

        public object Sender { get; private set; }
        public T Action { get; private set; }

        public override string ToString()
        {
            return "[Sender:" + Sender.ToString() + ",Action:" + Action.ToString() + "]";
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

        void Awake()
        {
            onUpdate = new ActionCollection();
            onFixedUpdate = new ActionCollection();
        }

        void Update()
        {
            onUpdate.Next();
        }

        void FixedUpdate()
        {
            onFixedUpdate.Next();
        }

        public IDisposable SubscribeUpdate(IUnityThreadBehaviour<Action> behaviour)
        {
            return onUpdate.Subscribe(behaviour);
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

}
