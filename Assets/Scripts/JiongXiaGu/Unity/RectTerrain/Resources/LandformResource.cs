using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{


    public class LandformResource
    {
        private readonly Dictionary<string, ModificationLandformInfo> descriptionDictionary;

        internal IDictionary<string, ModificationLandformInfo> Descriptions => descriptionDictionary;

        public LandformResource()
        {
            descriptionDictionary = new Dictionary<string, ModificationLandformInfo>();
        }

        /// <summary>
        /// 添加描述信息;
        /// </summary>
        public void Add(Modification content, IEnumerable<LandformDescription> descriptions, AddMode addMode)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (descriptions == null)
                throw new ArgumentNullException(nameof(descriptions));

            foreach (var description in descriptions)
            {
                string key = description.ID;
                ModificationLandformInfo info = new ModificationLandformInfo(content, description);
                descriptionDictionary.Add(key, info, addMode);
            }
        }

        /// <summary>
        /// 获取到地形资源;
        /// </summary>
        public LandformTextures Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            ModificationLandformInfo info;
            if (descriptionDictionary.TryGetValue(name, out info))
            {
                LandformTextures res = new LandformTextures()
                {
                    DiffuseTex = LoadTexture(info.Modification, info.Description.DiffuseTex),
                    DiffuseBlendTex = LoadTexture(info.Modification, info.Description.DiffuseBlendTex),
                    HeightTex = LoadTexture(info.Modification, info.Description.HeightTex),
                    HeightBlendTex = LoadTexture(info.Modification, info.Description.HeightBlendTex),
                };
                return res;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        private Texture2D LoadTexture(Modification modification, AssetInfo assetInfo)
        {
            return modification.GetAsset<Texture2D>(assetInfo);
        }

        public void Clear()
        {
            descriptionDictionary.Clear();
        }

        internal struct ModificationLandformInfo
        {
            public Modification Modification { get; private set; }
            public LandformDescription Description { get; private set; }

            public ModificationLandformInfo(Modification content, LandformDescription description)
            {
                Modification = content;
                Description = description;
            }
        }
    }
}
