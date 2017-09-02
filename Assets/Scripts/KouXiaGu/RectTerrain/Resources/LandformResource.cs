using System;
using System.Xml.Serialization;
using UnityEngine;
using KouXiaGu.Concurrent;

namespace KouXiaGu.RectTerrain
{

    [XmlRoot("TextureInfo")]
    public class TextureInfo
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlIgnore]
        public Texture Texture { get; set; }
    }


    /// <summary>
    /// 地貌资源信息;
    /// </summary>
    [XmlRoot("LandformResourceInfo")]
    public class LandformResourceInfo
    {
        /// <summary>
        /// 高度调整贴图名;
        /// </summary>
        [XmlElement("HeightTex")]
        public string HeightTexName { get; set; }

        [XmlIgnore]
        public Texture HeightTex { get; set; }


        /// <summary>
        /// 高度调整的权重贴图;
        /// </summary>
        [XmlElement("HeightBlendTex")]
        public string HeightBlendTexName { get; set; }

        [XmlIgnore]
        public Texture HeightBlendTex { get; set; }


        /// <summary>
        /// 漫反射贴图名;
        /// </summary>
        [XmlElement("DiffuseTex")]
        public string DiffuseTexName { get; set; }

        [XmlIgnore]
        public Texture DiffuseTex { get; set; }


        /// <summary>
        /// 漫反射混合贴图名;
        /// </summary>
        [XmlElement("DiffuseBlendTex")]
        public string DiffuseBlendTexName { get; set; }

        [XmlIgnore]
        public Texture DiffuseBlendTex { get; set; }
    }


    /// <summary>
    /// 地貌贴图信息;
    /// </summary>
    public class LandformResource
    {
        public LandformResourceInfo Info { get; protected set; }
        public Texture DiffuseTex { get; protected set; }
        public Texture DiffuseBlendTex { get; protected set; }
        public Texture HeightTex { get; protected set; }
        public Texture HeightBlendTex { get; protected set; }

        /// <summary>
        /// 是否为空?
        /// </summary>
        public virtual bool IsEmpty
        {
            get
            {
                return
                    DiffuseTex == null &&
                    DiffuseBlendTex == null &&
                    HeightTex == null &&
                    HeightBlendTex == null;
            }
        }

        /// <summary>
        /// 是否所有都不为空?
        /// </summary>
        public virtual bool IsComplete
        {
            get
            {
                return
                    DiffuseTex != null &&
                    DiffuseBlendTex != null &&
                    HeightTex != null &&
                    HeightBlendTex != null;
            }
        }

        /// <summary>
        /// 销毁所有贴图;
        /// </summary>
        public virtual void Destroy()
        {
            if (DiffuseTex != null)
            {
                Destroy(DiffuseTex);
                DiffuseTex = null;
            }

            if (DiffuseBlendTex != null)
            {
                Destroy(DiffuseBlendTex);
                DiffuseBlendTex = null;
            }

            if (HeightTex != null)
            {
                Destroy(HeightTex);
                HeightTex = null;
            }

            if (HeightBlendTex != null)
            {
                Destroy(HeightBlendTex);
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
}
