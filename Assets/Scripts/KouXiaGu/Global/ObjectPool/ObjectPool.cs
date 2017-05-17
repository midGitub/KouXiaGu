using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    public abstract class ObjectPool<T> : IObjectPool<T>
    {
        const int defaultMaxCapacity = 60;

        public ObjectPool()
            : this(defaultMaxCapacity)
        {
        }

        public ObjectPool(int maxCapacity)
        {
            MaxCapacity = maxCapacity;
            objectQueue = new Queue<T>(maxCapacity);
        }

        readonly Queue<T> objectQueue;
        public int MaxCapacity { get; private set; }

        /// <summary>
        /// 在对象池中的对象;
        /// </summary>
        protected IEnumerable<T> PoolObjects
        {
            get { return objectQueue; }
        }

        public int Count
        {
            get { return objectQueue.Count; }
        }

        public bool IsFull
        {
            get { return Count >= MaxCapacity; }
        }

        public bool IsEmpty
        {
            get { return Count <= 0; }
        }

        /// <summary>
        /// 实例化对象;
        /// </summary>
        public abstract T Instantiate();

        /// <summary>
        /// 在对象出池时进行重置;
        /// </summary>
        public abstract void ResetWhenOutPool(T item);

        /// <summary>
        /// 在进入对象池时进行重置;
        /// </summary>
        public abstract void ResetWhenEnterPool(T item);

        /// <summary>
        /// 销毁对象;
        /// </summary>
        public abstract void Destroy(T item);


        /// <summary>
        /// 获取到对象实例;
        /// </summary>
        public T Get()
        {
            if (IsEmpty)
            {
                T item = Instantiate();
                return item;
            }
            else
            {
                T item = objectQueue.Dequeue();
                ResetWhenOutPool(item);
                return item;
            }
        }

        /// <summary>
        /// 将对象放入池中,并且重置其数据;
        /// </summary>
        public void Release(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (IsFull)
            {
                Destroy(item);
            }
            else
            {
                ResetWhenEnterPool(item);
                objectQueue.Enqueue(item);
            }
        }

        /// <summary>
        /// 设置到最大容量;
        /// </summary>
        public void SetMaxCapacity(int maxCapacity)
        {
            MaxCapacity = MaxCapacity;
            while (IsFull)
            {
                T item = objectQueue.Dequeue();
                Destroy(item);
            }
        }

        /// <summary>
        /// 销毁所有池内对象;
        /// </summary>
        public void DestroyAll()
        {
            foreach (var item in objectQueue)
            {
                Destroy(item);
            }
            objectQueue.Clear();
        }

    }

}
