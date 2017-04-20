using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// Unity单例;
    /// </summary>
    public abstract class UnitySington<T> : MonoBehaviour
        where T : UnitySington<T>
    {
        static T instance;

        /// <summary>
        /// 获取到单例,仅Unity线程访问;
        /// </summary>
        public static T Instance
        {
            get { return instance ?? Initialize(); }
        }

        /// <summary>
        /// 是否已经实例化?
        /// </summary>
        public static bool IsInstanced
        {
            get { return instance != null; }
        }

        static T Initialize()
        {
            if (instance == null)
            {
                var instances = GameObject.FindObjectsOfType<T>();

                if (instances.Length == 0)
                {
                    var type = typeof(T);
                    var gameObject = new GameObject(type.Name, type);
                    instance = gameObject.GetComponent<T>();
                    Debug.Log("已创建单例;" + type.Name);
                }
                else if (instances.Length == 1)
                {
                    instance = instances[0];
                }
                else
                {
                    instance = instances[0];
                    Debug.LogError(instances.ToLog("存在多个单例在场景!将销毁其它;"));

                    for (int i = 1; i < instances.Length; i++)
                    {
                        var other = instances[i];
                        Debug.LogWarning("销毁多余单例:" + other.name);
                        GameObject.Destroy(other);
                    }
                }
            }
            return instance;
        }

    }

}
