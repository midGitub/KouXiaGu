using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 允许线程调用的实例化池;
    /// 注意: 实例化后需要在主线程调用 MainThreadUpdate(),以保证在主线程能够实例化物体;
    /// </summary>
    public class InstancePool
    {
        public InstancePool()
        {
            this.objectPool = new ObejctPool<string, LifecycleControl>();
        }
        public InstancePool(IObjectPool<string, LifecycleControl> objectPool)
        {
            this.objectPool = objectPool;
        }

        /// <summary>
        /// 对象池接口;
        /// </summary>
        private IObjectPool<string, LifecycleControl> objectPool;
        /// <summary>
        /// 等待主线程进行从池移除事件的队列;
        /// </summary>
        private ConcurrentQueue<AsyncComponent<LifecycleControl>> waitMTOutPoolEventQueue = new ConcurrentQueue<AsyncComponent<LifecycleControl>>();
        /// <summary>
        /// 等待主线程进行加入池事件的队列;
        /// </summary>
        private ConcurrentQueue<LifecycleControl> waitMTInPoolEventQueue = new ConcurrentQueue<LifecycleControl>();

        /// <summary>
        /// 等待实例化的元素个数;
        /// </summary>
        public int WaitInstantiateCount
        {
            get { return waitMTOutPoolEventQueue.Count; }
        }
        /// <summary>
        /// 等待销毁的元素个数;
        /// </summary>
        public int WaitDestroyCount
        {
            get { return waitMTInPoolEventQueue.Count; }
        }


        #region 主线程调用

        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则初始化克隆物体并返回;
        /// </summary>
        public LifecycleControl Instantiate(LifecycleControl original)
        {
            LifecycleControl cloneObject;
            if (!TryGetInstance(original, out cloneObject))
            {
                cloneObject = UnityEngine.Object.Instantiate(original) as LifecycleControl;
                Activate(cloneObject);
            }
            return cloneObject;
        }
        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则初始化克隆物体并返回;
        /// </summary>
        public LifecycleControl Instantiate(LifecycleControl original, Vector3 position, Quaternion rotation)
        {
            LifecycleControl cloneObject;
            if (TryGetInstance(original, out cloneObject))
            {
                Transform transform = cloneObject.transform;
                transform.position = position;
                transform.rotation = rotation;
            }
            else
            {
                cloneObject = UnityEngine.Object.Instantiate(original, position, rotation) as LifecycleControl;
                Activate(cloneObject);
            }
            return cloneObject;
        }
        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则初始化克隆物体并返回;
        /// </summary>
        public LifecycleControl Instantiate(LifecycleControl original, Transform parent, bool worldPositionStays = true)
        {
            LifecycleControl cloneObject;
            if (TryGetInstance(original, out cloneObject))
            {
                cloneObject.transform.SetParent(parent, worldPositionStays);
            }
            else
            {
                cloneObject = UnityEngine.Object.Instantiate(original, parent, worldPositionStays) as LifecycleControl;
                Activate(cloneObject);
            }
            return cloneObject;
        }
        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则初始化克隆物体并返回;
        /// </summary>
        public LifecycleControl Instantiate(LifecycleControl original, Vector3 position, Quaternion rotation, Transform parent)
        {
            LifecycleControl cloneObject;
            if (TryGetInstance(original, out cloneObject))
            {
                Transform transform = cloneObject.transform;
                transform.position = position;
                transform.rotation = rotation;
                transform.SetParent(parent);
            }
            else
            {
                cloneObject = UnityEngine.Object.Instantiate(original, position, rotation, parent) as LifecycleControl;
                Activate(cloneObject);
            }
            return cloneObject;
        }
        /// <summary>
        /// 尝试保存到对象池,若保存失败则摧毁物体;
        /// </summary>
        public void Destroy(LifecycleControl instance)
        {
            Sleep(instance);
            if (TryKeepInstance(instance))
            {
                GameObject.Destroy(instance);
            }
        }

        #endregion


        #region 非主线程调用

        /// <summary>
        /// 异步的实例化,若存在对象池内则从对象池返回,否则创建一个克隆返回;
        /// </summary>
        public IAsyncState<LifecycleControl> InstantiateAsync(AsyncComponent<LifecycleControl> asyncGameObject)
        {
            AddOutPoolEventQueue(asyncGameObject);
            return asyncGameObject;
        }
        /// <summary>
        /// 异步的实例化,若存在对象池内则从对象池返回,否则创建一个克隆返回;
        /// </summary>
        public IEnumerable<IAsyncState<LifecycleControl>> InstantiateAsync(IEnumerable<AsyncComponent<LifecycleControl>> asyncGameObjects)
        {
            foreach (var asyncGameObject in asyncGameObjects)
            {
                AddOutPoolEventQueue(asyncGameObject);
                yield return asyncGameObject;
            }
        }
        /// <summary>
        /// 异步的摧毁物体,或保存到对象池;
        /// </summary>
        public void DestroyAsync(LifecycleControl instance)
        {
            AddInPoolEventQueue(instance);
        }

        #endregion


        #region 私有方法;

        /// <summary>
        /// 获取到对象池的Key;
        /// </summary>
        private string GetKey(LifecycleControl instance)
        {
            return instance.Name;
        }
        /// <summary>
        /// 获取到对象池的Key;
        /// </summary>
        private string GetKey(AsyncComponent<LifecycleControl> instance)
        {
            return instance.Original.Name;
        }

        /// <summary>
        /// 激活物体活动;
        /// </summary>
        private void Activate(LifecycleControl instance)
        {
            instance.OnActivate();
        }

        /// <summary>
        /// 停止物体活动;
        /// </summary>
        private void Sleep(LifecycleControl instance)
        {
            instance.OnSleep();
        }

        /// <summary>
        /// 在主线程更新的方法;
        /// </summary>
        protected void MainThreadUpdate(uint times)
        {
            UpdateOutPoolEventQueue(times);
            UpdateInPoolEventQueue(times);
        }

        /// <summary>
        /// 主线程调用 更新取出对象事件队列;
        /// </summary>
        private void UpdateOutPoolEventQueue(uint times)
        {
            string key;
            LifecycleControl clone;
            AsyncComponent<LifecycleControl> asyncGameObject;

            while (!waitMTOutPoolEventQueue.IsEmpty && times-- > uint.MinValue)
            {
                if (waitMTOutPoolEventQueue.TryDequeue(out asyncGameObject))
                {
                    key = GetKey(asyncGameObject);
                    if (TryGetInstance(key, out clone))
                    {
                        asyncGameObject.SetResult(clone);
                    }
                    else
                    {
                        asyncGameObject.Instantiate();
                    }
                    Activate(clone);
                }
            }
        }

        /// <summary>
        /// 主线程调用 更新加入对象事件队列;
        /// </summary>
        private void UpdateInPoolEventQueue(uint times)
        {
            LifecycleControl instance;
            while (!waitMTInPoolEventQueue.IsEmpty && times-- > uint.MinValue)
            {
                if (waitMTInPoolEventQueue.TryDequeue(out instance))
                {
                    Destroy(instance);
                }
            }
        }

        /// <summary>
        /// 主线程调用 尝试从对象池获取到物体;
        /// </summary>
        private bool TryGetInstance(LifecycleControl instance, out LifecycleControl instance2)
        {
            string key = GetKey(instance);
            return TryGetInstance(key, out instance2);
        }
        /// <summary>
        /// 主线程调用 尝试从对象池获取到物体;
        /// </summary>
        private bool TryGetInstance(string key, out LifecycleControl instance)
        {
            return objectPool.TryGetInstance(key, out instance);
        }

        /// <summary>
        /// 主线程调用 从游戏移除这个组件物体;
        /// </summary>
        private bool TryKeepInstance(LifecycleControl instance)
        {
            string key = GetKey(instance);
            return objectPool.TryKeepInstance(key, instance);
        }

        /// <summary>
        /// 加入到取出对象事件队列;
        /// </summary>
        private void AddOutPoolEventQueue(AsyncComponent<LifecycleControl> asyncInstance)
        {
            waitMTOutPoolEventQueue.Enqueue(asyncInstance);
        }

        /// <summary>
        /// 加入到加入对象事件队列;
        /// </summary>
        private void AddInPoolEventQueue(LifecycleControl instance)
        {
            waitMTInPoolEventQueue.Enqueue(instance);
        }

        #endregion

    }

}
