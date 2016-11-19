using System.Collections.Generic;
using System;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 允许线程调用的实例化池;
    /// 注意: 实例化后需要在主线程调用 MainThreadUpdate(),以保证在主线程能够实例化物体;
    /// </summary>
    public class XiaGuObjectPool : IThreadOfInstantiate<XiaGuObject>
    {
        public XiaGuObjectPool()
        {
            this.objectPool = new ObejctPool<string, XiaGuObject>();
        }
        public XiaGuObjectPool(IObjectPool<string, XiaGuObject> objectPool)
        {
            this.objectPool = objectPool;
        }

        /// <summary>
        /// 对象池接口;
        /// </summary>
        private IObjectPool<string, XiaGuObject> objectPool;
        /// <summary>
        /// 等待主线程进行从池移除事件的队列;
        /// </summary>
        private ConcurrentQueue<InstantiateRequest<XiaGuObject>> waitInstantiateQueue = new ConcurrentQueue<InstantiateRequest<XiaGuObject>>();
        /// <summary>
        /// 等待主线程进行加入池事件的队列;
        /// </summary>
        private ConcurrentQueue<XiaGuObject> waitDestroyQueue = new ConcurrentQueue<XiaGuObject>();

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

        private void NullXiaGuObjectException(XiaGuObject original)
        {
            if(original == null)
                throw new NullReferenceException();
        }

        #region 主线程调用

        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则初始化克隆物体并返回;
        /// </summary>
        public XiaGuObject Instantiate(XiaGuObject original)
        {
            NullXiaGuObjectException(original);

            XiaGuObject cloneObject;
            if (!TryGetInstance(original, out cloneObject))
            {
                cloneObject = UnityEngine.Object.Instantiate(original) as XiaGuObject;
                Activate(cloneObject);
            }
            return cloneObject;
        }
        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则初始化克隆物体并返回;
        /// </summary>
        public XiaGuObject Instantiate(XiaGuObject original, Vector3 position, Quaternion rotation)
        {
            NullXiaGuObjectException(original);

            XiaGuObject cloneObject;
            if (TryGetInstance(original, out cloneObject))
            {
                Transform transform = cloneObject.transform;
                transform.position = position;
                transform.rotation = rotation;
            }
            else
            {
                cloneObject = UnityEngine.Object.Instantiate(original, position, rotation) as XiaGuObject;
                Activate(cloneObject);
            }
            return cloneObject;
        }
        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则初始化克隆物体并返回;
        /// </summary>
        public XiaGuObject Instantiate(XiaGuObject original, Transform parent, bool worldPositionStays = true)
        {
            NullXiaGuObjectException(original);

            XiaGuObject cloneObject;
            if (TryGetInstance(original, out cloneObject))
            {
                cloneObject.transform.SetParent(parent, worldPositionStays);
            }
            else
            {
                cloneObject = UnityEngine.Object.Instantiate(original, parent, worldPositionStays) as XiaGuObject;
                Activate(cloneObject);
            }
            return cloneObject;
        }
        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则初始化克隆物体并返回;
        /// </summary>
        public XiaGuObject Instantiate(XiaGuObject original, Vector3 position, Quaternion rotation, Transform parent)
        {
            NullXiaGuObjectException(original);

            XiaGuObject cloneObject;
            if (TryGetInstance(original, out cloneObject))
            {
                Transform transform = cloneObject.transform;
                transform.position = position;
                transform.rotation = rotation;
                transform.SetParent(parent);
            }
            else
            {
                cloneObject = UnityEngine.Object.Instantiate(original, position, rotation, parent) as XiaGuObject;
                Activate(cloneObject);
            }
            return cloneObject;
        }
        /// <summary>
        /// 尝试保存到对象池,若保存失败则摧毁物体;
        /// </summary>
        public void Destroy(XiaGuObject instance)
        {
            NullXiaGuObjectException(instance);

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
        public IAsyncState<XiaGuObject> InstantiateAsync(InstantiateRequest<XiaGuObject> asyncGameObject)
        {
            AddInstantiateQueue(asyncGameObject);
            return asyncGameObject;
        }
        /// <summary>
        /// 异步的摧毁物体,或保存到对象池;
        /// </summary>
        public void DestroyAsync(XiaGuObject instance)
        {
            NullXiaGuObjectException(instance);
            AddDestroyQueue(instance);
        }

        #endregion


        #region 私有方法;

        /// <summary>
        /// 获取到对象池的Key;
        /// </summary>
        private string GetKey(XiaGuObject instance)
        {
            return instance.Name;
        }
        /// <summary>
        /// 获取到对象池的Key;
        /// </summary>
        private string GetKey(InstantiateRequest<XiaGuObject> instance)
        {
            return instance.Original.Name;
        }

        /// <summary>
        /// 激活物体活动;
        /// </summary>
        private void Activate(XiaGuObject instance)
        {
            instance.OnActivate();
        }

        /// <summary>
        /// 停止物体活动;
        /// </summary>
        private void Sleep(XiaGuObject instance)
        {
            instance.OnSleep();
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
        /// 主线程调用 更新取出对象事件队列;
        /// </summary>
        private void UpdateInstantiateQueue(uint times)
        {
            string key;
            XiaGuObject clone;
            InstantiateRequest<XiaGuObject> asyncGameObject;

            while (!waitInstantiateQueue.IsEmpty && times-- > uint.MinValue)
            {
                if (waitInstantiateQueue.TryDequeue(out asyncGameObject))
                {
                    asyncGameObject.OnInitializeQueue = false;
                    key = GetKey(asyncGameObject);
                    if (TryGetInstance(key, out clone))
                    {
                        asyncGameObject.SetResult(clone);
                    }
                    else
                    {
                        clone = asyncGameObject.Instantiate();
                    }
                    Activate(clone);
                }
            }
        }

        /// <summary>
        /// 主线程调用 更新加入对象事件队列;
        /// </summary>
        private void UpdateDestroyQueue(uint times)
        {
            XiaGuObject instance;
            while (!waitDestroyQueue.IsEmpty && times-- > uint.MinValue)
            {
                if (waitDestroyQueue.TryDequeue(out instance))
                {
                    Destroy(instance);
                }
            }
        }

        /// <summary>
        /// 主线程调用 尝试从对象池获取到物体;
        /// </summary>
        private bool TryGetInstance(XiaGuObject instance, out XiaGuObject instance2)
        {
            string key = GetKey(instance);
            return TryGetInstance(key, out instance2);
        }
        /// <summary>
        /// 主线程调用 尝试从对象池获取到物体;
        /// </summary>
        private bool TryGetInstance(string key, out XiaGuObject instance)
        {
            return objectPool.TryGetInstance(key, out instance);
        }

        /// <summary>
        /// 主线程调用 从游戏移除这个组件物体;
        /// </summary>
        private bool TryKeepInstance(XiaGuObject instance)
        {
            string key = GetKey(instance);
            return objectPool.TryKeepInstance(key, instance);
        }

        /// <summary>
        /// 加入到取出对象事件队列;
        /// </summary>
        private void AddInstantiateQueue(InstantiateRequest<XiaGuObject> asyncInstance)
        {
            waitInstantiateQueue.Enqueue(asyncInstance);
            asyncInstance.OnInitializeQueue = true;
        }

        /// <summary>
        /// 加入到加入对象事件队列;
        /// </summary>
        private void AddDestroyQueue(XiaGuObject instance)
        {
            waitDestroyQueue.Enqueue(instance);
        }

        #endregion

    }

}
