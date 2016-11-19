using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 线程安全的对象池;
    /// </summary>
    public class ConcurrentObjectPool<TKey, TValue> : IObjectPool<TKey, TValue>
        where TValue : class, IPoolObject
    {

        /// <summary>
        /// 对象池字典;
        /// </summary>
        private Dictionary<TKey, Queue<TValue>> objectDictionary = new Dictionary<TKey, Queue<TValue>>();

        /// <summary>
        /// 尝试获取到保存在对象池的实例;若加入的值为 Null 则返回false;
        /// </summary>
        public bool TryKeepInstance(TKey key, TValue instance)
        {
            Queue<TValue> objectQueue;

            if (instance == null)
            {
                return false;
            }

            lock (objectDictionary)
            {
                if (objectDictionary.TryGetValue(key, out objectQueue))
                {
                    return TryAddInObjectQueue(objectQueue, instance);
                }
                else
                {
                    objectQueue = new Queue<TValue>();
                    objectDictionary.Add(key, objectQueue);
                    return TryAddInObjectQueue(objectQueue, instance);
                }
            }
        }

        /// <summary>
        /// 尝试获取到保存在对象池的实例;
        /// </summary>
        public bool TryGetInstance(TKey key, out TValue instance)
        {
            Queue<TValue> objectQueue;

            lock (objectDictionary)
            {
                if (objectDictionary.TryGetValue(key, out objectQueue))
                {
                    return TryDequeueNotNull(objectQueue, out instance);
                }
            }

            instance = null;
            return false;
        }

        /// <summary>
        /// 加入到对象队列,若加入失败则返回false;
        /// </summary>
        private bool TryAddInObjectQueue(Queue<TValue> objectQueue, TValue instance)
        {
            if (objectQueue.Count <= instance.MaxCountInPool)
            {
                objectQueue.Enqueue(instance);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 尝试移除并返回位于 Queue<TValue> 从开始到结尾的第一个非 Null 对象,若获取到则返回true,否则返回false;
        /// </summary>
        private bool TryDequeueNotNull(Queue<TValue> objectQueue, out TValue instance)
        {
            instance = null;
            while (objectQueue.Count != 0 || instance == null)
            {
                instance = objectQueue.Dequeue();
            }
            return instance != null;
        }

        /// <summary>
        /// 清除所有保存的实例引用;
        /// </summary>
        public void Clear()
        {
            lock (objectDictionary)
            {
                foreach (var queue in objectDictionary.Values)
                {
                    queue.Clear();
                }
                objectDictionary.Clear();
            }
        }

    }

}
