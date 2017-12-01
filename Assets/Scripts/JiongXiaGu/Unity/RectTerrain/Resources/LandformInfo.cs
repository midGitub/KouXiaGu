using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain.Resources
{

    /// <summary>
    /// 地形资源;
    /// </summary>
    public class LandformInfo
    {
        public LoadableContent LoadableContent { get; private set; }
        public LandformDescription Description { get; private set; }
        public Texture2D HeightTex { get; private set; }
        public Texture2D HeightBlendTex { get; private set; }
        public Texture2D DiffuseTex { get; private set; }
        public Texture2D DiffuseBlendTex { get; private set; }

        public LandformInfo(LoadableContent loadableContent, LandformDescription description)
        {
            if (loadableContent == null)
                throw new ArgumentNullException(nameof(loadableContent));

            LoadableContent = loadableContent;
            Description = description;
            HeightTex = GetTexture2D(Description.HeightTex);
            HeightBlendTex = GetTexture2D(Description.HeightBlendTex);
            DiffuseTex = GetTexture2D(Description.DiffuseTex);
            DiffuseBlendTex = GetTexture2D(Description.DiffuseBlendTex);
        }

        private Texture2D GetTexture2D(AssetInfo assetInfo)
        {
            return LoadableContent.GetAsset<Texture2D>(assetInfo);
        }
    }
}
