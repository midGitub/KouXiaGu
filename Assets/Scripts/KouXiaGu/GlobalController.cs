using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;
using KouXiaGu.OperationRecord;
using System.Reflection;

namespace KouXiaGu
{

    /// <summary>
    /// 在程序一开始就存在的物体,保持该物体不随场景切换销毁;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GlobalController : UnitySington<GlobalController>
    {   
        [CustomUnityTag("全局控制器标签;")]
        public const string Tag = "GlobalController";

        void Awake()
        {
            SetInstance(this);
            tag = Tag;
            DontDestroyOnLoad(gameObject);
            XiaGu.Initialize();
            Resource.Initialize();
        }

        [ContextMenu("Test")]
        void Test()
        {
            Type type = typeof(ListAdd<int>);
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            Debug.Log(methods.Length);
            foreach (var method in methods)
            {
                Debug.Log(method.Name);
            }
        }
    }
}
