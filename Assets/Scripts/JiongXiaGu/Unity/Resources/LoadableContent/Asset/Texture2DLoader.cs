using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    internal class Texture2DLoader : AssetLoader<Texture2D>
    {
        public static Texture2DLoader Default { get; private set; } = new Texture2DLoader();

        private static readonly AssetLoadModes[] supportedLoadModes = new AssetLoadModes[]
            {
                AssetLoadModes.AssetBundle,
                AssetLoadModes.File,
            };

        public override IReadOnlyList<AssetLoadModes> SupportedLoadModes
        {
            get { return supportedLoadModes; }
        }

        public override Texture2D Load(LoadableContent content, AssetInfo assetInfo)
        {
            UnityThread.ThrowIfNotUnityThread();
            ThrowIfNotSupportedLoadMode(assetInfo.From);
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            switch (assetInfo.From)
            {
                case AssetLoadModes.AssetBundle:
                    return InternalFromAssetBundleReadTexture2D(content, assetInfo);

                case AssetLoadModes.File:
                    return InternalFromFileReadTexture2D(content, assetInfo);

                default:
                    throw NotSupportedLoadModeException(assetInfo.From);
            }
        }

        /// <summary>
        /// 从 AssetBundle 读取 Texture2D
        /// </summary>
        private static Texture2D InternalFromAssetBundleReadTexture2D(LoadableContent content, AssetInfo assetInfo)
        {
            AssetBundle assetBundle = content.GetOrLoadAssetBundle(assetInfo.AssetBundleName);
            var texture = assetBundle.LoadAsset<Texture2D>(assetInfo.Name.Name);
            if (texture == null)
            {
                throw new ArgumentException(string.Format("在AssetBundle[{0}]内未找到资源[{1}]", assetInfo.AssetBundleName, assetInfo.Name));
            }
            else
            {
                return texture;
            }
        }

        /// <summary>
        /// 从 文件 读取 Texture2D;
        /// </summary>
        private static Texture2D InternalFromFileReadTexture2D(LoadableContent content, AssetInfo assetInfo)
        {
            var stream = content.GetInputStream(assetInfo.Name.Name);
            byte[] imageData = new byte[stream.Length];
            stream.Read(imageData, 0, (int)stream.Length);

            var texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            return texture;
        }
    }
}