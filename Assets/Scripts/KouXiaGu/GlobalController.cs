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
            LogRecorder recorder = new LogRecorder(10);
            recorder.Log("今年过节不收礼,收礼只收脑白金;");
            Debug.Log(recorder.GetText());
            recorder.Log("又喝成长快了啦!");
            Debug.Log(recorder.GetText());
        }
    }
}
