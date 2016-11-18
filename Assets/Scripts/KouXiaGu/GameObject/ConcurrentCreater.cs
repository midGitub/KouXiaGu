//using System;
//using System.Collections.Generic;
//using UnityEngine;

//namespace KouXiaGu
//{


//    /// <summary>
//    /// 支持多线程调用的实例化方法;
//    /// </summary>
//    public class ConcurrentCreater<T>
//        where T : UnityEngine.Component
//    {
//        public void GameObjectConstructor()
//        {
//            waitMainThreadCreateQueue = new ConcurrentQueue<AsyncGameObject<T>>();
//        }

//        private ConcurrentQueue<AsyncGameObject<T>> waitMainThreadCreateQueue;


//        /// <summary>
//        /// 在主线程更新的方法;
//        /// </summary>
//        /// <param name="instantiateCount">更新的最大次数</param>
//        public virtual void MainThreadUpdate(uint instantiateCount)
//        {
//            while (!waitMainThreadCreateQueue.IsEmpty && instantiateCount-- > uint.MinValue)
//            {
//                AsyncGameObject<T> asyncGameObject;
//                if (waitMainThreadCreateQueue.TryDequeue(out asyncGameObject))
//                {
//                    asyncGameObject.Instantiate();
//                }
//            }
//        }


//        public AsyncGameObject<T> InstantiateAsync(T original)
//        {
//            AsyncGameObject<T> asyncGameObject = AsyncGameObjectPool<T>.Give();
//            asyncGameObject.Set(original);
//            return InstantiateAsync(asyncGameObject);
//        }

//        public AsyncGameObject<T> InstantiateAsync(T original, Vector3 position, Quaternion rotation)
//        {
//            AsyncGameObject<T> asyncGameObject = AsyncGameObjectPool<T>.Give();
//            asyncGameObject.Set(original, position, rotation);
//            return InstantiateAsync(asyncGameObject);
//        }

//        public AsyncGameObject<T> InstantiateAsync(T original, Transform parent, bool worldPositionStays)
//        {
//            AsyncGameObject<T> asyncGameObject = AsyncGameObjectPool<T>.Give();
//            asyncGameObject.Set(original, parent, worldPositionStays);
//            return InstantiateAsync(asyncGameObject);
//        }

//        public AsyncGameObject<T> InstantiateAsync(T original, Vector3 position, Quaternion rotation, Transform parent)
//        {
//            AsyncGameObject<T> asyncGameObject = AsyncGameObjectPool<T>.Give();
//            asyncGameObject.Set(original, position, rotation, parent);
//            return InstantiateAsync(asyncGameObject);
//        }

//        /// <summary>
//        /// 加入到等待队列内实例化;
//        /// </summary>
//        public AsyncGameObject<T> InstantiateAsync(AsyncGameObject<T> asyncGameObject)
//        {
//            waitMainThreadCreateQueue.Enqueue(asyncGameObject);
//            return asyncGameObject;
//        }

//        /// <summary>
//        /// 加入到等待队列内实例化;
//        /// </summary>
//        public void InstantiateAsync(IEnumerable<AsyncGameObject<T>> asyncGameObject)
//        {
//            waitMainThreadCreateQueue.Enqueue(asyncGameObject);
//        }

//    }

//}
