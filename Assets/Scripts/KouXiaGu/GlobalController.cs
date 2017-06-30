using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;
using KouXiaGu.OperationRecord;
using System.Reflection;
using KouXiaGu.Globalization;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;

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
            LanguagePack pack = new LanguagePack("www", "Chinese")
            {
                TextDictionary = new Dictionary<string, string>()
                {
                    { "1", "111" },
                    { "2", "211" },
                    { "3", "311" },
                },
            };

            XmlSerializer serializer = new XmlSerializer(typeof(LanguagePack));
            serializer.SerializeXiaGu(pack, Path.Combine(Application.streamingAssetsPath, "Localization/test.xml"));
            Debug.Log(Application.systemLanguage);
        }
    }
}
