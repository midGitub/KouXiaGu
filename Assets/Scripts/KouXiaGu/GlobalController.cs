﻿using KouXiaGu.Grids;
using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;
using KouXiaGu.Globalization;
using System.Xml.Serialization;
using System.IO;
using KouXiaGu.Diagnostics;
using KouXiaGu.RectTerrain;
using KouXiaGu.RectTerrain.Resources;
using KouXiaGu.Resources.Archives;

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

        static GameObject globalController;

        void Awake()
        {
            SetInstance(this);
            globalController = gameObject;
            tag = Tag;
            DontDestroyOnLoad(gameObject);

            XiaGu.Initialize();
            Resource.Initialize();
        }

        public static T GetSington<T>()
        {
            return globalController.GetComponentInChildren<T>();
        }

        [ContextMenu("Test")]
        void Test()
        {

        }
    }
}
