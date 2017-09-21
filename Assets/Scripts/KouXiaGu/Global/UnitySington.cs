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

        [CustomUnityTag("全局控制器")]
        public const string GlobalControllerTagName = "GlobalController";

        static T _instance;

        /// <summary>
        /// 获取到单例,仅Unity线程访问;
        /// </summary>
        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!XiaGu.IsPlaying)
                {
                    Debug.LogWarning("在编辑模式下进行了单例访问;");
                    return Find_safe();
                }
#endif
                return _instance ?? Find_safe();
            }
            private set { _instance = value; }
        }

        /// <summary>
        /// 是否已经实例化?
        /// </summary>
        public static bool IsInitialized
        {
            get { return _instance != null; }
        }

        static T Find_safe()
        {
            if (XiaGu.IsUnityThread)
            {
                _instance = Find();
                return _instance;
            }
            return null;
        }

        static T Find()
        {
            GameObject sceneController = GameObject.FindWithTag(GlobalControllerTagName);
            if (sceneController != null)
            {
                T instance = sceneController.GetComponentInChildren<T>();
                return instance;
            }
            return null;
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
            if (_instance != null && _instance != instance)
            {
                throw new ArgumentException("设置不同的单例;原本:" + (_instance as MonoBehaviour).name + ",请求:" + (instance as MonoBehaviour).name);
            }
            else
            {
                _instance = instance;
            }
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}
