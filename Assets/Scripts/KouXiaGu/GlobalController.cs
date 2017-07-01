using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;
using KouXiaGu.Globalization;
using System.Xml.Serialization;
using System.IO;

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

            LanguagePackXmlSerializer serializer = new LanguagePackXmlSerializer();
            LanguagePackFilePath file = new LanguagePackFilePath(pack);

            using (Stream stream = file.LoadStream())
            {
                serializer.Serialize(pack, stream);
            }

            LanguagePackXmlSearcher searcher = new LanguagePackXmlSearcher();
            var packs = searcher.EnumeratePacks();

            foreach (var packItem in packs)
            {
                Debug.Log(packItem.ToString());
            }
        }
    }
}
