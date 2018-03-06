//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace JiongXiaGu.Unity
//{

//    /// <summary>
//    /// AssetBundle 拓展方法;
//    /// </summary>
//    public static class AssetBundleHelper
//    {

//        /// <summary>
//        /// 异步读取到 AssetBundle;
//        /// </summary>
//        /// <exception cref="InvalidOperationException"></exception>
//        /// <exception cref="IOException"></exception>
//        public static Task<AssetBundle> LoadAssetBundleAsync(string filePath, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            UnityThread.ThrowIfNotUnityThread();
//            cancellationToken.ThrowIfCancellationRequested();

//            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(filePath);
//            var taskCompletionSource = new TaskCompletionSource<AssetBundle>();

//            request.completed += delegate (AsyncOperation asyncOperation)
//            {
//                var assetBundle = request.assetBundle;
//                if (assetBundle == null)
//                {
//                    taskCompletionSource.SetException(new IOException("无法读取到 AssetBundle;"));
//                }
//                else
//                {
//                    if (cancellationToken.IsCancellationRequested)
//                    {
//                        assetBundle.Unload(true);
//                        taskCompletionSource.SetCanceled();
//                    }
//                    else
//                    {
//                        taskCompletionSource.SetResult(assetBundle);
//                    }
//                }
//            };

//            return taskCompletionSource.Task;
//        }



//        /// <summary>
//        /// 异步读取到 AssetBundle;
//        /// </summary>
//        /// <exception cref="InvalidOperationException"></exception>
//        /// <exception cref="IOException"></exception>
//        [Obsolete]
//        public static Task<AssetBundle> LoadAssetBundleAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken), bool onFailCloseStream = true)
//        {
//            UnityThread.ThrowIfNotUnityThread();
//            if (stream == null)
//            {
//                throw new ArgumentNullException(nameof(stream));
//            }
//            if (cancellationToken.IsCancellationRequested)
//            {
//                if (onFailCloseStream)
//                {
//                    stream.Dispose();
//                }
//                throw new OperationCanceledException();
//            }

//            AssetBundleCreateRequest request = AssetBundle.LoadFromStreamAsync(stream);
//            var taskCompletionSource = new TaskCompletionSource<AssetBundle>();

//            request.completed += delegate (AsyncOperation asyncOperation)
//            {
//                var assetBundle = request.assetBundle;
//                if (assetBundle == null)
//                {
//                    if (onFailCloseStream)
//                    {
//                        stream.Dispose();
//                    }
//                    taskCompletionSource.SetException(new IOException("无法读取到 AssetBundle;"));
//                }
//                else
//                {
//                    if (cancellationToken.IsCancellationRequested)
//                    {
//                        assetBundle.Unload(true);
//                        if (onFailCloseStream)
//                        {
//                            stream.Dispose();
//                        }
//                        taskCompletionSource.SetCanceled();
//                    }
//                    else
//                    {
//                        taskCompletionSource.SetResult(assetBundle);
//                    }
//                }
//            };

//            return taskCompletionSource.Task;
//        }

//    }
//}
