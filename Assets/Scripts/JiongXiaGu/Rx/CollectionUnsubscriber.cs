using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu
{

    /// <summary>
    /// 取消订阅器,不进行 Add() 操作;
    /// </summary>
    public sealed class CollectionUnsubscriber<T> : IDisposable
    {
        public CollectionUnsubscriber(ICollection<T> collection, T observer)
        {
            if (collection == null || observer == null)
                throw new ArgumentNullException();

            Collection = collection;
            Observer = observer;
        }

        ~CollectionUnsubscriber()
        {
            Dispose(false);
        }

        public ICollection<T> Collection { get; private set; }
        public T Observer { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
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
