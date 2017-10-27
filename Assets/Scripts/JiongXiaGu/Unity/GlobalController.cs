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
            string str = "0123456789";
            Debug.Log(str.IndexOf("23"));
            Debug.Log(str.Substring(4));
        }
    }
}
