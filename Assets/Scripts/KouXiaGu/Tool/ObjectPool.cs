using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    [Serializable]
    public abstract class ObjectPool<T>
    {

        const int DEFAULT_CAPACITY = 120;


        protected ObjectPool()
        {
            IsInitialized = false;
        }

        public ObjectPool(int capacity)
        {
            objectPool = new Queue<T>(capacity);
            IsInitialized = true;
        }

        public void Initialize()
        {
            if (!IsInitialized)
            {
                objectPool = new Queue<T>(capacity);
                IsInitialized = true;
            }
        }


        /// <summary>
        /// 最大容量;
        /// </summary>
        [SerializeField, Range(5, DEFAULT_CAPACITY * 2)]
        int capacity = DEFAULT_CAPACITY;

        Queue<T> objectPool;

        /// <summary>
        /// 是否已经初始化?
        /// </summary>
        public bool IsInitialized { get; private set; }


        /// <summary>
        /// 最大容量;
        /// </summary>
        public int Capacity
        {
            get { return capacity; }
            private set { capacity = value; }
        }

        /// <summary>
        /// 池内的对象数;
        /// </summary>
        public int Count
        {
            get { return objectPool == null ? 0 : objectPool.Count; }
        }

        public bool IsFull
        {
            get { return objectPool == null ? false : Count >= Capacity; }
        }

        public bool IsEmpty
        {
            get { return objectPool == null ? true : Count <= 0; }
        }


        public T Get()
        {
            if (IsEmpty)
            {
                T item = Instantiate();
                return item;
            }
            else
            {
                T item = objectPool.Dequeue();
                return item;
            }
        }

        /// <summary>
        /// 实例化一个物体;
        /// </summary>
        protected abstract T Instantiate();


        /// <summary>
        /// 将对象放入池中,并且重置其数据;
        /// </summary>
        public void Release(T item)
        {
            if (IsFull)
            {
                Destroy(item);
            }
            else
            {
                Reset(item);
                objectPool.Enqueue(item);
            }
        }

        /// <summary>
        /// 重置实例内容;
        /// </summary>
        protected abstract void Reset(T item);

        /// <summary>
        /// 销毁实例对象;
        /// </summary>
        protected abstract void Destroy(T item);

        /// <summary>
        /// 销毁所有池内对象;
        /// </summary>
        public void Clear()
        {
            if (objectPool == null)
                return;

            foreach (var item in objectPool)
            {
                Destroy(item);
            }
            objectPool.Clear();
        }

    }

}
