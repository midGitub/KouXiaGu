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
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    return Find();
                }
#endif
                return instance ?? (instance = FindOrCreate());
            }
        }

        /// <summary>
        /// 是否已经实例化?
        /// </summary>
        public static bool IsInitialized
        {
            get { return instance != null; }
        }

        /// <summary>
        /// 编辑模式下使用的模式;
        /// </summary>
        internal static T FindInEditor()
        {
            T instance;
            var instances = GameObject.FindObjectsOfType<T>();
            if (instances.Length == 0)
            {
                instance = CreateInstance();
            }
            else if (instances.Length == 1)
            {
                instance = instances[0];
            }
            else
            {
                instance = instances[0];
                Debug.LogError(instances.ToLog("多个单例脚本存在于场景!"));
            }
            return instance;
        }

        internal static T Find()
        {
            T instance = GameObject.FindObjectOfType<T>();
            return instance;
        }

        internal static T FindOrCreate()
        {
            T instance = Find();
            if (instance == null)
            {
                instance = CreateInstance();
            }
            return instance;
        }

        internal static T CreateInstance()
        {
            var type = typeof(T);
            var gameObject = new GameObject(type.Name, type);
            T item = gameObject.GetComponent<T>();
            return item;
        }



        [Obsolete]
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
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            if (Instance != null && Instance != instance)
            {
                throw new ArgumentException("设置不同的单例;原本:" + (Instance as MonoBehaviour).name + ",请求:" + (instance as MonoBehaviour).name);
            }
            UnitySington<T>.instance = instance;
        }

        [ContextMenu("输出场景实例数目;")]
        protected void LogSingtonCount()
        {
            var instances = GameObject.FindObjectsOfType<T>();
            Debug.Log(instances.ToLog("场景存在单例"));
        }
    }
}
