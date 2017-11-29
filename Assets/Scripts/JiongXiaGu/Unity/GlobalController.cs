using JiongXiaGu.Unity.Resources;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using ICSharpCode.SharpZipLib.Core;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;

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
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            using (var stream = new FileStream(@"1.zip", FileMode.Open, FileAccess.Read))
            {
                var md5 = mD5CryptoServiceProvider.ComputeHash(stream);
                string md5Str = string.Join(string.Empty, md5);
                Debug.Log("0 : " + md5Str);

                ZipFile zipFile = new ZipFile(stream);

                stream.Seek(0, SeekOrigin.Begin);
                md5 = mD5CryptoServiceProvider.ComputeHash(stream);
                md5Str = string.Join(string.Empty, md5);
                Debug.Log("1 : " + md5Str);
            }
        }

        //private async Task Update()
        //{
        //    await Task.Run(delegate ()
        //    {
        //        XiaGu.UnitySynchronizationContext.Post(_ => Debug.Log("0 : " + XiaGu.IsUnityThread), null);
        //        Debug.Log("1 : " + XiaGu.IsUnityThread);
        //    });
        //    Debug.Log("2 : " + XiaGu.IsUnityThread);
        //}
    }
}
