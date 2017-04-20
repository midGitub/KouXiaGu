using System;
using System.Collections.Generic;

namespace KouXiaGu
{

    public abstract class ObjectPool<T>
    {
        const int defaultMaxCapacity = 120;

        public ObjectPool()
            : this(defaultMaxCapacity)
        {
        }

        public ObjectPool(int maxCapacity)
        {
            MaxCapacity = maxCapacity;
            objectQueue = new Queue<T>(maxCapacity);
        }

        Queue<T> objectQueue;
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
        /// 重置对象;
        /// </summary>
        public abstract void Reset(T item);

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
                Reset(item);
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
        public void Clear()
        {
            foreach (var item in objectQueue)
            {
                Destroy(item);
            }
            objectQueue.Clear();
        }

    }

}
