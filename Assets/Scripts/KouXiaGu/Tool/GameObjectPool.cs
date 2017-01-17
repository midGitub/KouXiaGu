using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 对象池;
    /// </summary>
    public class GameObjectPool<T>
        where T : MonoBehaviour, IReusable
    {

        const string DEFAULT_NAME = "GameObject";

        public GameObjectPool()
        {
            objectPool = new Stack<T>();
            Capacity = int.MaxValue;
        }

        public GameObjectPool(int capacity)
        {
            objectPool = new Stack<T>(capacity);
            this.Capacity = capacity;
        }

        /// <summary>
        /// 对象存放结构;
        /// </summary>
        Stack<T> objectPool;

        /// <summary>
        /// 最大容量;
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// 池内的对象数;
        /// </summary>
        public int Count
        {
            get { return objectPool.Count; }
        }

        public bool IsFull
        {
            get { return Count >= Capacity; }
        }

        public bool IsEmpty
        {
            get { return Count <= 0; }
        }

        /// <summary>
        /// 获取到一个池对象;
        /// </summary>
        public T Get()
        {
            if (IsEmpty)
            {
                return Create();
            }
            else
            {
                return objectPool.Pop();
            }
        }

        /// <summary>
        /// 将对象放入池中;
        /// </summary>
        public void Release(T item)
        {
            if (IsFull)
            {
                Destroy(item);
            }
            else
            {
                item.Reset();
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
