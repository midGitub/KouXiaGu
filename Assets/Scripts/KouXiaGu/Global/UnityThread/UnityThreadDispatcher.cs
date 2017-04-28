using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu
{

    public class UnityThreadBehaviour<T>
        where T :class
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

        public IEnumerable<UnityThreadBehaviour<Action>> UpdateObservers
        {
            get { return onUpdate.Observers; }
        }

        public int UpdateObserverCount
        {
            get { return onUpdate.ObserverCount; }
        }

        public IEnumerable<UnityThreadBehaviour<Action>> FixedUpdateObservers
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

        /// <summary>
        /// 订阅到 Update 更新,每次更新都会调用 OnNext(),若返回异常,则输出异常到日志;
        /// </summary>
        [Obsolete]
        public IDisposable SubscribeUpdate(IObserver<object> item)
        {
            return SubscribeUpdate(item, () => item.OnNext(this));
        }

        /// <summary>
        /// 订阅到 FixedUpdate 更新,只会更新观察者的 OnNext() 和 OnCompleted();
        /// </summary>
        [Obsolete]
        public IDisposable SubscribeFixedUpdate(IObserver<object> item)
        {
            return SubscribeFixedUpdate(item, () => item.OnNext(this));
        }


        public IDisposable SubscribeUpdate(object sender, Action action)
        {
            var behaviour = new UnityThreadBehaviour<Action>(sender, action);
            return SubscribeUpdate(behaviour);
        }

        public IDisposable SubscribeUpdate(UnityThreadBehaviour<Action> behaviour)
        {
            return onUpdate.Subscribe(behaviour);
        }


        public IDisposable SubscribeFixedUpdate(object sender, Action action)
        {
            var behaviour = new UnityThreadBehaviour<Action>(sender, action);
            return SubscribeFixedUpdate(behaviour);
        }

        public IDisposable SubscribeFixedUpdate(UnityThreadBehaviour<Action> behaviour)
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

        class ActionCollection : ActionCollectionBase<UnityThreadBehaviour<Action>>
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
