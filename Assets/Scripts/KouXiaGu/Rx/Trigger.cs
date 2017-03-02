using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Rx
{

    /// <summary>
    /// 观察者结构;
    /// </summary>
    public class Trigger<T> : IObservable<T>
    {

        public Trigger()
        {
            observersHashSet = new HashSet<IObserver<T>>();
        }

        /// <summary>
        /// 存放所有观察者;
        /// </summary>
        HashSet<IObserver<T>> observersHashSet;


        public int ObserverCount
        {
            get { return observersHashSet.Count; }
        }


        /// <summary>
        /// 当时间发生变化时回调;
        /// </summary>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!observersHashSet.Add(observer))
                throw new ArgumentException("重复加入;");

            var unsubscriber = new Unsubscriber(observersHashSet, observer);
            return unsubscriber;
        }

        /// <summary>
        /// 可以移除观察者的迭代;
        /// </summary>
        IEnumerable<IObserver<T>> EnumerateObservers()
        {
            var observersTempArray = observersHashSet.ToArray();

            foreach (var observer in observersTempArray)
            {
                if (observersHashSet.Contains(observer))
                    yield return observer;
            }
        }

        /// <summary>
        /// 推送信息到所有观察者;
        /// </summary>
        public void Triggering(T item)
        {
            foreach (var observer in EnumerateObservers())
            {
                observer.OnNext(item);
            }
        }

        /// <summary>
        /// 清空所有观察者;
        /// </summary>
        public void EndTrigger()
        {
            foreach (var observer in EnumerateObservers())
            {
                observer.OnCompleted();
            }
            observersHashSet.Clear();
        }

        /// <summary>
        /// 取消订阅器;
        /// </summary>
        class Unsubscriber : IDisposable
        {

            public Unsubscriber(ICollection<IObserver<T>> collection, IObserver<T> observer)
            {
                this.collection = collection;
                this.observer = observer;
            }

            ICollection<IObserver<T>> collection;
            IObserver<T> observer;

            public void Dispose()
            {
                if (observer != null)
                {
                    collection.Remove(observer);
                    collection = null;
                    observer = null;
                }
            }

            public override bool Equals(object obj)
            {
                var item = obj as Unsubscriber;

                if (item == null)
                    return false;

                return observer != null && item.observer == observer;
            }

            public override int GetHashCode()
            {
                return observer.GetHashCode();
            }

        }

    }

}
