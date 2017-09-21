using KouXiaGu.Grids;
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
            new Archive(new ArchiveInfo("鸡腿")).Create();
            new Archive(new ArchiveInfo("鸡腿1")).Create();
            new Archive(new ArchiveInfo("鸡腿2")).Create();
            new Archive(new ArchiveInfo("鸡腿3")).Create();

            Debug.Log(Archive.EnumerateArchives().ToLog());
        }
    }
}
