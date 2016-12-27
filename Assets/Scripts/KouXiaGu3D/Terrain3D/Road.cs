using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public sealed class Road
    {

        #region 已初始化合集(静态);

        /// <summary>
        /// 所有已经初始化完毕的道路信息;
        /// </summary>
        static readonly CustomDictionary<int, Road> initializedDictionary = new CustomDictionary<int, Road>();

        /// <summary>
        /// 所有已经初始化完毕的道路信息;
        /// </summary>
        public static IReadOnlyDictionary<int, Road> initializedInstances
        {
            get { return initializedDictionary; }
        }

        #endregion


        #region 初始化方法;


        /// <summary>
        /// 初始化所有信息;
        /// </summary>
        public IEnumerator LoadAll(IEnumerable<RoadDescr> descriptions, AssetBundle asset)
        {

            foreach (var description in descriptions)
            {
                Road road = new Road(description);
                road.Load(asset);
            }

            yield return null;
        }


        #endregion

        Road(RoadDescr description)
        {
            this.Description = description;
        }

        public RoadDescr Description { get; private set; }
        public Texture HeightAdjustTex { get; private set; }
        public Texture HeightAdjustBlendTex { get; private set; }
        public Texture DiffuseTex { get; private set; }
        public Texture DiffuseBlendTex { get; private set; }

        /// <summary>
        /// 是否不为空?
        /// </summary>
        public bool IsNotEmpty
        {
            get { return DiffuseTex != null || HeightAdjustTex != null || DiffuseBlendTex != null || HeightAdjustBlendTex != null; }
        }

        /// <summary>
        /// 贴图读取完毕?
        /// </summary>
        public bool IsLoadComplete
        {
            get { return DiffuseTex != null && HeightAdjustTex != null && DiffuseBlendTex != null && HeightAdjustBlendTex != null; }
        }

        /// <summary>
        /// 清除所有资源;
        /// </summary>
        void Destroy()
        {
            if (IsNotEmpty)
            {
                GameObject.Destroy(this.DiffuseTex);
                GameObject.Destroy(this.HeightAdjustTex);
                GameObject.Destroy(this.DiffuseBlendTex);
                GameObject.Destroy(this.HeightAdjustBlendTex);

                this.DiffuseTex = null;
                this.HeightAdjustTex = null;
                this.DiffuseBlendTex = null;
                this.HeightAdjustBlendTex = null;
            }
        }

        /// <summary>
        /// 从资源包同步读取到;
        /// </summary>
        void Load(AssetBundle assetBundle)
        {
            if (IsNotEmpty)
                throw new ArgumentException();

            try
            {
                Texture diffuse = assetBundle.LoadAsset<Texture>(Description.DiffuseTex);
                Texture height = assetBundle.LoadAsset<Texture>(Description.HeightAdjustTex);
                Texture diffuseBlend = assetBundle.LoadAsset<Texture>(Description.DiffuseBlendTex);
                Texture heightBlend = assetBundle.LoadAsset<Texture>(Description.HeightAdjustTex);

                this.DiffuseTex = diffuse;
                this.HeightAdjustTex = height;
                this.DiffuseBlendTex = diffuseBlend;
                this.HeightAdjustBlendTex = heightBlend;
            }
            catch (Exception e)
            {
                Destroy();
                throw e;
            }
        }

        public override bool Equals(object obj)
        {
            return this.Description.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Description.GetHashCode();
        }

        public override string ToString()
        {
            string info = this.Description.ToString() + "[LoadComplete:" + IsLoadComplete + "]";
            return info;
        }


        [XmlType("Road")]
        public struct RoadDescr
        {

            /// <summary>
            /// 唯一标示(0,-1作为保留);
            /// </summary>
            [XmlAttribute("id")]
            public int ID { get; set; }

            /// <summary>
            /// 定义名,允许为空;
            /// </summary>
            [XmlAttribute("name")]
            public string Name { get; set; }

            /// <summary>
            /// 高度调整贴图名;
            /// </summary>
            [XmlElement("HeightTex")]
            public string HeightAdjustTex { get; set; }

            /// <summary>
            /// 高度调整的权重贴图;
            /// </summary>
            [XmlElement("HeightBlendTex")]
            public string HeightAdjustBlendTex { get; set; }

            /// <summary>
            /// 漫反射贴图名;
            /// </summary>
            [XmlElement("DiffuseTex")]
            public string DiffuseTex { get; set; }

            /// <summary>
            /// 漫反射混合贴图名;
            /// </summary>
            [XmlElement("DiffuseBlendTex")]
            public string DiffuseBlendTex { get; set; }

            public override bool Equals(object obj)
            {
                if (!(obj is RoadDescr))
                    return false;
                return ID == ((RoadDescr)obj).ID;
            }

            public override int GetHashCode()
            {
                return ID.GetHashCode();
            }

            public override string ToString()
            {
                return "[ID:" + ID.ToString() + ",Name:" + Name.ToString() + "]";
            }

        }

    }

}
