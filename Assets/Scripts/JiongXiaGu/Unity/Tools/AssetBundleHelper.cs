using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// AssetBundle 拓展方法;
    /// </summary>
    public static class AssetBundleHelper
    {

        /// <summary>
        /// 异步读取到 AssetBundle;
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IOException"></exception>
        public static Task<AssetBundle> LoadAssetBundleAsync(Stream stream)
        {
            UnityThread.ThrowIfNotUnityThread();

            var taskCompletionSource = new TaskCompletionSource<AssetBundle>();

            AssetBundleCreateRequest request = AssetBundle.LoadFromStreamAsync(stream);
            request.completed += delegate (AsyncOperation asyncOperation)
            {
                var assetBundle = request.assetBundle;
                if (assetBundle == null)
                {
                    stream.Dispose();
                    taskCompletionSource.SetException(new IOException("无法读取到 AssetBundle;"));
                }
                else
                {
                    taskCompletionSource.SetResult(assetBundle);
                }
            };

            return taskCompletionSource.Task;
        }

    }
}
