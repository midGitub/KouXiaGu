using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// Unity物体实例化\克隆方法;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ComponentClone : MonoBehaviour
    {
        private ComponentClone() { }

        [SerializeField, Range(1, 100)]
        private uint times = 50;

        private static ConcurrentInstantiate concurrentInstantiate;
        private static XiaGuObjectPool xiaGuObjectPool;
        private static ComponentClone instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                concurrentInstantiate = new ConcurrentInstantiate();
                xiaGuObjectPool = new XiaGuObjectPool();
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


        #region XiaGuObjectPool

        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则初始化克隆物体并返回;
        /// </summary>
        public static XiaGuObject PoolInstantiate(XiaGuObject original)
        {
            return xiaGuObjectPool.Instantiate(original);
        }
        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则初始化克隆物体并返回;
        /// </summary>
        public static XiaGuObject PoolInstantiate(XiaGuObject original, Vector3 position, Quaternion rotation)
        {
            return xiaGuObjectPool.Instantiate(original, position, rotation);
        }
        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则初始化克隆物体并返回;
        /// </summary>
        public static XiaGuObject PoolInstantiate(XiaGuObject original, Transform parent, bool worldPositionStays = true)
        {
            return xiaGuObjectPool.Instantiate(original, parent, worldPositionStays);
        }
        /// <summary>
        /// 从对象池取出物体,若存在物体则初始化返回,否则初始化克隆物体并返回;
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
        public static IAsyncState<XiaGuObject> PoolInstantiateAsync(InstantiateAction<XiaGuObject> asyncGameObject)
        {
            return xiaGuObjectPool.InstantiateAsync(asyncGameObject);
        }
        /// <summary>
        /// 异步的实例化,若存在对象池内则从对象池返回,否则创建一个克隆返回;
        /// </summary>
        public static IEnumerable<IAsyncState<XiaGuObject>> PoolInstantiateAsync(IEnumerable<InstantiateAction<XiaGuObject>> asyncGameObjects)
        {
            return xiaGuObjectPool.InstantiateAsync(asyncGameObjects);
        }
        /// <summary>
        /// 异步的摧毁物体,或保存到对象池;
        /// </summary>
        public static void PoolDestroyAsync(XiaGuObject instance)
        {
            xiaGuObjectPool.DestroyAsync(instance);
        }

        #endregion

        #region ConcurrentInstantiate

        ///// <summary>
        ///// Unity方法实例化到游戏中;
        ///// </summary>
        //public static UnityEngine.Object Instantiate(UnityEngine.Object original)
        //{
        //    return UnityEngine.Object.Instantiate(original);
        //}
        ///// <summary>
        ///// Unity方法实例化到游戏中;
        ///// </summary>
        //public static UnityEngine.Object Instantiate(UnityEngine.Object original, Vector3 position, Quaternion rotation)
        //{
        //    return UnityEngine.Object.Instantiate(original, position, rotation);
        //}
        ///// <summary>
        ///// Unity方法实例化到游戏中;
        ///// </summary>
        //public static UnityEngine.Object Instantiate(UnityEngine.Object original, Transform parent, bool worldPositionStays)
        //{
        //    return UnityEngine.Object.Instantiate(original, parent, worldPositionStays);
        //}
        ///// <summary>
        ///// Unity方法实例化到游戏中;
        ///// </summary>
        //public static UnityEngine.Object Instantiate(UnityEngine.Object original, Vector3 position, Quaternion rotation, Transform parent)
        //{
        //    return UnityEngine.Object.Instantiate(original, position, rotation, parent);
        //}

        /// <summary>
        /// 异步实例化;
        /// </summary>
        public static IAsyncState<Component> InstantiateAsync(InstantiateAction<Component> asyncOject)
        {
            return concurrentInstantiate.InstantiateAsync(asyncOject);
        }
        /// <summary>
        /// 异步实例化;
        /// </summary>
        public static IEnumerable<IAsyncState<Component>> InstantiateAsync(IEnumerable<InstantiateAction<Component>> asyncOjects)
        {
            return concurrentInstantiate.InstantiateAsync(asyncOjects);
        }
        /// <summary>
        /// 异步摧毁物体;
        /// </summary>
        public static void DestroyAsync(Component instance)
        {
            concurrentInstantiate.DestroyAsync(instance);
        }

        #endregion

    }

}
