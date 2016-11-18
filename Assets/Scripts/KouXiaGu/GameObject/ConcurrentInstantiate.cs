using System;
using System.Collections.Generic;

namespace KouXiaGu
{

    /// <summary>
    /// 非主线程实例化 Unity.Component;
    /// </summary>
    public class ConcurrentInstantiate
    {

        /// <summary>
        /// 等待主线程实例化的队列;
        /// </summary>
        private ConcurrentQueue<InstantiateAction<UnityEngine.Component>> waitInstantiateQueue =
            new ConcurrentQueue<InstantiateAction<UnityEngine.Component>>();
        private ConcurrentQueue<UnityEngine.Component> waitDestroyQueue = new ConcurrentQueue<UnityEngine.Component>();


        /// <summary>
        /// 异步实例化物体;
        /// </summary>
        public IAsyncState<UnityEngine.Component> InstantiateAsync(InstantiateAction<UnityEngine.Component> asyncGameObject)
        {
            AddWaitInstantiate(asyncGameObject);
            return asyncGameObject;
        }
        /// <summary>
        /// 异步实例化物体;
        /// </summary>
        public IEnumerable<IAsyncState<UnityEngine.Component>> InstantiateAsync(
            IEnumerable<InstantiateAction<UnityEngine.Component>> asyncGameObjects)
        {
            foreach (var asyncGameObject in asyncGameObjects)
            {
                AddWaitInstantiate(asyncGameObject);
                yield return asyncGameObject;
            }
        }
        /// <summary>
        /// 异步的摧毁物体;
        /// </summary>
        public void DestroyAsync(UnityEngine.Component instance)
        {
            AddWaitDestroy(instance);
        }


        /// <summary>
        /// 加入到等待实例化队列中;
        /// </summary>
        private void AddWaitInstantiate(InstantiateAction<UnityEngine.Component> instance)
        {
            waitInstantiateQueue.Enqueue(instance);
        }
        /// <summary>
        /// 加入到等待摧毁队列中;
        /// </summary>
        private void AddWaitDestroy(UnityEngine.Component instance)
        {
            waitDestroyQueue.Enqueue(instance);
        }

        /// <summary>
        /// 在主线程更新的方法;
        /// </summary>
        public void MainThreadUpdate(uint times)
        {
            UpdateInstantiateQueue(times);
            UpdateDestroyQueue(times);
        }

        /// <summary>
        /// 主线程调用 对等待实例化队列内的物体进行实例化;
        /// </summary>
        private void UpdateInstantiateQueue(uint times)
        {
            InstantiateAction<UnityEngine.Component> async;

            while (!waitInstantiateQueue.IsEmpty && times-- > uint.MinValue)
            {
                if (waitInstantiateQueue.TryDequeue(out async))
                {
                    Instantiate(async);
                }
            }
        }

        /// <summary>
        /// 主线程调用 对等待销毁的物体进行销毁;
        /// </summary>
        private void UpdateDestroyQueue(uint times)
        {
            UnityEngine.Component instance;
            while (!waitDestroyQueue.IsEmpty && times-- > uint.MinValue)
            {
                if (waitDestroyQueue.TryDequeue(out instance))
                {
                    Destroy(instance);
                }
            }
        }

        /// <summary>
        /// 主线程调用 实例化物体;
        /// </summary>
        private void Instantiate(InstantiateAction<UnityEngine.Component> async)
        {
            async.Instantiate();
        }

        /// <summary>
        /// 主线程调用 销毁物体;
        /// </summary>
        private void Destroy(UnityEngine.Component instance)
        {
            UnityEngine.Component.Destroy(instance);
        }

    }

}
