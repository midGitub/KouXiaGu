using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 延迟实例化的单例;
    /// 限制一个场景多个实例;
    /// </summary>
    [Obsolete]
    public class OSceneSington<T> : MonoBehaviour
        where T : OSceneSington<T>
    {
        protected OSceneSington() { }

        static T instance;

        public static T GetInstance
        {
            get
            {
                return instance ?? (instance = Initialize());
//#if UNITY_EDITOR
//                return Initialize();
//#else
//                return instance ?? (instance = Initialize());
//#endif
            }
        }


        /// <summary>
        /// 是否已经实例化?
        /// </summary>
        public static bool IsInstanced
        {
            get { return instance != null; }
        }

        /// <summary>
        /// 若未实例化则返回异常;
        /// </summary>
        protected static void NotInstancedException()
        {
            if (!IsInstanced)
                throw new ArgumentNullException("类未实例化");
        }


        static readonly object asyncObject = new object();

        static T Initialize()
        {
            lock (asyncObject)
            {
                if (instance == null)
                {
                    UnityEngine.Object[] instances = GameObject.FindObjectsOfType(typeof(T));
                    if (instances.Length == 0)
                    {
                        Debug.LogError("场景不存在实例;" + typeof(T).Name);
                        throw new ObjectDisposedException("场景不存在实例;" + typeof(T).Name);
                    }
                    else if (instances.Length == 1)
                    {
                        instance = (T)instances[0];
                    }
                    else
                    {
                        instance = (T)instances[0];
                        Debug.LogError(instances.ToLog("存在多个单例在场景!"));
                    }
                }
            }
            return instance;
        }

        protected virtual void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.LogWarning(GetType().Name + ";尝试实例化多个单例;" + name + ";Instanced:" + IsInstanced + "," + (instance == this));
                Destroy(this);
                return;
            }
            instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }


        [ContextMenu("实例存在数")]
        protected void OutputCount()
        {
            var instances = GameObject.FindObjectsOfType(typeof(T));
            Debug.Log("实例数量:" + instances.Length);
        }

    }

}
