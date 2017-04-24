using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 挂载在场景控制器下的物体;
    /// </summary>
    public static class SceneObject
    {

        [CustomUnityTag]
        public const string TagName = "SceneController";

        /// <summary>
        /// 获取到场景中第一个 这个标签的 实例;
        /// </summary>
        public static GameObject GetGameObject()
        {
            
            return GameObject.FindWithTag(TagName);
        }

        /// <summary>
        /// 获取到场景中存在这个标签的实例数量;
        /// </summary>
        public static int GetCountInScene()
        {
            var array = GameObject.FindGameObjectsWithTag(TagName);
            return array.Length;
        }

        /// <summary>
        /// 获取到挂载在场景控制器下的第一个实例,若不存在则返回异常;
        /// </summary>
        public static T GetObject<T>()
            where T : MonoBehaviour
        {
            var ins = GetGameObject().GetComponentInChildren<T>();

            if (ins == null)
                throw new KeyNotFoundException("未找到类[" + typeof(T).Name + "]");

            return ins;
        }

    }

}
