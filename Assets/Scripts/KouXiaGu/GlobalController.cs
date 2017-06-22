using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;

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
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            ThreadPool.QueueUserWorkItem(delegate (object state)
            {
                for (int i = 0; i < 10000; i++)
                {
                    dictionary.Add(i, i);
                    ////Thread.Sleep(200);
                }
            });

            ThreadPool.QueueUserWorkItem(delegate (object state)
            {
                for (int i = 0; i < 10; i++)
                {
                    Debug.Log(dictionary.Keys.Count);
                    //Thread.Sleep(400);
                }
            });
        }
    }
}
