using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 只允许内部访问的,延迟实例化的单例;
    /// 主要是"限制多个实例化";
    /// </summary>
    public class UnitySington<T> : MonoBehaviour
        where T : UnitySington<T>
    {
        protected UnitySington() { }

        static T instance;

        protected static T GetInstance
        {
            get { return instance ?? Initialize(); }
        }

        static T Initialize()
        {
            UnityEngine.Object[] instances = GameObject.FindObjectsOfType(typeof(T));
            if (instances.Length == 0)
            {
                instance = GetSingletonGameObject().AddComponent<T>();
            }
            else if (instances.Length == 1)
            {
                instance = instances[0] as T;
            }
            else
            {
                instance = instances[0] as T;
                Debug.LogError(instances.ToLog("存在多个单例在场景!"));
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
            if (instance == null)
            {
                instance = Initialize();
                if (instance == this)
                    return;
            }

            Debug.LogWarning("尝试实例化多个单例;" + name);
            Destroy(this);
        }

    }

}
