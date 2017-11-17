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
            Debug.Log(GetFileName("123123/ddaeqwe"));
            Debug.Log(GetFileName("123123/ddaeqwe/11.text"));
            Debug.Log(GetFileName("123123/ddaeqwe/"));
        }

        private static char[] DirectorySeparatorChars = new char[] { '/', '\\' };

        /// <summary>
        /// 获取到文件路径;
        /// </summary>
        private string GetFileName(string path)
        {
            int i = path.LastIndexOfAny(DirectorySeparatorChars);
            if (i == path.Length - 1)
            {
                throw new ArgumentException(string.Format("路径[{0}]不为文件路径", path));
            }
            else
            {
                path = path.Remove(0, i + 1);
                return path;
            }
        }
    }
}
