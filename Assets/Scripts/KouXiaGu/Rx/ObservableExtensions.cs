using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Rx
{

    public static class RxExtensions
    {

        public static IObservable<T> Where<T>(this IObservable<T> observable, Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }


        public static IDisposable SubscribUpdate(this IObserver<object> observer)
        {
            return UnityThreadDispatcher.Instance.SubscribeUpdate(observer);
        }

        public static IDisposable SubscribeFixedUpdate(this IObserver<object> observer)
        {
            return UnityThreadDispatcher.Instance.SubscribeFixedUpdate(observer);
        }

    }

}
