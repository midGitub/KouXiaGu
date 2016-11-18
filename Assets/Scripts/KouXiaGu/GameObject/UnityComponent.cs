using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 对 Component 拓展方法;
    /// </summary>
    internal static class UnityComponent
    {

        /// <summary>
        /// 设置参数到;
        /// </summary>
        public static void Set(this Component instance, Vector3 position, Quaternion rotation)
        {
            Transform transform = instance.transform;
            transform.position = position;
            transform.rotation = rotation;
        }
        /// <summary>
        /// 设置参数到;
        /// </summary>
        public static void Set(this Component instance, Transform parent, bool worldPositionStays)
        {
            instance.transform.SetParent(parent, worldPositionStays);
        }
        /// <summary>
        /// 设置参数到;
        /// </summary>
        public static void Set(this Component instance, Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays = false)
        {
            instance.Set(position, rotation);
            instance.Set(parent, worldPositionStays);
        }

    }

}
