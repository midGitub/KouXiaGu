using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 场景单例,为了允许多线程访问;
    /// </summary>
    [DisallowMultipleComponent]
    public class SceneSington<T> : MonoBehaviour
        where T : SceneSington<T>
    {
        static T _instance;

        /// <summary>
        /// 获取到单例,允许多线程访问,若不存在则为Null;
        /// </summary>
        public static T Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        /// <summary>
        /// 是否已经实例化?
        /// </summary>
        public static bool IsInitialized
        {
            get { return _instance != null; }
        }

        /// <summary>
        /// 手动设置到单例,若出现错误则弹出异常;
        /// </summary>
        protected static void SetInstance(T instance)
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

        protected virtual void Awake()
        {
            SetInstance((T)this);
        }

        protected virtual void OnDestroy()
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
