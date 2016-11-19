using UnityEngine;


namespace KouXiaGu
{

    /// <summary>
    /// Unity物体实例化\克隆方法;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UnityObjectClone : MonoBehaviour
    {
        private UnityObjectClone() { }

        [SerializeField, Range(1, 300)]
        private uint times = 120;

        internal static readonly ConcurrentInstantiate concurrentInstantiate = new ConcurrentInstantiate();
        internal static readonly XiaGuObjectPool xiaGuObjectPool = new XiaGuObjectPool();
        internal static UnityObjectClone instance;

        [SerializeField]
        private int max = 0;

        [ShowOnlyProperty]
        public static int WaitDestroy0
        {
            get{ return concurrentInstantiate.WaitDestroyCount; }
        }
        [ShowOnlyProperty]
        public static int WaitDestroy1
        {
            get { return xiaGuObjectPool.WaitDestroyCount; }
        }
        [ShowOnlyProperty]
        public static int WaitInstantiate0
        {
            get{ return concurrentInstantiate.WaitInstantiateCount; }
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
            int count = WaitInstantiate0;
            if (max < count)
                max = count;

            concurrentInstantiate.MainThreadUpdate(times);
            xiaGuObjectPool.MainThreadUpdate(times);
        }

        #region Instantiate

        /// <summary>
        /// 异步实例化;
        /// </summary>
        public static IAsyncState<Component> InstantiateInThread(InstantiateRequest<Component> instantiate)
        {
            return concurrentInstantiate.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步实例化;
        /// </summary>
        public static IAsyncState<Component> InstantiateInThread(Component original)
        {
            var instantiate = GetInstantiateRequest<Component>(original);
            return concurrentInstantiate.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步实例化;
        /// </summary>
        public static IAsyncState<Component> InstantiateInThread(Component original, Vector3 position, Quaternion rotation)
        {
            var instantiate = GetInstantiateRequest<Component>(original, position, rotation);
            return concurrentInstantiate.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步实例化;
        /// </summary>
        public static IAsyncState<Component> InstantiateInThread(Component original, Transform parent, bool worldPositionStays = true)
        {
            var instantiate = GetInstantiateRequest<Component>(original, parent, worldPositionStays);
            return concurrentInstantiate.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步实例化;
        /// </summary>
        public static IAsyncState<Component> InstantiateInThread(Component original, Vector3 position, Quaternion rotation, Transform parent)
        {
            var instantiate = GetInstantiateRequest<Component>(original, position, rotation, parent);
            return concurrentInstantiate.InstantiateAsync(instantiate);
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
        public static IAsyncState<XiaGuObject> InstantiateInThread(InstantiateRequest<XiaGuObject> asyncGameObject)
        {
            return xiaGuObjectPool.InstantiateAsync(asyncGameObject);
        }
        /// <summary>
        /// 异步的实例化,若存在对象池内则从对象池返回,否则创建一个克隆返回;
        /// </summary>
        public static IAsyncState<XiaGuObject> InstantiateInThread(XiaGuObject original)
        {
            var instantiate = GetInstantiateRequest<XiaGuObject>(original);
            return xiaGuObjectPool.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步的实例化,若存在对象池内则从对象池返回,否则创建一个克隆返回;
        /// </summary>
        public static IAsyncState<XiaGuObject> InstantiateInThread(XiaGuObject original, Vector3 position, Quaternion rotation)
        {
            var instantiate = GetInstantiateRequest<XiaGuObject>(original, position, rotation);
            return xiaGuObjectPool.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步的实例化,若存在对象池内则从对象池返回,否则创建一个克隆返回;
        /// </summary>
        public static IAsyncState<XiaGuObject> InstantiateInThread(XiaGuObject original, Transform parent, bool worldPositionStays = true)
        {
            var instantiate = GetInstantiateRequest<XiaGuObject>(original, parent, worldPositionStays);
            return xiaGuObjectPool.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步的实例化,若存在对象池内则从对象池返回,否则创建一个克隆返回;
        /// </summary>
        public static IAsyncState<XiaGuObject> InstantiateInThread(XiaGuObject original, Vector3 position, Quaternion rotation, Transform parent)
        {
            var instantiate = GetInstantiateRequest<XiaGuObject>(original, position, rotation, parent);
            return xiaGuObjectPool.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步的摧毁物体,或保存到对象池;
        /// </summary>
        public static void DestroyInThread(XiaGuObject instance)
        {
            xiaGuObjectPool.DestroyAsync(instance);
        }

        #endregion

        #region 获取到请求类;

        private static InstantiateRequest<T> GetInstantiateRequest<T>(T original)
             where T : Component
        {
            InstantiateRequest<T> instantiate = new InstantiateRequest<T>(original);
            return instantiate;
        }
        private static InstantiateRequest<T> GetInstantiateRequest<T>(T original, Vector3 position, Quaternion rotation)
             where T : Component
        {
            InstantiateRequest<T> instantiate = new InstantiateRequest<T>(original, position, rotation);
            return instantiate;
        }
        private static InstantiateRequest<T> GetInstantiateRequest<T>(T original, Transform parent, bool worldPositionStays)
            where T : Component
        {
            InstantiateRequest<T> instantiate = new InstantiateRequest<T>(original, parent, worldPositionStays);
            return instantiate;
        }
        private static InstantiateRequest<T> GetInstantiateRequest<T>(T original, Vector3 position, Quaternion rotation, Transform parent)
            where T : Component
        {
            InstantiateRequest<T> instantiate = new InstantiateRequest<T>(original, position, rotation, parent);
            return instantiate;
        }

        #endregion

    }

}
