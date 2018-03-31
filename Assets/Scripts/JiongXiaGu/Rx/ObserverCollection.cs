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
        /// 枚举所有观察者;
        /// </summary>
        protected abstract IEnumerable<IObserver<T>> EnumerateObserver();

        /// <summary>
        /// 安全的枚举所有观察者,在枚举过程中允许进行取消订阅操作;
        /// </summary>
        protected virtual IEnumerable<IObserver<T>> EnumerateObserverSafe()
        {
            return EnumerateObserver().ToArray();
        }

        /// <summary>
        /// 返回重复加入观察者的异常;
        /// </summary>
        protected Exception RepeatedObserverException(IObserver<T> observer)
        {
            return new ArgumentException(string.Format("重复加入观察者[{0}];", observer));
        }

        /// <summary>
        /// 向观察者提供新数据;
        /// </summary>
        public void NotifyNext(T value)
        {
            foreach (var observer in EnumerateObserver())
            {
                observer.OnNext(value);
            }
        }

        /// <summary>
        /// 向观察者提供新数据,在执行中允许改变合集;
        /// </summary>
        public void NotifyNextSafe(T value)
        {
            foreach (var observer in EnumerateObserverSafe())
            {
                observer.OnNext(value);
            }
        }

        /// <summary>
        /// 通知观察者，提供程序遇到错误信息;
        /// </summary>
        public void NotifyError(Exception ex)
        {
            foreach (var observer in EnumerateObserver())
            {
                observer.OnError(ex);
            }
        }

        /// <summary>
        /// 通知观察者，提供程序遇到错误信息,在执行中允许改变合集;
        /// </summary>
        public void NotifyErrorSafe(Exception ex)
        {
            foreach (var observer in EnumerateObserverSafe())
            {
                observer.OnError(ex);
            }
        }

        /// <summary>
        /// 通知观察者，提供程序已完成发送基于推送的通知,在执行中允许改变合集;
        /// </summary>
        public void NotifyCompleted()
        {
            foreach (var observer in EnumerateObserverSafe())
            {
                observer.OnCompleted();
            }
        }
    }
}
