using System.Diagnostics;
using UnityEngine;

namespace XGame
{

    /// <summary>
    /// 全局组件;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Controller<T> : MonoBehaviour
        where T : Controller<T>
    {

        protected Controller() { }

        /// <summary>
        /// 实例;
        /// </summary>
        protected static T instance = null;

        /// <summary>
        /// 获取到this;
        /// </summary>
        protected abstract T This { get; }

        /// <summary>
        /// 返回实例;若不存在,返回NULL;
        /// </summary>
        public static T GetInstance
        {
            get
            {
#if UNITY_EDITOR
                if (instance == null)
                    UnityEngine.Debug.LogError("该场景未初始化此类!\n" + new StackTrace().ToString());
#endif
                return instance;
            }
        }

        protected virtual void Awake()
        {
            instance = This;
        }

    }

}
