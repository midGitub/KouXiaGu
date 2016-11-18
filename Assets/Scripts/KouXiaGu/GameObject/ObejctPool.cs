using System.Collections;
using System.Collections.Generic;

namespace KouXiaGu
{

    /// <summary>
    /// 加入对象池物体需要实现的接口;
    /// </summary>
    public interface IPoolObject
    {
        /// <summary>
        /// 保存在池内的最大数目;
        /// </summary>
        uint MaxCountInPool { get; }
    }

    /// <summary>
    /// 非线程安全的对象池;
    /// </summary>
    public interface IObjectPool<TKey, TValue>
    {
        /// <summary>
        /// 尝试获取到保存在对象池的实例;若加入的值为 Null 则返回false;
        /// </summary>
        bool TryGetInstance(TKey key, out TValue instance);

        /// <summary>
        /// 尝试将实例保存到对象池;
        /// </summary>
        bool TryKeepInstance(TKey key, TValue instance);

        /// <summary>
        /// 清除所有保存的实例引用;
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// 非线程安全的对象池;
    /// </summary>
    public class ObejctPool<TKey, TValue> : IObjectPool<TKey, TValue>, IEnumerable<TValue>
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

        /// <summary>
        /// 尝试获取到保存在对象池的实例;
        /// </summary>
        public bool TryGetInstance(TKey key, out TValue instance)
        {
            Queue<TValue> objectQueue;

            if (objectDictionary.TryGetValue(key, out objectQueue))
            {
                return TryDequeueNotNull(objectQueue, out instance);
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
            foreach (var queue in objectDictionary.Values)
            {
                queue.Clear();
            }
            objectDictionary.Clear();
        }

        /// <summary>
        /// 获取到所有保存到对象池的非 Null 引用;
        /// </summary>
        public IEnumerator<TValue> GetEnumerator()
        {
            foreach (var queue in objectDictionary.Values)
            {
                foreach (var item in queue)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// 获取到所有保存到对象池的非 Null 引用;
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}
