using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 非主线程实例化 Unity.Component;
    /// </summary>
    public class ConcurrentInstantiate : IThreadOfInstantiate<Component>
    {

        /// <summary>
        /// 等待主线程实例化的队列;
        /// </summary>
        private ConcurrentQueue<InstantiateAction<Component>> waitInstantiateQueue =
            new ConcurrentQueue<InstantiateAction<Component>>();
        private ConcurrentQueue<Component> waitDestroyQueue = new ConcurrentQueue<Component>();


        /// <summary>
        /// 等待实例化的元素个数;
        /// </summary>
        public int WaitInstantiateCount
        {
            get { return waitInstantiateQueue.Count; }
        }
        /// <summary>
        /// 等待销毁的元素个数;
        /// </summary>
        public int WaitDestroyCount
        {
            get { return waitDestroyQueue.Count; }
        }


        /// <summary>
        /// 异步实例化物体;
        /// </summary>
        public IAsyncState<Component> InstantiateAsync(InstantiateAction<Component> asyncGameObject)
        {
            AddWaitInstantiate(asyncGameObject);
            return asyncGameObject;
        }
        /// <summary>
        /// 异步的摧毁物体;
        /// </summary>
        public void DestroyAsync(Component instance)
        {
            AddWaitDestroy(instance);
        }


        /// <summary>
        /// 加入到等待实例化队列中;
        /// </summary>
        private void AddWaitInstantiate(InstantiateAction<Component> instance)
        {
            waitInstantiateQueue.Enqueue(instance);
            instance.OnInitializeQueue = true;
        }
        /// <summary>
        /// 加入到等待摧毁队列中;
        /// </summary>
        private void AddWaitDestroy(Component instance)
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
            InstantiateAction<Component> async;

            while (!waitInstantiateQueue.IsEmpty && times-- > uint.MinValue)
            {
                if (waitInstantiateQueue.TryDequeue(out async))
                {
                    async.OnInitializeQueue = false;
                    Instantiate(async);
                }
            }
        }

        /// <summary>
        /// 主线程调用 对等待销毁的物体进行销毁;
        /// </summary>
        private void UpdateDestroyQueue(uint times)
        {
            Component instance;
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
        private void Instantiate(InstantiateAction<Component> async)
        {
            async.Instantiate();
        }

        /// <summary>
        /// 主线程调用 销毁物体;
        /// </summary>
        private void Destroy(Component instance)
        {
            UnityEngine.Component.Destroy(instance);
        }

    }

}
