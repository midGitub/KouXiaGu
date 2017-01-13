using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 只允许内部访问的,延迟实例化的单例;
    /// 主要是"限制一个场景多个实例";
    /// </summary>
    public class SceneSington<T> : MonoBehaviour
        where T : SceneSington<T>
    {
        protected SceneSington() { }

        static T instance;

        protected static T GetInstance
        {
            get
            {
#if UNITY_EDITOR
                return Initialize();
#else
                return instance ?? (instance = Initialize());
#endif
            }
        }

        protected bool IsInstanced
        {
            get { return instance != null; }
        }

        static T Initialize()
        {
            if (instance == null)
            {
                UnityEngine.Object[] instances = GameObject.FindObjectsOfType(typeof(T));
                if (instances.Length == 0)
                {
                    instance = GetSingletonGameObject().AddComponent<T>();
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
            return instance;
        }

        static GameObject GetSingletonGameObject()
        {
            GameObject singletonGameObject = new GameObject(typeof(T).Name);
            GameObject.DontDestroyOnLoad(singletonGameObject);
            return singletonGameObject;
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
