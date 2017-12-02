using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    public static class ContentReadTool
    {

        /// <summary>
        /// 获取到贴图,若未能获取到则返回失败的Task;
        /// 允许在任何线程调用,
        /// 若为AssetBundle,在Unity线程则为同步的读取到,在其它线程则返回异步操作;
        /// 若为File,都为异步操作;
        /// </summary>
        public static Task<Texture2D> ReadAsTexture(this LoadableContent content, AssetInfo assetInfo)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            switch (assetInfo.From)
            {
                case LoadMode.AssetBundle:
                    return InternalFromAssetBundleAsTexture(content, assetInfo.Name);

                case LoadMode.File:
                    return InternalFromFileAsTexture(content, assetInfo.Name);

                default:
                    throw new ArgumentException(nameof(LoadMode));
            }
        }

        private static Task<Texture2D> InternalFromAssetBundleAsTexture(LoadableContent content, string name)
        {
            return TaskInUnityThread(delegate ()
            {
                AssetBundle assetBundle = content.GetAssetBundle();
                if (assetBundle != null)
                {
                    var texture = assetBundle.LoadAsset<Texture2D>(name);
                    return texture;
                }
                return default(Texture2D);
            });
        }

        private static async Task<Texture2D> InternalFromFileAsTexture(LoadableContent content, string name)
        {
            byte[] imageData = null;

            await Task.Run(delegate ()
            {
                ILoadableEntry entry = content.GetEntry(name);
                var stream = content.GetInputStream(entry);
                imageData = new byte[stream.Length];
                stream.Read(imageData, 0, (int)stream.Length);
            });

            var t = await TaskInUnityThread(delegate ()
            {
                var texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                return texture;
            });

            return t;
        }

        /// <summary>
        /// 在Unity线程执行;
        /// 若在其它线程调用,则转到Unity线程执行,返回异步Task;
        /// 若在Unity线程调用,则同步执行;
        /// </summary>
        private static Task<T> TaskInUnityThread<T>(Func<T> func)
        {
            if (XiaGu.IsUnityThread)
            {
                T t = func.Invoke();
                return Task.FromResult(t);
            }
            else
            {
                var task = TaskHelper.Run(func, XiaGu.UnityTaskScheduler);
                return task;
            }
        }
    }
}
