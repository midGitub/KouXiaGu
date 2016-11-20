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
    public interface IThreadToClone<T>
        where T : Component
    {
        /// <summary>
        /// 异步实例化物体;
        /// </summary>
        void InstantiateQueue(IRequestForInstance<T> asyncObject);
    }

    /// <summary>
    /// 拓展;
    /// </summary>
    public static class ExpandUnityObjectClone
    {

        public static IAsyncState<T> InstantiateState<T>(this IThreadToClone<T> instance, RequestForInstance<T> asyncObject)
            where T : UnityEngine.Component
        {
            RequestForInstanceAsync<T> instantiate = new RequestForInstanceAsync<T>(asyncObject);
            instance.InstantiateQueue(instantiate);
            return instantiate;
        }

        public static IAsyncState<T> InstantiateState<T>(this IThreadToClone<T> instance, T original)
            where T : Component
        {
            RequestForInstanceAsync<T> instantiate = new RequestForInstanceAsync<T>(original);
            instance.InstantiateQueue(instantiate);
            return instantiate;
        }

        public static IAsyncState<T> InstantiateState<T>(this IThreadToClone<T> instance, T original, Vector3 position, Quaternion rotation)
             where T : Component
        {
            RequestForInstanceAsync<T> instantiate = new RequestForInstanceAsync<T>(original, position, rotation);
            instance.InstantiateQueue(instantiate);
            return instantiate;
        }

        public static IAsyncState<T> InstantiateState<T>(this IThreadToClone<T> instance, T original, Transform parent, bool worldPositionStays)
             where T : Component
        {
            RequestForInstanceAsync<T> instantiate = new RequestForInstanceAsync<T>(original, parent, worldPositionStays);
            instance.InstantiateQueue(instantiate);
            return instantiate;
        }

        public static IAsyncState<T> InstantiateState<T>(this IThreadToClone<T> instance, T original, Vector3 position, Quaternion rotation, Transform parent)
             where T : Component
        {
            RequestForInstanceAsync<T> instantiate = new RequestForInstanceAsync<T>(original, position, rotation, parent);
            instance.InstantiateQueue(instantiate);
            return instantiate;
        }

        public static void InstantiateInQueue<T>(this IThreadToClone<T> instance, RequestForInstance<T> asyncObject)
            where T : UnityEngine.Component
        {
            instance.InstantiateQueue(asyncObject);
        }

        public static void InstantiateInQueue<T>(this IThreadToClone<T> instance, T original)
             where T : Component
        {
            RequestForInstance<T> instantiate = new RequestForInstance<T>(original);
            instance.InstantiateQueue(instantiate);
        }

        public static void InstantiateInQueue<T>(this IThreadToClone<T> instance, T original, Vector3 position, Quaternion rotation)
             where T : Component
        {
            RequestForInstance<T> instantiate = new RequestForInstance<T>(original, position, rotation);
            instance.InstantiateQueue(instantiate);
        }

        public static void InstantiateInQueue<T>(this IThreadToClone<T> instance, T original, Transform parent, bool worldPositionStays)
            where T : Component
        {
            RequestForInstance<T> instantiate = new RequestForInstance<T>(original, parent, worldPositionStays);
            instance.InstantiateQueue(instantiate);
        }

        public static void InstantiateInQueue<T>(this IThreadToClone<T> instance, T original, Vector3 position, Quaternion rotation, Transform parent)
             where T : Component
        {
            RequestForInstance<T> instantiate = new RequestForInstance<T>(original, position, rotation, parent);
            instance.InstantiateQueue(instantiate);
        }

    }

}
