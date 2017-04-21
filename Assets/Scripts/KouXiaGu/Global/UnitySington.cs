using System;
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
            private set { instance = value; }
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
                    string errorStr = "存在多个单例在场景!将销毁其它;";

                    for (int i = 1; i < instances.Length; i++)
                    {
                        var other = instances[i];
                        errorStr += string.Format("\n[{0}]{1}", i, "销毁多余单例:" + other.name);
                        GameObject.Destroy(other);
                    }

                    Debug.LogError(errorStr);
                }
            }
            return instance;
        }


        /// <summary>
        /// 手动设置到单例,若出现错误则弹出异常;
        /// </summary>
        protected static void SetInstance(T instance)
        {
            if (Instance != instance && Instance != null)
            {
                throw new ArgumentException("实例化多个Unity单例;");
            }

            Instance = instance;
        }


        [ContextMenu("输出场景实例数目;")]
        protected void LogSingtonCount()
        {
            var instances = GameObject.FindObjectsOfType<T>();
            Debug.Log(instances.ToLog("场景存在单例"));
        }


    }

}
