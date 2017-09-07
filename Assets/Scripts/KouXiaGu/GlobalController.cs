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
            var serializer = new XmlFileSerializer<LandformResource[]>();
            LandformResource[] array = new LandformResource[]
                {
                    new LandformResource()
                    {
                        DiffuseTex = new TextureInfo("None"),
                        DiffuseBlendTex = new TextureInfo("None"),
                        HeightTex = new TextureInfo("None"),
                        HeightBlendTex = new TextureInfo("None"),
                    },
                };
            serializer.Write(array, @"11.xml", FileMode.Create);
        }
    }
}
