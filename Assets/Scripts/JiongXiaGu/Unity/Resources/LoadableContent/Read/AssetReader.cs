using System;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 因为涉及到UnityAPI操作,所以都需要在Unity线程调用;
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
        /// 读取 Texture2D;
        /// </summary>
        public static Texture2D ReadAsTexture2D(this LoadableContent content, AssetInfo assetInfo)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            switch (assetInfo.From)
            {
                case LoadMode.AssetBundle:
                    return InternalFromAssetBundleReadTexture2D(content, assetInfo);

                case LoadMode.File:
                    return InternalFromFileReadTexture2D(content, assetInfo);

                default:
                    throw InvalidLoadModeException(nameof(Texture2D), assetInfo.From);
            }
        }

        /// <summary>
        /// 从 AssetBundle 读取 Texture2D
        /// </summary>
        private static Texture2D InternalFromAssetBundleReadTexture2D(LoadableContent content, AssetInfo assetInfo)
        {
            AssetBundle assetBundle = content.GetOrLoadAssetBundle(assetInfo.AssetBundleName);
            if (assetBundle != null)
            {
                var texture = assetBundle.LoadAsset<Texture2D>(assetInfo.Name);
                return texture;
            }
            return default(Texture2D);
        }

        /// <summary>
        /// 从 文件 读取 Texture2D;
        /// </summary>
        private static Texture2D InternalFromFileReadTexture2D(LoadableContent content, AssetInfo assetInfo)
        {
            lock (content.AsyncLock)
            {
                var stream = content.GetInputStream(assetInfo.Name);
                byte[] imageData = new byte[stream.Length];
                stream.Read(imageData, 0, (int)stream.Length);

                var texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                return texture;
            }
        }
    }
}
