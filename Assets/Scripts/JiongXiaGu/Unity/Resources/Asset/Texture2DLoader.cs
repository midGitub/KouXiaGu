using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

        public override IReadOnlyList<AssetLoadModes> SupportedLoadModes => supportedLoadModes;

        public override Task<Texture2D> LoadAsync(Modification content, AssetInfo assetInfo, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从 AssetBundle 读取 Texture2D;
        /// </summary>
        public override Texture2D Load(Modification content, AssetInfo assetInfo)
        {
            AssetBundle assetBundle = LoadableResource.SharedContent.GetAssetBundle(content, assetInfo.From);
            var texture = assetBundle.LoadAsset<Texture2D>(assetInfo.Name);
            if (texture == null)
            {
                throw new ArgumentException(string.Format("在AssetBundle[{0}]内未找到资源[{1}]", assetInfo.From, assetInfo.Name));
            }
            else
            {
                return texture;
            }
        }

        //private static Texture2D FromAssetBundleLoadTexture2DAsync(LoadableContent content, AssetInfo assetInfo)
        //{
        //    AssetBundle assetBundle = LoadableResource.SharedContent.GetAssetBundle(content, assetInfo.BundleName);
        //    var texture = assetBundle.LoadAssetAsync<Texture2D>(assetInfo.Name.Name);

        //}

        /// <summary>
        /// 从 文件 读取 Texture2D;
        /// </summary>
        private static Texture2D FromFileLoadTexture2D(Modification content, AssetInfo assetInfo)
        {
            var stream = LoadableResource.SharedContent.GetInputStream(content, assetInfo.Name);
            byte[] imageData = new byte[stream.Length];
            stream.Read(imageData, 0, (int)stream.Length);

            var texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            return texture;
        }
    }
}