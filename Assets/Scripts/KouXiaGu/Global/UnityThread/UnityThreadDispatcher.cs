using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using UniRx;

namespace KouXiaGu
{

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

        LinkedListTracker<object> onUpdateTracker;
        LinkedListTracker<object> onFixedUpdateTracker;

        public IEnumerable<Action> UpdateObservers
        {
            get { return onUpdate.Observers; }
        }

        public int UpdateObserverCount
        {
            get { return onUpdate.ObserverCount; }
        }

        public IEnumerable<Action> FixedUpdateObservers
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

            onUpdateTracker = new LinkedListTracker<object>();
            onFixedUpdateTracker = new LinkedListTracker<object>();
        }

        void Update()
        {
            onUpdate.Next();
            //onUpdateTracker.Track(this);
        }

        void FixedUpdate()
        {
            onFixedUpdate.Next();
            //onFixedUpdateTracker.Track(this);
        }

        /// <summary>
        /// 订阅到 Update 更新,每次更新都会调用 OnNext(),若返回异常,则输出异常到日志;
        /// </summary>
        public IDisposable SubscribeUpdate(IObserver<object> item)
        {
            return onUpdate.Subscribe(() => item.OnNext(this));
            //return onUpdateTracker.Subscribe(item);
        }

        /// <summary>
        /// 订阅到 FixedUpdate 更新,只会更新观察者的 OnNext() 和 OnCompleted();
        /// </summary>
        public IDisposable SubscribeFixedUpdate(IObserver<object> item)
        {
            return onFixedUpdate.Subscribe(() => item.OnNext(this));
            //return onFixedUpdateTracker.Subscribe(item);
        }

        abstract class ActionCollectionBase<T> : ObserverCollection<T>
        {
            protected void OnError(T item, Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        class ActionCollection : ActionCollectionBase<Action>
        {
            public void Next()
            {
                foreach (var item in EnumerateObserver())
                {
                    try
                    {
                        item();
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
