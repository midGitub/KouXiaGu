using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 异步类实例化的池;
    /// </summary>
    public class InstantiateActionPool<T>
        where T : Component
    {

        public InstantiateActionPool(uint maxCount = DefaultMaxCount)
        {
            this.maxCount = maxCount;
            instanceQueue = new ConcurrentQueue<InstantiateAction<T>>(); 
        }

        /// <summary>
        /// 最大保存的数目;
        /// </summary>
        private const uint DefaultMaxCount = 80;
        private ConcurrentQueue<InstantiateAction<T>> instanceQueue;

        private uint maxCount;
        public uint MaxCount { get { return maxCount; } }
        public int Count { get { return instanceQueue.Count; } }

        /// <summary>
        /// 尝试从队列获取到实例,若实例还在初始化队列中,则重新添加进实例队列,返回false,若已经不再初始化队列中,则返回true;
        /// </summary>
        private bool TryDequeue(out InstantiateAction<T> instance)
        {
            if (instanceQueue.TryDequeue(out instance))
            {
                if (instance.OnInitializeQueue)
                {
                    instanceQueue.Enqueue(instance);
                    return false;
                }
                else
                {
                    instance.Clear();
                    return true;
                }
            }
            instance = default(InstantiateAction<T>);
            return false;
        }

        /// <summary>
        /// 从对象池获取到创建方法,若不存在则新建;
        /// </summary>
        public InstantiateAction<T> Get(T original)
        {
            InstantiateAction<T> instance;
            if (instanceQueue.TryDequeue(out instance))
                instance.Set(original);
            else
                instance = InstantiateAction<T>.GetNew(original);
            return instance;
        }
        /// <summary>
        /// 从对象池获取到创建方法,若不存在则新建;
        /// </summary>
        public InstantiateAction<T> Get(T original, Vector3 position, Quaternion rotation)
        {
            InstantiateAction<T> instance;
            if (instanceQueue.TryDequeue(out instance))
                instance.Set(original, position, rotation);
            else
                instance = InstantiateAction<T>.GetNew(original, position, rotation);
            return instance;
        }
        /// <summary>
        /// 从对象池获取到创建方法,若不存在则新建;
        /// </summary>
        public InstantiateAction<T> Get(T original, Transform parent, bool worldPositionStays)
        {
            InstantiateAction<T> instance;
            if (instanceQueue.TryDequeue(out instance))
                instance.Set(original, parent, worldPositionStays);
            else
                instance = InstantiateAction<T>.GetNew(original, parent, worldPositionStays);
            return instance;
        }
        /// <summary>
        /// 从对象池获取到创建方法,若不存在则新建;
        /// </summary>
        public InstantiateAction<T> Get(T original, Vector3 position, Quaternion rotation, Transform parent)
        {
            InstantiateAction<T> instance;
            if (instanceQueue.TryDequeue(out instance))
                instance.Set(original, position, rotation, parent);
            else
                instance = InstantiateAction<T>.GetNew(original, position, rotation, parent);
            return instance;
        }

        /// <summary>
        /// 尝试将实例保存到对象池;
        /// </summary>
        public void GiveBack(InstantiateAction<T> instance)
        {
            if (instanceQueue.Count <= maxCount)
            {
                instanceQueue.Enqueue(instance);
            }
        }

        /// <summary>
        /// 清空对象池;
        /// </summary>
        public void Clear()
        {
            instanceQueue.Clear();
        }

    }

}
