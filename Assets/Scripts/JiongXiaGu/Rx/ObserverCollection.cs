using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JiongXiaGu
{

    /// <summary>
    /// 不允许重复添加的观察者合集;
    /// </summary>
    public abstract class ObserverCollection<T> : IObservable<T>
    {
        public abstract IDisposable Subscribe(IObserver<T> observer);

        /// <summary>
        /// 安全的枚举所有观察者,在枚举过程中允许更改合集;
        /// </summary>
        protected abstract IEnumerable<IObserver<T>> EnumerateObserverSafe();

        /// <summary>
        /// 返回重复加入观察者的异常;
        /// </summary>
        protected Exception RepeatedObserverException(IObserver<T> observer)
        {
            return new ArgumentException(string.Format("重复加入观察者[{0}];", observer));
        }

        /// <summary>
        /// 向观察者提供新数据;(通知过程中若返回任何异常都不会进行返回)
        /// </summary>
        public void NotifyNext(T value)
        {
            foreach (var observer in EnumerateObserverSafe())
            {
                try
                {
                    observer.OnNext(value);
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 向观察者提供新数据;(若有观察者返回异常,则调用 onError,并继续通知下一个观察者)
        /// </summary>
        public void NotifyNext(T value, Action<IObserver<T>, Exception> onError)
        {
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            foreach (var observer in EnumerateObserverSafe())
            {
                try
                {
                    observer.OnNext(value);
                }
                catch (Exception ex)
                {
                    onError.Invoke(observer, ex);
                    continue;
                }
            }
        }

        /// <summary>
        /// 通知观察者，提供程序遇到错误情况;(通知过程中若返回任何异常都不会进行返回)
        /// </summary>
        public void NotifyError(Exception ex)
        {
            foreach (var observer in EnumerateObserverSafe())
            {
                try
                {
                    observer.OnError(ex);
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 通知观察者，提供程序遇到错误情况;(若有观察者返回异常,则调用 onError,并继续通知下一个观察者)
        /// </summary>
        public void NotifyError(Exception ex, Action<IObserver<T>, Exception> onError)
        {
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            foreach (var observer in EnumerateObserverSafe())
            {
                try
                {
                    observer.OnError(ex);
                }
                catch (Exception observerEx)
                {
                    onError.Invoke(observer, observerEx);
                    continue;
                }
            }
        }

        /// <summary>
        /// 通知观察者，提供程序已完成发送基于推送的通知;(通知过程中若返回任何异常都不会进行返回)
        /// </summary>
        public void NotifyCompleted()
        {
            foreach (var observer in EnumerateObserverSafe())
            {
                try
                {
                    observer.OnCompleted();
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 通知观察者，提供程序已完成发送基于推送的通知;(若有观察者返回异常,则调用 onError,并继续通知下一个观察者)
        /// </summary>
        public void NotifyCompleted(Action<IObserver<T>, Exception> onError)
        {
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            foreach (var observer in EnumerateObserverSafe())
            {
                try
                {
                    observer.OnCompleted();
                }
                catch (Exception ex)
                {
                    onError.Invoke(observer, ex);
                    continue;
                }
            }
        }
    }

    /// <summary>
    /// 观察者合集;加入O(n),移除O(n), 迭代O(n);
    /// </summary>
    public class ObserverList<T> : ObserverCollection<T>
    {
        private List<IObserver<T>> observers;
        public IEqualityComparer<IObserver<T>> Comparer { get; private set; }

        public ObserverList()
        {
            observers = new List<IObserver<T>>();
            Comparer = EqualityComparer<IObserver<T>>.Default;
        }

        public override IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (observers.Contains(observer))
                throw RepeatedObserverException(observer);

            observers.Add(observer);
            var unsubscriber = new Unsubscriber(observers, observer);
            return unsubscriber;
        }

        protected override IEnumerable<IObserver<T>> EnumerateObserverSafe()
        {
            return observers.ToArray();
        }

        /// <summary>
        /// 提供移除观察者方法;
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            private bool isDisposed;
            private ICollection<IObserver<T>> observersCollection;
            private IObserver<T> observer;

            public Unsubscriber(ICollection<IObserver<T>> observersCollection, IObserver<T> observer)
            {
                isDisposed = false;
                this.observersCollection = observersCollection;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (!isDisposed)
                {
                    observersCollection.Remove(observer);

                    observersCollection = default(List<IObserver<T>>);
                    observer = default(IObserver<T>);
                    isDisposed = true;
                }
            }
        }
    }

    /// <summary>
    /// 使用双向链表存储观察者,添加O(n), 通过IDisposable接口移除O(1),Remove方法O(n), 迭代O(n);
    /// </summary>
    public class ObserverLinkedList<T> : ObserverCollection<T>
    {
        private LinkedList<IObserver<T>> observers;
        public IEqualityComparer<IObserver<T>> Comparer { get; private set; }

        public ObserverLinkedList()
        {
            observers = new LinkedList<IObserver<T>>();
            Comparer = EqualityComparer<IObserver<T>>.Default;
        }

        public override IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (observers.Contains(observer, Comparer))
                throw RepeatedObserverException(observer);

            var node = observers.AddLast(observer);
            var unsubscriber = new LinkedListUnsubscriber(observers, node);
            return unsubscriber;
        }

        protected override IEnumerable<IObserver<T>> EnumerateObserverSafe()
        {
            return observers.ToArray();
        }

        private class LinkedListUnsubscriber : IDisposable
        {
            private bool isDisposed = false;
            private LinkedList<IObserver<T>> collection;
            private LinkedListNode<IObserver<T>> node;

            public LinkedListUnsubscriber(LinkedList<IObserver<T>> collection, LinkedListNode<IObserver<T>> node)
            {
                this.collection = collection;
                this.node = node;
            }

            public void Dispose()
            {
                if (!isDisposed)
                {
                    collection.Remove(node);

                    collection = default(LinkedList<IObserver<T>>);
                    node = default(LinkedListNode<IObserver<T>>);
                    isDisposed = true;
                }
            }
        }
    }




    ///// <summary>
    ///// 抽象类 不允许重复添加的观察者合集;
    ///// </summary>
    //public class ObserverCollection<T> : ICollection<IObserver<T>>, IObservable<T>
    //{
    //    protected ICollection<IObserver<T>> Observers { get; private set; }

    //    /// <summary>
    //    /// 观察者总数;
    //    /// </summary>
    //    public int Count => Observers.Count;

    //    /// <summary>
    //    /// 总是返回false;
    //    /// </summary>
    //    bool ICollection<IObserver<T>>.IsReadOnly => false;

    //    /// <summary>
    //    /// 提供子类的构造函数;
    //    /// </summary>
    //    protected ObserverCollection(ICollection<IObserver<T>> observers)
    //    {
    //        Observers = observers;
    //    }

    //    /// <summary>
    //    /// 观察者订阅;
    //    /// </summary>
    //    public IDisposable Subscribe(IObserver<T> observer)
    //    {
    //        return Add(observer);
    //    }

    //    /// <summary>
    //    /// 添加观察者;
    //    /// </summary>
    //    public virtual IDisposable Add(IObserver<T> observer)
    //    {
    //        if (observer == null)
    //            throw new ArgumentNullException(nameof(observer));
    //        if (Observers.Contains(observer))
    //            throw RepeatedObserverException(observer);

    //        Observers.Add(observer);
    //        var unsubscriber = new Unsubscriber(Observers, observer);
    //        return unsubscriber;
    //    }

    //    /// <summary>
    //    /// 返回重复加入观察者的异常;
    //    /// </summary>
    //    protected Exception RepeatedObserverException(IObserver<T> observer)
    //    {
    //        return new ArgumentException(string.Format("重复加入观察者[{0}];", observer));
    //    }

    //    void ICollection<IObserver<T>>.Add(IObserver<T> observer)
    //    {
    //        Add(observer);
    //    }

    //    /// <summary>
    //    /// 移除指定观察者,若移除成功则返回true;此方法不会调用观察者的OnCompleted(),
    //    /// </summary>
    //    public bool Remove(IObserver<T> observer)
    //    {
    //        if (observer == null)
    //            throw new ArgumentNullException(nameof(observer));

    //        return Observers.Remove(observer);
    //    }

    //    bool ICollection<IObserver<T>>.Remove(IObserver<T> observer)
    //    {
    //        return Remove(observer);
    //    }

    //    /// <summary>
    //    /// 确认是否存在此观察者;
    //    /// </summary>
    //    public bool Contains(IObserver<T> observer)
    //    {
    //        if (observer == null)
    //            throw new ArgumentNullException(nameof(observer));

    //        return Observers.Contains(observer);
    //    }

    //    bool ICollection<IObserver<T>>.Contains(IObserver<T> observer)
    //    {
    //        return Contains(observer);
    //    }

    //    /// <summary>
    //    /// 清空合集,不会调用 NotifyCompleted();
    //    /// </summary>
    //    public void Clear()
    //    {
    //        Observers.Clear();
    //    }

    //    void ICollection<IObserver<T>>.Clear()
    //    {
    //        Clear();
    //    }

    //    /// <summary>
    //    /// 向观察者提供新数据;(通知过程中若返回任何异常都不会进行返回)
    //    /// </summary>
    //    public void NotifyNext(T value)
    //    {
    //        foreach (var observer in EnumerateObserver())
    //        {
    //            try
    //            {
    //                observer.OnNext(value);
    //            }
    //            catch
    //            {
    //                continue;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 向观察者提供新数据;(若有观察者返回异常,则调用 onError,并继续通知下一个观察者)
    //    /// </summary>
    //    public void NotifyNext(T value, Action<IObserver<T>, Exception> onError)
    //    {
    //        if (onError == null)
    //            throw new ArgumentNullException(nameof(onError));

    //        foreach (var observer in EnumerateObserver())
    //        {
    //            try
    //            {
    //                observer.OnNext(value);
    //            }
    //            catch(Exception ex)
    //            {
    //                onError.Invoke(observer, ex);
    //                continue;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 通知观察者，提供程序遇到错误情况;(通知过程中若返回任何异常都不会进行返回)
    //    /// </summary>
    //    public void NotifyError(Exception ex)
    //    {
    //        foreach (var observer in EnumerateObserver())
    //        {
    //            try
    //            {
    //                observer.OnError(ex);
    //            }
    //            catch
    //            {
    //                continue;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 通知观察者，提供程序遇到错误情况;(若有观察者返回异常,则调用 onError,并继续通知下一个观察者)
    //    /// </summary>
    //    public void NotifyError(Exception ex, Action<IObserver<T>, Exception> onError)
    //    {
    //        if (onError == null)
    //            throw new ArgumentNullException(nameof(onError));

    //        foreach (var observer in EnumerateObserver())
    //        {
    //            try
    //            {
    //                observer.OnError(ex);
    //            }
    //            catch(Exception observerEx)
    //            {
    //                onError.Invoke(observer, observerEx);
    //                continue;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 通知观察者，提供程序已完成发送基于推送的通知;(通知过程中若返回任何异常都不会进行返回)
    //    /// </summary>
    //    public void NotifyCompleted()
    //    {
    //        foreach (var observer in EnumerateObserver())
    //        {
    //            try
    //            {
    //                observer.OnCompleted();
    //            }
    //            catch
    //            {
    //                continue;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 通知观察者，提供程序已完成发送基于推送的通知;(若有观察者返回异常,则调用 onError,并继续通知下一个观察者)
    //    /// </summary>
    //    public void NotifyCompleted(Action<IObserver<T>, Exception> onError)
    //    {
    //        if (onError == null)
    //            throw new ArgumentNullException(nameof(onError));

    //        foreach (var observer in EnumerateObserver())
    //        {
    //            try
    //            {
    //                observer.OnCompleted();
    //            }
    //            catch (Exception ex)
    //            {
    //                onError.Invoke(observer, ex);
    //                continue;
    //            }
    //        }
    //    }

    //    void ICollection<IObserver<T>>.CopyTo(IObserver<T>[] array, int arrayIndex)
    //    {
    //        Observers.CopyTo(array, arrayIndex);
    //    }

    //    /// <summary>
    //    /// 安全的枚举所有观察者;
    //    /// </summary>
    //    protected virtual IEnumerable<IObserver<T>> EnumerateObserver()
    //    {
    //        return Observers.ToList();
    //    }

    //    public IEnumerator<IObserver<T>> GetEnumerator()
    //    {
    //        return Observers.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }

    //    /// <summary>
    //    /// 提供移除观察者方法;
    //    /// </summary>
    //    private class Unsubscriber : IDisposable
    //    {
    //        private bool isDisposed;
    //        private ICollection<IObserver<T>> observersCollection;
    //        private IObserver<T> observer;

    //        public Unsubscriber(ICollection<IObserver<T>> observersCollection, IObserver<T> observer)
    //        {
    //            isDisposed = false;
    //            this.observersCollection = observersCollection;
    //            this.observer = observer;
    //        }

    //        public void Dispose()
    //        {
    //            if (!isDisposed)
    //            {
    //                observersCollection.Remove(observer);

    //                observersCollection = default(List<IObserver<T>>);
    //                observer = default(IObserver<T>);
    //                isDisposed = true;
    //            }
    //        }
    //    }
    //}
}
