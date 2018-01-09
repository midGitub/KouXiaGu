using System;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 非继承形式的Unity单例(线程安全);
    /// </summary>
    public abstract class UnitySingleton<T>
        where T : MonoBehaviour
    {
        /// <summary>
        /// 静态单例;
        /// </summary>
        private volatile static T instance;

        /// <summary>
        /// 控制器标签;
        /// </summary>
        public abstract string ControllerTagName { get; }

        private static readonly object asyncLock = new object();

        /// <summary>
        /// 获取到单例;
        /// </summary>
        public T GetInstance()
        {
            if (instance != null)
                return instance;

            lock (asyncLock)
            {
                if (UnityThread.IsUnityThread)
                {
#if UNITY_EDITOR
                    if (!UnityThread.IsPlaying)
                    {
                        Debug.LogWarning("在编辑模式下进行了单例访问;");
                        return Find();
                    }
#endif
                    return instance ?? (instance = Find());
                }
                else
                {
#if UNITY_EDITOR
                    if (!UnityThread.IsPlaying)
                    {
                        Debug.LogWarning("在编辑模式下进行了单例访问;");
                        return UnityThread.RunInUnityThread(Find).Result;
                    }
#endif
                    return instance ?? (instance = UnityThread.RunInUnityThread(Find).Result);
                }
            }
        }

        /// <summary>
        /// 获取到单例;
        /// </summary>
        private T Find()
        {
            GameObject sceneController = GameObject.FindWithTag(ControllerTagName);
            if (sceneController != null)
            {
                var instance = sceneController.GetComponentInChildren<T>();
                return instance;
            }
            return null;
        }

        /// <summary>
        /// 设置到单例;
        /// </summary>
        public void SetInstance(T newInstance)
        {
            lock (asyncLock)
            {
                if (newInstance == null)
                {
                    throw new ArgumentNullException(nameof(newInstance));
                }
                else if (instance != null && instance != newInstance)
                {
                    string message = string.Format("重复设置单例{0},当前:{1},传入:{2}", nameof(T), instance, newInstance);
                    throw new InvalidOperationException(message);
                }
                else
                {
                    instance = newInstance;
                }
            }
        }

        /// <summary>
        /// 当单例销毁时调用;
        /// </summary>
        public void RemoveInstance(T newInstance)
        {
            lock (asyncLock)
            {
                if (newInstance == null)
                {
                    throw new ArgumentNullException(nameof(newInstance));
                }
                if (instance != null && instance != newInstance)
                {
                    string message = string.Format("当前单例 {0} 不等于传入的 {1};", instance, newInstance);
                    throw new InvalidOperationException(message);
                }
                else
                {
                    instance = null;
                }
            }
        }
    }

    /// <summary>
    /// 挂载在场景控制器的单例(线程安全);
    /// </summary>
    public class SceneSingleton<T> : UnitySingleton<T>
         where T : MonoBehaviour
    {
        public override string ControllerTagName => UnityTagDefinition.SceneController.ToString();
    }

    /// <summary>
    /// 挂载在全局控制器的单例(线程安全);
    /// </summary>
    public class GlobalSingleton<T> : UnitySingleton<T>
        where T : MonoBehaviour
    {
        public override string ControllerTagName => UnityTagDefinition.GlobalController.ToString();
    }
}
