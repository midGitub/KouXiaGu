using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 非主线程实例化游戏物体方法;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IThreadOfInstantiate<T>
        where T : Component
    {
        /// <summary>
        /// 异步实例化物体;
        /// </summary>
        IAsyncState<T> InstantiateAsync(InstantiateRequest<T> asyncObject);
    }

    /// <summary>
    /// 拓展;
    /// </summary>
    public static class ExpandInstantiate
    {


        /// <summary>
        /// 异步的实例化;
        /// </summary>
        public static IAsyncState<T> InstantiateAsync<T>(this IThreadOfInstantiate<T> instance, T original)
            where T : Component
        {
            InstantiateRequest<T> instantiate = GetInstantiateRequest<T>(original);
            return instance.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步的实例化;
        /// </summary>
        public static IAsyncState<T> InstantiateAsync<T>(this IThreadOfInstantiate<T> instance, T original, Vector3 position, Quaternion rotation)
             where T : Component
        {
            InstantiateRequest<T> instantiate = GetInstantiateRequest<T>(original, position, rotation);
            return instance.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步的实例化;
        /// </summary>
        public static IAsyncState<T> InstantiateAsync<T>(this IThreadOfInstantiate<T> instance, T original, Transform parent, bool worldPositionStays)
             where T : Component
        {
            InstantiateRequest<T> instantiate = GetInstantiateRequest<T>(original, parent, worldPositionStays);
            return instance.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步的实例化;
        /// </summary>
        public static IAsyncState<T> InstantiateAsync<T>(this IThreadOfInstantiate<T> instance, T original, Vector3 position, Quaternion rotation, Transform parent)
             where T : Component
        {
            InstantiateRequest<T> instantiate = GetInstantiateRequest<T>(original, position, rotation, parent);
            return instance.InstantiateAsync(instantiate);
        }


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
