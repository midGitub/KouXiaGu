using JiongXiaGu.Unity.Resources;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using ICSharpCode.SharpZipLib.Core;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 在程序一开始就存在的物体,保持该物体不随场景切换销毁;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GlobalController :MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        [ContextMenu("Test")]
        private void Test()
        {
            var chars = "知道AA啊부모".ToCharArray();
            Debug.Log(chars.ToText());
        }

        private async void Start()
        {
            await Task.Run(delegate ()
            {
                XiaGu.UnitySynchronizationContext.Post(_ => Debug.Log("0 : " + XiaGu.IsUnityThread), null);
                Debug.Log("1 : " + XiaGu.IsUnityThread);
            });
            Debug.Log("2 : " + XiaGu.IsUnityThread);
        }
    }
}
