using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.KeyInputs;
using System;
using System.IO;
using UnityEngine;
using JiongXiaGu.Grids;
using System.Reflection;
using System.Collections.Generic;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 在程序一开始就存在的物体,保持该物体不随场景切换销毁;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GlobalController : UnitySington<GlobalController>
    {   
        void Awake()
        {
            SetInstance(this);
            DontDestroyOnLoad(gameObject);
        }

        public static T GetSington<T>()
        {
            return Instance.GetComponentInChildren<T>();
        }

        [ContextMenu("Test")]
        void Test()
        {
            var methodInfo = typeof(GlobalController).GetMethod("TTT", BindingFlags.Public | BindingFlags.Static);
            Action action = CreateDelegate<Action>(methodInfo, null);
            action.Invoke();
        }

        /// <summary>
        /// 转换成委托;
        /// </summary>
        private static T CreateDelegate<T>(MethodInfo methodInfo, System.Object target)
            where T : class
        {
            if (target == null)
            {
                return methodInfo.CreateDelegate(typeof(T)) as T;
            }
            else
            {
                return methodInfo.CreateDelegate(typeof(T), target) as T;
            }
        }

        public static void TTT()
        {
            Debug.Log("成功!");
        }
    }
}
