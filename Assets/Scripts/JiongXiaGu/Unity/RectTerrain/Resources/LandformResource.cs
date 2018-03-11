using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Resources;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    public class LandformResourceFactroy
    {

        public LandformResource Read(IEnumerable<Modification> modifications)
        {
            throw new NotImplementedException();
        }

        public void Add(Dictionary<string, ModificationLandformInfo> infos, Modification modification)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<LandformDescription> Load(Modification modification)
        {
            throw new NotImplementedException();
        }
    }

    public struct ModificationLandformInfo
    {
        public Modification Modification { get; private set; }
        public LandformDescription Description { get; private set; }

        public ModificationLandformInfo(Modification content, LandformDescription description)
        {
            Modification = content;
            Description = description;
        }
    }

    public class LandformResource
    {
        private readonly Dictionary<string, ModificationLandformInfo> infos;

        internal LandformResource(Dictionary<string, ModificationLandformInfo> infos)
        {
            this.infos = infos;
        }

        /// <summary>
        /// 获取到地形资源;
        /// </summary>
        public LandformRes Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            ModificationLandformInfo info;
            if (infos.TryGetValue(name, out info))
            {
                LandformRes res = new LandformRes()
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
    }
}
