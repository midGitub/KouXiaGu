using System;
using System.Threading;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// Unity单例,允许多线程访问;
    /// </summary>
    public abstract class UnitySington<T> : MonoBehaviour
        where T : UnitySington<T>
    {

        [CustomUnityTag]
        public const string DefaultTagName = "GlobalController";

        static T _instance;

        /// <summary>
        /// 获取到单例,仅Unity线程访问;
        /// </summary>
        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (XiaGu.IsUnityThread && !Application.isPlaying)
                {
                    return FindInEditor();
                }
#endif
                return _instance ?? (_instance = FindOrCreate());
            }
        }

        /// <summary>
        /// 是否已经实例化?
        /// </summary>
        public static bool IsInitialized
        {
            get { return _instance != null; }
        }

        internal static T FindInEditor()
        {
            Debug.LogWarning("在编辑模式下进行了单例访问;");
            return Find();
        }

        /// <summary>
        /// 编辑模式下使用的模式;
        /// </summary>
        internal static T FindAllInEditor()
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

        /// <summary>
        /// 手动设置到单例,若出现错误则弹出异常;
        /// </summary>
        protected static void SetInstance(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            if (Instance != null && Instance != instance)
            {
                throw new ArgumentException("设置不同的单例;原本:" + (Instance as MonoBehaviour).name + ",请求:" + (instance as MonoBehaviour).name);
            }
            _instance = instance;
        }

        [ContextMenu("输出场景实例数目;")]
        protected void LogSingtonCount()
        {
            var instances = GameObject.FindObjectsOfType<T>();
            Debug.Log(instances.ToLog("场景存在单例"));
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}
