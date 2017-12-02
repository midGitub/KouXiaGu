using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 因为涉及到UnityAPI操作,所以都返回异步Task,并且方法都支持其它线程调用;
    /// 对于所有方法都有:
    /// 在Unity线程调用则为同步操作,返回值可直接取 Task.Result,若出现异常,则会返回失败状态的Task;
    /// 在其它线程调用也为同步操作,但是涉及UnityAPI的操作会转到Unity线程执行,除了特别标注,否则需要等待执行完毕才能取值;
    /// </summary>
    public static class AssetReader
    {

        /// <summary>
        /// 不支持读取方式异常;
        /// </summary>
        private static Exception InvalidLoadModeException(string type, LoadMode mode)
        {
            throw new InvalidOperationException(string.Format("[{0}]不支持通过[{1}]读取;", type, mode));
        }

        /// <summary>
        /// 在Unity线程执行;
        /// 若在其它线程调用,则转到Unity线程执行,返回异步操作的Task;
        /// 若在Unity线程调用,则同步操作;
        /// </summary>
        private static Task<T> TaskInUnityThread<T>(Func<T> func, CancellationToken token)
        {
            if (XiaGu.IsUnityThread)
            {
                try
                {
                    T t = func.Invoke();
                    return Task.FromResult(t);
                }
                catch (Exception ex)
                {
                    return Task.FromException<T>(ex);
                }
            }
            else
            {
                var task = TaskHelper.Run(func, token, XiaGu.UnityTaskScheduler);
                return task;
            }
        }


        /// <summary>
        /// 读取 Texture2D;
        /// </summary>
        public static Task<Texture2D> ReadAsTexture2D(this LoadableContent content, AssetInfo assetInfo, CancellationToken token = default(CancellationToken))
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            token.ThrowIfCancellationRequested();

            switch (assetInfo.From)
            {
                case LoadMode.AssetBundle:
                    return InternalFromAssetBundleAsTexture2D(content, assetInfo.Name, token);

                case LoadMode.File:
                    return InternalFromFileAsTexture2D(content, assetInfo.Name, token);

                default:
                    throw InvalidLoadModeException(nameof(Texture2D), assetInfo.From);
            }
        }

        /// <summary>
        /// 从 AssetBundle 读取 Texture2D
        /// </summary>
        private static Task<Texture2D> InternalFromAssetBundleAsTexture2D(LoadableContent content, string name, CancellationToken token)
        {
            return TaskInUnityThread(delegate ()
            {
                token.ThrowIfCancellationRequested();
                AssetBundle assetBundle = content.GetAssetBundle();
                if (assetBundle != null)
                {
                    var texture = assetBundle.LoadAsset<Texture2D>(name);
                    return texture;
                }
                return default(Texture2D);
            }, token);
        }

        /// <summary>
        /// 从 文件 读取 Texture2D;
        /// </summary>
        private static Task<Texture2D> InternalFromFileAsTexture2D(LoadableContent content, string name, CancellationToken token)
        {
            try
            {
                ILoadableEntry entry = content.GetEntry(name);
                var stream = content.GetInputStream(entry);
                byte[] imageData = new byte[stream.Length];
                stream.Read(imageData, 0, (int)stream.Length);

                return TaskInUnityThread(delegate ()
                {
                    var texture = new Texture2D(2, 2);
                    texture.LoadImage(imageData);
                    return texture;
                }, token);
            }
            catch (Exception ex)
            {
                return Task.FromException<Texture2D>(ex);
            }
        }
    }
}
