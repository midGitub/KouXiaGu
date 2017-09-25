using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;
using JiongXiaGu.Concurrent;
using System.IO;
using JiongXiaGu.Resources;
using System.Linq;

namespace JiongXiaGu.Unity.RectTerrain.Resources
{

    /// <summary>
    /// 地貌资源信息;
    /// </summary>
    [XmlRoot("LandformResourceInfo")]
    public sealed class LandformResource
    {

        /// <summary>
        /// 唯一标识ID;
        /// </summary>
        [XmlAttribute("id")]
        public int ID { get; set; }

        /// <summary>
        /// 地形名;
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 高度调整贴图;
        /// </summary>
        [XmlElement("HeightTex")]
        public TextureInfo HeightTex { get; set; }

        /// <summary>
        /// 高度调整的权重贴图;
        /// </summary>
        [XmlElement("HeightBlendTex")]
        public TextureInfo HeightBlendTex { get; set; }

        /// <summary>
        /// 漫反射贴图名;
        /// </summary>
        [XmlElement("DiffuseTex")]
        public TextureInfo DiffuseTex { get; set; }

        /// <summary>
        /// 漫反射混合贴图名;
        /// </summary>
        [XmlElement("DiffuseBlendTex")]
        public TextureInfo DiffuseBlendTex { get; set; }

        /// <summary>
        /// 是否为空?
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return
                    DiffuseTex.Texture == null &&
                    DiffuseBlendTex.Texture == null &&
                    HeightTex.Texture == null &&
                    HeightBlendTex.Texture == null;
            }
        }

        /// <summary>
        /// 是否所有都不为空?
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return
                    DiffuseTex.IsCompleted &&
                    DiffuseBlendTex.IsCompleted &&
                    HeightTex.IsCompleted &&
                    HeightBlendTex.IsCompleted;
            }
        }

        /// <summary>
        /// 销毁所有贴图;
        /// </summary>
        public void Destroy()
        {
            if (DiffuseTex.Texture != null)
            {
                Destroy(DiffuseTex.Texture);
                DiffuseTex = null;
            }

            if (DiffuseBlendTex.Texture != null)
            {
                Destroy(DiffuseBlendTex.Texture);
                DiffuseBlendTex = null;
            }

            if (HeightTex.Texture != null)
            {
                Destroy(HeightTex.Texture);
                HeightTex = null;
            }

            if (HeightBlendTex.Texture != null)
            {
                Destroy(HeightBlendTex.Texture);
                HeightBlendTex = null;
            }
        }

        void Destroy(UnityEngine.Object item)
        {
#if UNITY_EDITOR
            GameObject.DestroyImmediate(item);
#else
            GameObject.Destroy(item);
#endif
        }
    }


    /// <summary>
    /// 地形资源序列化;
    /// </summary>
    public sealed class LandformResourceSerializer : ResourcesReader<LandformResource[], Dictionary<int, LandformResource>>
    {
        public LandformResourceSerializer(ISerializer<LandformResource[]> serializer, ResourcesSearcher resourceSearcher) : base(serializer, resourceSearcher)
        {
        }

        protected override Dictionary<int, LandformResource> Combine(List<LandformResource[]> sources)
        {
            var dictionary = new Dictionary<int, LandformResource>();
            foreach (var source in sources)
            {
                foreach (var res in source)
                {
                    dictionary.Add(res.ID, res);
                }
            }
            return dictionary;
        }

        protected override LandformResource[] Convert(Dictionary<int, LandformResource> result)
        {
            return result.Values.ToArray();
        }
    }

    /// <summary>
    /// 序列化之后处理程序;
    /// </summary>
    public sealed class LandformResourceLoadRequest : Request
    {
        public LandformResourceLoadRequest(LandformResource landformResource, AssetBundle assetbundle)
        {
            this.landformResource = landformResource;
            this.assetbundle = assetbundle;
        }

        readonly LandformResource landformResource;
        readonly AssetBundle assetbundle;

        protected override void Operate()
        {
            Load(landformResource.DiffuseTex);
            Load(landformResource.DiffuseBlendTex);
            Load(landformResource.HeightTex);
            Load(landformResource.HeightBlendTex);
        }

        void Load(TextureInfo info)
        {
            var texture = assetbundle.LoadAsset<Texture>(info.Name);
            if (texture == null)
            {
                AddException(new FileNotFoundException("未能从<" + assetbundle.name + ">读取到资源:", info.Name));
                IsFaulted = true;
            }
            info.Texture = texture;
        }
    }
}
