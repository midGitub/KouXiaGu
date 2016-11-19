using System.Collections.Generic;
using UnityEngine;


namespace KouXiaGu
{

    /// <summary>
    /// Unity物体实例化\克隆方法;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ObjectClone : MonoBehaviour
    {
        private ObjectClone() { }

        [SerializeField, Range(1, 100)]
        private uint times = 50;

        internal static readonly ConcurrentInstantiate concurrentInstantiate = new ConcurrentInstantiate();
        internal static readonly XiaGuObjectPool xiaGuObjectPool = new XiaGuObjectPool();
        internal static ObjectClone instance;

        [ShowOnlyProperty]
        public static int WaitDestroy0
        {
            get { return concurrentInstantiate.WaitDestroyCount; }
        }
        [ShowOnlyProperty]
        public static int WaitDestroy1
        {
            get { return xiaGuObjectPool.WaitDestroyCount; }
        }
        [ShowOnlyProperty]
        public static int WaitInstantiate0
        {
            get { return concurrentInstantiate.WaitInstantiateCount; }
        }
        [ShowOnlyProperty]
        public static int WaitInstantiate1
        {
            get { return xiaGuObjectPool.WaitInstantiateCount; }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("重复的实例化的单例!" + this.GetType().FullName);
                Destroy(this);
            }
        }

        /// <summary>
        /// 主线程更新;
        /// </summary>
        private void Update()
        {
            concurrentInstantiate.MainThreadUpdate(times);
            xiaGuObjectPool.MainThreadUpdate(times);
        }

        #region Instantiate

        /// <summary>
        /// 异步实例化;
        /// </summary>
        public static IAsyncState<Component> InstantiateInThread(InstantiateAction<Component> asyncOject)
        {
            return concurrentInstantiate.InstantiateAsync(asyncOject);
        }
        /// <summary>
        /// 异步实例化;
        /// </summary>
        public static IAsyncState<Component> InstantiateInThread(Component original)
        {
            return concurrentInstantiate.InstantiateAsync(original);
        }
        /// <summary>
        /// 异步实例化;
        /// </summary>
        public static IAsyncState<Component> InstantiateInThread(Component original, Vector3 position, Quaternion rotation)
        {
            return concurrentInstantiate.InstantiateAsync(original, position, rotation);
        }
        /// <summary>
        /// 异步实例化;
        /// </summary>
        public static IAsyncState<Component> InstantiateInThread(Component original, Transform parent, bool worldPositionStays = true)
        {
            return concurrentInstantiate.InstantiateAsync(original, parent, worldPositionStays);
        }
        /// <summary>
        /// 异步实例化;
        /// </summary>
        public static IAsyncState<Component> InstantiateInThread(Component original, Vector3 position, Quaternion rotation, Transform parent)
        {
            return concurrentInstantiate.InstantiateAsync(original, position, rotation, parent);
        }
        /// <summary>
        /// 异步摧毁物体;
        /// </summary>
        public static void DestroyInThread(Component instance)
        {
            concurrentInstantiate.DestroyAsync(instance);
        }

        #endregion

        #region Pool

        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则实例化\克隆物体并返回;
        /// </summary>
        public static XiaGuObject PoolInstantiate(XiaGuObject original)
        {
            return xiaGuObjectPool.Instantiate(original);
        }
        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则实例化\克隆物体并返回;
        /// </summary>
        public static XiaGuObject PoolInstantiate(XiaGuObject original, Vector3 position, Quaternion rotation)
        {
            return xiaGuObjectPool.Instantiate(original, position, rotation);
        }
        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则实例化\克隆物体并返回;
        /// </summary>
        public static XiaGuObject PoolInstantiate(XiaGuObject original, Transform parent, bool worldPositionStays = true)
        {
            return xiaGuObjectPool.Instantiate(original, parent, worldPositionStays);
        }
        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则实例化\克隆物体并返回;
        /// </summary>
        public static XiaGuObject PoolInstantiate(XiaGuObject original, Vector3 position, Quaternion rotation, Transform parent)
        {
            return xiaGuObjectPool.Instantiate(original, position, rotation, parent);
        }
        /// <summary>
        /// 尝试保存到对象池,若保存失败则摧毁物体;
        /// </summary>
        public static void PoolDestroy(XiaGuObject instance)
        {
            xiaGuObjectPool.Destroy(instance);
        }

        /// <summary>
        /// 异步的实例化,若存在对象池内则从对象池返回,否则创建一个克隆返回;
        /// </summary>
        public static IAsyncState<XiaGuObject> InstantiateInThread(InstantiateAction<XiaGuObject> asyncGameObject)
        {
            return xiaGuObjectPool.InstantiateAsync(asyncGameObject);
        }
        /// <summary>
        /// 异步的实例化,若存在对象池内则从对象池返回,否则创建一个克隆返回;
        /// </summary>
        public static IAsyncState<XiaGuObject> InstantiateInThread(XiaGuObject original)
        {
            return xiaGuObjectPool.InstantiateAsync(original);
        }
        /// <summary>
        /// 异步的实例化,若存在对象池内则从对象池返回,否则创建一个克隆返回;
        /// </summary>
        public static IAsyncState<XiaGuObject> InstantiateInThread(XiaGuObject original, Vector3 position, Quaternion rotation)
        {
            return xiaGuObjectPool.InstantiateAsync(original, position, rotation);
        }
        /// <summary>
        /// 异步的实例化,若存在对象池内则从对象池返回,否则创建一个克隆返回;
        /// </summary>
        public static IAsyncState<XiaGuObject> InstantiateInThread(XiaGuObject original, Transform parent, bool worldPositionStays = true)
        {
            return xiaGuObjectPool.InstantiateAsync(original, parent, worldPositionStays);
        }
        /// <summary>
        /// 异步的实例化,若存在对象池内则从对象池返回,否则创建一个克隆返回;
        /// </summary>
        public static IAsyncState<XiaGuObject> InstantiateInThread(XiaGuObject original, Vector3 position, Quaternion rotation, Transform parent)
        {
            return xiaGuObjectPool.InstantiateAsync(original, position, rotation, parent);
        }
        /// <summary>
        /// 异步的摧毁物体,或保存到对象池;
        /// </summary>
        public static void DestroyInThread(XiaGuObject instance)
        {
            xiaGuObjectPool.DestroyAsync(instance);
        }

        #endregion

    }

}
