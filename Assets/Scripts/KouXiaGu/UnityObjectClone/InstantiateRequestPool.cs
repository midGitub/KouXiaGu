//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace KouXiaGu
//{

//    /// <summary>
//    /// 异步类实例化的池;
//    /// </summary>
//    public class InstantiateRequestPool<T>
//        where T : Component
//    {
//        public InstantiateRequestPool()
//        {
//            this.maxCount = DefaultMaxCount;
//        }
//        public InstantiateRequestPool(uint maxCount)
//        {
//            this.maxCount = maxCount;
//        }

//        /// <summary>
//        /// 最大保存的数目;
//        /// </summary>
//        private const uint DefaultMaxCount = 40;
//        private ConcurrentQueue<InstantiateRequest<T>> instanceQueue = new ConcurrentQueue<InstantiateRequest<T>>();

//        private uint maxCount;
//        public uint MaxCount { get { return maxCount; } }
//        public int Count { get { return instanceQueue.Count; } }

//        /// <summary>
//        /// 从对象池获取到创建方法,若不存在则新建;
//        /// </summary>
//        public InstantiateRequest<T> Get(T original)
//        {
//            InstantiateRequest<T> instance;
//            if (TryDequeue(out instance))
//                instance.Set(original);
//            else
//                instance = new InstantiateRequest<T>(original);
//            return instance;
//        }
//        /// <summary>
//        /// 从对象池获取到创建方法,若不存在则新建;
//        /// </summary>
//        public InstantiateRequest<T> Get(T original, Vector3 position, Quaternion rotation)
//        {
//            InstantiateRequest<T> instance;
//            if (TryDequeue(out instance))
//                instance.Set(original, position, rotation);
//            else
//                instance = new InstantiateRequest<T>(original, position, rotation);
//            return instance;
//        }
//        /// <summary>
//        /// 从对象池获取到创建方法,若不存在则新建;
//        /// </summary>
//        public InstantiateRequest<T> Get(T original, Transform parent, bool worldPositionStays)
//        {
//            InstantiateRequest<T> instance;
//            if (TryDequeue(out instance))
//                instance.Set(original, parent, worldPositionStays);
//            else
//                instance = new InstantiateRequest<T>(original, parent, worldPositionStays);
//            return instance;
//        }
//        /// <summary>
//        /// 从对象池获取到创建方法,若不存在则新建;
//        /// </summary>
//        public InstantiateRequest<T> Get(T original, Vector3 position, Quaternion rotation, Transform parent)
//        {
//            InstantiateRequest<T> instance;
//            if (TryDequeue(out instance))
//                instance.Set(original, position, rotation, parent);
//            else
//                instance = new InstantiateRequest<T>(original, position, rotation, parent);
//            return instance;
//        }

//        /// <summary>
//        /// 尝试将实例保存到对象池;
//        /// </summary>
//        public void GiveBack(InstantiateRequest<T> instance)
//        {
//            if (instanceQueue.Count <= maxCount)
//            {
//                instanceQueue.Enqueue(instance);
//            }
//        }

//        /// <summary>
//        /// 尝试从队列获取到实例,若实例还在初始化队列中,则重新添加进实例队列,返回false,若已经不再初始化队列中,则返回true;
//        /// </summary>
//        private bool TryDequeue(out InstantiateRequest<T> instance)
//        {
//            if (instanceQueue.TryDequeue(out instance))
//            {
//                if (instance.OnInitializeQueue)
//                {
//                    instanceQueue.Enqueue(instance);
//                    return false;
//                }
//                else
//                {
//                    instance.Clear();
//                    return true;
//                }
//            }
//            instance = default(InstantiateRequest<T>);
//            return false;
//        }

//        /// <summary>
//        /// 清空对象池;
//        /// </summary>
//        public void Clear()
//        {
//            instanceQueue.Clear();
//        }

//    }

//}
