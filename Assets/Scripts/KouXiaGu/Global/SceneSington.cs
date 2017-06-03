using KouXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 场景单例;
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class SceneSington<T> : MonoBehaviour
        where T : SceneSington<T>
    {

        [CustomUnityTag]
        public const string DefaultTagName = "SceneController";

        static T _instance;
        static readonly object asyncLock = new object();

        /// <summary>
        /// 获取到单例,允许多线程访问,若不存在则为Null;
        /// </summary>
        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!XiaGu.IsPlaying)
                {
                    throw new ArgumentException("尝试在编辑器模式下访问单例;");
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
            lock (asyncLock)
            {
                if (_instance == null)
                {
                    if (XiaGu.IsMainThread)
                    {
                        Instance = Find();
                    }
                    else
                    {
                        Operate operate = new Operate(() => Instance = Find());
                        UnityAsyncRequestDispatcher.Instance.AddQueue(operate);
                        while (!operate.IsCompleted)
                        {
                        }
                    }
                }
                return Instance;
            }
        }

        static T Find()
        {
            GameObject sceneController = GameObject.FindWithTag(DefaultTagName);
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
            lock (asyncLock)
            {
                if (instance == null)
                {
                    Debug.LogError("传入参数 instance 为 Null;");
                }
                else if (Instance != null && Instance != instance)
                {
                    Debug.LogError("设置不同的单例;原本:" + (Instance as MonoBehaviour).name + ",请求:" + (instance as MonoBehaviour).name);
                }
                else
                {
                    Instance = instance;
                }
            }
        }

        protected virtual void OnDestroy()
        {
            lock (asyncLock)
            {
                if (Instance == null)
                {
                    Debug.LogError("场景单例不为即将销毁的;");
                }
                else
                {
                    Instance = null;
                }
            }
        }
    }
}
