using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 对象池;
    /// </summary>
    [Serializable]
    public class ReusableObjectPool<T>
        where T : MonoBehaviour, IReusable
    {

        const string DEFAULT_NAME = "GameObject";
        const int DEFAULT_CAPACITY = 500;

        /// <summary>
        /// 对象存放结构;
        /// </summary>
        Stack<T> objectPool;

        /// <summary>
        /// 最大容量;
        /// </summary>
        [SerializeField, Range(5, DEFAULT_CAPACITY * 2)]
        int capacity = DEFAULT_CAPACITY;

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


        void InitObjectPool()
        {
            if (objectPool == null)
                objectPool = new Stack<T>(capacity);
        }

        /// <summary>
        /// 获取到一个池对象;
        /// </summary>
        public T Get()
        {
            InitObjectPool();

            if (IsEmpty)
            {
                return Create();
            }
            else
            {
                T item = objectPool.Pop();
                item.gameObject.SetActive(true);
                return item;
            }
        }

        /// <summary>
        /// 将对象放入池中;
        /// </summary>
        public void Release(T item)
        {
            InitObjectPool();

            if (IsFull)
            {
                Destroy(item);
            }
            else
            {
                item.Reset();
                item.gameObject.SetActive(false);
                objectPool.Push(item);
            }
        }

        /// <summary>
        /// 创建一个实例;
        /// </summary>
        protected virtual T Create()
        {
            GameObject gameObject = new GameObject(DEFAULT_NAME, typeof(T));
            T item = gameObject.GetComponent<T>();

            if (item == default(T))
            {
                GameObject.Destroy(gameObject);
                throw new ObjectDisposedException("对象创建失败;");
            }

            return item;
        }

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

        /// <summary>
        /// 销毁物体;
        /// </summary>
        protected virtual void Destroy(T item)
        {
            GameObject.Destroy(item.gameObject);
        }

    }

}
