using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 线程安全的
    /// </summary>
    public class InstantiateActionPool<T>
        where T : Component
    {

        public InstantiateActionPool(uint maxCount = DefaultMaxCount)
        {
            this.maxCount = maxCount;
        }

        /// <summary>
        /// 最大保存的数目;
        /// </summary>
        private const uint DefaultMaxCount = 80;

        private uint maxCount;
        private ConcurrentQueue<InstantiateAction<T>> instanceQueue = new ConcurrentQueue<InstantiateAction<T>>();

        /// <summary>
        /// 对象池存在的元素的数目;
        /// </summary>
        public int Count { get { return instanceQueue.Count; } }

        /// <summary>
        /// 清空对象池;
        /// </summary>
        public void Clear()
        {
            instanceQueue.Clear();
        }

        /// <summary>
        /// 从对象池获取到创建方法,若不存在则新建;
        /// </summary>
        public InstantiateAction<T> TryGet(T original)
        {
            InstantiateAction<T> instance;
            if (instanceQueue.TryDequeue(out instance))
                instance.Set(original);
            else
                instance = new InstantiateAction<T>(original);
            return instance;
        }
        /// <summary>
        /// 从对象池获取到创建方法,若不存在则新建;
        /// </summary>
        public InstantiateAction<T> TryGet(T original, Vector3 position, Quaternion rotation)
        {
            InstantiateAction<T> instance;
            if (instanceQueue.TryDequeue(out instance))
                instance.Set(original, position, rotation);
            else
                instance = new InstantiateAction<T>(original, position, rotation);
            return instance;
        }
        /// <summary>
        /// 从对象池获取到创建方法,若不存在则新建;
        /// </summary>
        public InstantiateAction<T> TryGet(T original, Transform parent, bool worldPositionStays)
        {
            InstantiateAction<T> instance;
            if (instanceQueue.TryDequeue(out instance))
                instance.Set(original, parent, worldPositionStays);
            else
                instance = new InstantiateAction<T>(original, parent, worldPositionStays);
            return instance;
        }
        /// <summary>
        /// 从对象池获取到创建方法,若不存在则新建;
        /// </summary>
        public InstantiateAction<T> TryGet(T original, Vector3 position, Quaternion rotation, Transform parent)
        {
            InstantiateAction<T> instance;
            if (instanceQueue.TryDequeue(out instance))
                instance.Set(original, position, rotation, parent);
            else
                instance = new InstantiateAction<T>(original, position, rotation, parent);
            return instance;
        }

        /// <summary>
        /// 尝试将实例保存到对象池;
        /// </summary>
        public void GiveBack(InstantiateAction<T> instance)
        {
#if UNITY_EDITOR
            if (instance.IsDone != true)
                Debug.LogWarning("尝试归还一个未读取成功的异步方法;");
#endif
            instance.Clear();
            if (instanceQueue.Count <= maxCount)
            {
                instanceQueue.Enqueue(instance);
            }
        }

    }

}
