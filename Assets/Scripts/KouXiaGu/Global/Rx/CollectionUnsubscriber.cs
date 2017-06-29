using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 取消订阅器,不进行 Add() 操作;;
    /// </summary>
    class CollectionUnsubscriber<T> : IDisposable
    {
        /// <summary>
        /// 不进行 Add() 操作;
        /// </summary>
        public CollectionUnsubscriber(ICollection<T> collection, T observer)
        {
            if (collection == null || observer == null)
                throw new ArgumentNullException();

            Collection = collection;
            Observer = observer;
        }

        public ICollection<T> Collection { get; private set; }
        public T Observer { get; private set; }

        public void Dispose()
        {
            if (Collection != null)
            {
                Collection.Remove(Observer);
                Collection = null;
                Observer = default(T);
            }
        }

    }
}
