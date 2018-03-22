using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity
{
    /// <summary>
    /// 对 Unity.AsyncOperation 类进行包装;
    /// </summary>
    public static class UnityAsyncOperationExtensions
    {
        /// <summary>
        /// 转换成 Task 格式;
        /// </summary>
        public static Task AsTask(this AsyncOperation asyncOperation)
        {
            if (asyncOperation == null)
                throw new ArgumentNullException(nameof(asyncOperation));
            if (asyncOperation.isDone)
                return Task.CompletedTask;

            var taskCompletionSource = new TaskCompletionSource<object>();
            asyncOperation.completed += delegate (AsyncOperation operation)
            {
                taskCompletionSource.SetResult(null);
            };

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// 转换成 Task 格式;
        /// </summary>
        public static Task<AssetBundle> AsTask(this AssetBundleCreateRequest assetBundleCreateRequest)
        {
            const string ExceptionMessage = "无法读取到 AssetBundle;";

            if (assetBundleCreateRequest == null)
                throw new ArgumentNullException(nameof(assetBundleCreateRequest));

            if (assetBundleCreateRequest.isDone)
            {
                var assetBundle = assetBundleCreateRequest.assetBundle;
                if (assetBundle == null)
                {
                    return Task.FromException<AssetBundle>(new IOException(ExceptionMessage));
                }
                else
                {
                    return Task.FromResult(assetBundleCreateRequest.assetBundle);
                }
            }

            var taskCompletionSource = new TaskCompletionSource<AssetBundle>();
            assetBundleCreateRequest.completed += delegate (AsyncOperation operation)
            {
                var assetBundle = assetBundleCreateRequest.assetBundle;
                if (assetBundle == null)
                {
                    taskCompletionSource.SetException(new IOException(ExceptionMessage));
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
