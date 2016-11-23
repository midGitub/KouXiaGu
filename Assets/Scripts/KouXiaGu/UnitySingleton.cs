using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    public class UnitySingleton<T> : MonoBehaviour
        where T : UnitySingleton<T>
    {

        const string singletonGameObjectTag = "Singleton";
        static readonly object syncRoot = new object();
        static T instance;

        public static T GetInstance
        {
            get{ return instance ?? Initialize(); }
        }

        static T Initialize()
        {
            lock (syncRoot)
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(T)) as T;
                    if (instance == null)
                    {
                        instance = GetSingletonGameObject().AddComponent<T>();
                    }
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

        //protected virtual void Awake()
        //{
        //    //GameObject.DontDestroyOnLoad(gameObject);
        //}

    }

}
