using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    public interface IThreadOfInstantiate<T>
        where T : Component
    {

        IAsyncState<T> InstantiateAsync(InstantiateAction<T> asyncObject);

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
            InstantiateAction<T> instantiate = InstantiateAction<T>.GetNew(original);
            return instance.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步的实例化;
        /// </summary>
        public static IAsyncState<T> InstantiateAsync<T>(this IThreadOfInstantiate<T> instance, T original, Vector3 position, Quaternion rotation)
             where T : Component
        {
            InstantiateAction<T> instantiate = InstantiateAction<T>.GetNew(original, position, rotation);
            return instance.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步的实例化;
        /// </summary>
        public static IAsyncState<T> InstantiateAsync<T>(this IThreadOfInstantiate<T> instance, T original, Transform parent, bool worldPositionStays)
             where T : Component
        {
            InstantiateAction<T> instantiate = InstantiateAction<T>.GetNew(original, parent, worldPositionStays);
            return instance.InstantiateAsync(instantiate);
        }
        /// <summary>
        /// 异步的实例化;
        /// </summary>
        public static IAsyncState<T> InstantiateAsync<T>(this IThreadOfInstantiate<T> instance, T original, Vector3 position, Quaternion rotation, Transform parent)
             where T : Component
        {
            InstantiateAction<T> instantiate = InstantiateAction<T>.GetNew(original, position, rotation, parent);
            return instance.InstantiateAsync(instantiate);
        }

    }

}
