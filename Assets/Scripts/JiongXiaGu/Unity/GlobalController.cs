using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.KeyInputs;
using System.IO;
using UnityEngine;
using JiongXiaGu.Grids;
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
            Debug.Log(string.Format("{0}  {1}", typeof(List<int>).GetHashCode() , typeof(List<string>).GetHashCode()));
        }
    }
}
