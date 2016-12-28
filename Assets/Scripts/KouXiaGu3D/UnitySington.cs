using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 只允许内部访问的单例;
    /// </summary>
    public class UnitySington<T> : MonoBehaviour
        where T : UnitySington<T>
    {
        static UnitySington<T> instance;

        protected static T GetInstance
        {
            get { return (T)(instance ?? Initialize()); }
        }

        static UnitySington<T> Initialize()
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
                Debug.LogWarning(instances.ToLog("存在多个单例在场景!"));
            }
            return instance;
        }

        static GameObject GetSingletonGameObject()
        {
            GameObject singletonGameObject = new GameObject(typeof(T).Name);
            GameObject.DontDestroyOnLoad(singletonGameObject);
            return singletonGameObject;
        }

    }

}
