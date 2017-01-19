using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{


    public class GameObjectPool<T>
        where T : Component
    {

        const string DEFAULT_NAME = "GameObject";
        const int DEFAULT_CAPACITY = 120;

        static Transform parent;

        protected Transform Parent
        {
            get { return parent ?? (parent = new GameObject("PoolObjects").transform); }
        }

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
                T item = Create();
                return item;
            }
            else
            {
                T item = objectPool.Pop();
                GetAction(item);
                return item;
            }
        }

        protected virtual void GetAction(T item)
        {
            item.gameObject.SetActive(true);
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
                ReleaseAction(item);
                objectPool.Push(item);
            }
        }

        protected virtual void ReleaseAction(T item)
        {
            item.gameObject.SetActive(false);
        }



        /// <summary>
        /// 创建一个实例;
        /// </summary>
        protected virtual T Create()
        {
            GameObject gameObject = new GameObject(DEFAULT_NAME, typeof(T));
            gameObject.transform.SetParent(Parent);
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
            if (item != null)
            {
                GameObject.Destroy(item.gameObject);
            }
        }

    }

}
