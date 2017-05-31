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
    public abstract class SceneObject : MonoBehaviour
    {
        protected SceneObject()
        {
        }

        [CustomUnityTag]
        public const string DefaultTagName = "SceneController";
        static SceneObject current;

        /// <summary>
        /// 当前场景中的控制器;
        /// </summary>
        public static SceneObject Current
        {
            get { return current; }
        }

        protected virtual void Awake()
        {
            if (current != null)
            {
                Debug.LogError("场景已经存在 SceneManager:" + current.ToString() + ";尝试加入新的:" + ToString());
            }
            else
            {
                tag = DefaultTagName;
                current = this;
            }
        }

        protected virtual void OnDestroy()
        {
            if (current != this)
            {
                Debug.LogError("当前的 SceneManager: " + current.ToString() + " 不为即将销毁的;" + ToString());
            }
            else
            {
                current = null;
            }
        }

        /// <summary>
        /// 获取到场景中第一个 这个标签的 实例;
        /// </summary>
        public static GameObject GetSceneManagerObject()
        {
            return current == null ? GameObject.FindWithTag(DefaultTagName) : current.gameObject;
        }

        /// <summary>
        /// 获取到挂载在场景控制器下的第一个实例,若不存在则返回异常;
        /// </summary>
        public static T Find<T>()
            where T : MonoBehaviour
        {
            var ins = GetSceneManagerObject().GetComponentInChildren<T>();

            if (ins == null)
                throw new KeyNotFoundException("未找到类[" + typeof(T).Name + "]");

            return ins;
        }

        /// <summary>
        /// 获取到场景中存在这个标签的实例数量;
        /// </summary>
        public static int GetCountInScene()
        {
            var array = GameObject.FindGameObjectsWithTag(DefaultTagName);
            return array.Length;
        }
    }
}
