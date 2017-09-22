using JiongXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu
{

    /// <summary>
    /// 场景单例;
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class SceneSington<T> : MonoBehaviour
        where T : SceneSington<T>
    {

        [CustomUnityTag("场景控制器;")]
        public const string SceneControllerTagName = "SceneController";

        static T _instance;

        /// <summary>
        /// 获取到单例(允许多线程访问,但是未在Unity线程设置到单例,永远返回Null);
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
            GameObject sceneController = GameObject.FindWithTag(SceneControllerTagName);
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
        protected void SetInstance(T instance)
        {
            if (instance == null)
            {
                Debug.LogError("传入参数 instance 为 Null;");
            }
            else if (_instance != null && _instance != instance)
            {
                Debug.LogError("设置不同的单例;原本:" + (_instance as MonoBehaviour).name + ",请求:" + (instance as MonoBehaviour).name);
            }
            else
            {
                Instance = instance;
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance != null && Instance != this)
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
