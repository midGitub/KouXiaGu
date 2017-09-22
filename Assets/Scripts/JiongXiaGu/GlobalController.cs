using JiongXiaGu.Grids;
using JiongXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;
using JiongXiaGu.Globalization;
using System.Xml.Serialization;
using System.IO;
using JiongXiaGu.Diagnostics;
using JiongXiaGu.RectTerrain;
using JiongXiaGu.RectTerrain.Resources;
using JiongXiaGu.Resources.Archives;

namespace JiongXiaGu
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

            XiaGu.Initialize();
            Resource.Initialize();
        }

        public static T GetSington<T>()
        {
            return Instance.GetComponentInChildren<T>();
        }

        [ContextMenu("Test")]
        void Test()
        {
        }
    }
}
