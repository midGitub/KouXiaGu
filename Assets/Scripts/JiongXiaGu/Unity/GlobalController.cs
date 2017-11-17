using JiongXiaGu.Unity.Resources;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using ICSharpCode.SharpZipLib.Core;
using System;

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
            var path1 = new Uri(new DirectoryInfo("F:\\My_Code\\Unity5\\KouXiaGu/Assets/Scenes/1.text").FullName);
            var path2 = new Uri(new DirectoryInfo("F:\\My_Code\\Unity5\\KouXiaGu/Assets/Scenes/").FullName);

            Debug.Log(path1.MakeRelativeUri(path2));
        }
    }
}
