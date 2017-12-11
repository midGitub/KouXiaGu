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
        private static T instance;

        public UnitySingleton()
        {
        }

        /// <summary>
        /// 控制器标签;
        /// </summary>
        public abstract string ControllerTagName { get; }

        /// <summary>
        /// 获取到单例;
        /// </summary>
        public T GetInstance()
        {
            if (UnityThread.IsUnityThread)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    Debug.LogWarning("在编辑模式下进行了单例访问;");
                    return Find();
                }
#endif
                return instance ?? Find();
            }
            else
            {
                return instance;
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
                instance = sceneController.GetComponentInChildren<T>();
                return instance;
            }
            return null;
        }

        /// <summary>
        /// 设置到单例;
        /// </summary>
        public void SetInstance(T currentInstance)
        {
            if (currentInstance == null)
            {
                throw new ArgumentNullException(nameof(currentInstance));
            }
            else if (instance != null && instance != currentInstance)
            {
                string message = string.Format("重复设置单例{0},当前:{1},传入:{2}", nameof(T), instance, currentInstance);
                throw new InvalidOperationException(message);
            }
            else
            {
                instance = currentInstance;
            }
        }

        /// <summary>
        /// 当单例销毁时调用;
        /// </summary>
        public void RemoveInstance(T currentInstance)
        {
            if (currentInstance == null)
            {
                throw new ArgumentNullException(nameof(currentInstance));
            }
            if (instance != null && instance != currentInstance)
            {
                string message = string.Format("当前单例 {0} 不等于传入的 {1};", instance, currentInstance);
                throw new InvalidOperationException(message);
            }
            else
            {
                instance = null;
            }
        }
    }

    /// <summary>
    /// 挂载在场景控制器的单例(线程安全);
    /// </summary>
    public class SceneSingleton<T> : UnitySingleton<T>
         where T : MonoBehaviour
    {
        [CustomUnityTag("场景控制器;")]
        public const string SceneControllerTagName = "SceneController";

        public override string ControllerTagName
        {
            get { return SceneControllerTagName; }
        }
    }

    /// <summary>
    /// 挂载在全局控制器的单例(线程安全);
    /// </summary>
    public class GlobalSingleton<T> : UnitySingleton<T>
        where T : MonoBehaviour
    {
        [CustomUnityTag("全局控制器")]
        public const string GlobalControllerTagName = "GlobalController";

        public override string ControllerTagName
        {
            get { return GlobalControllerTagName; }
        }
    }
}
