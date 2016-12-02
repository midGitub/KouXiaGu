using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;

namespace KouXiaGu.Terrain
{

    /// <summary>
    /// 地貌定义;
    /// </summary>
    public class Landform 
    {

        public Landform()
        {
        }
        public Landform(int id) : this()
        {
            this.id = id;
        }

        /// <summary>
        /// 地形名;
        /// </summary>
        [XmlAttribute]
        public string name { get; private set; }

        /// <summary>
        /// 地形唯一标示;
        /// </summary>
        [XmlAttribute]
        public int id { get; private set; }

        // 贴图名或路径定义;
        [XmlElement]
        public string diffusePath { get; private set; }
        [XmlElement]
        public string heightPath { get; private set; }
        [XmlElement]
        public string mixerPath { get; private set; }

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        [XmlIgnore]
        public Texture DiffuseTexture { get; private set; }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        [XmlIgnore]
        public Texture HeightTexture { get; private set; }

        /// <summary>
        /// 混合贴图;
        /// </summary>
        [XmlIgnore]
        public Texture MixerTexture { get; private set; }

        /// <summary>
        /// 是否已经初始化完毕?
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return DiffuseTexture != null || HeightTexture != null || MixerTexture != null;
            }
        }

        /// <summary>
        /// 设置贴图到;
        /// </summary>
        public void SetLandform(Texture diffuse, Texture height, Texture mixer)
        {
            this.DiffuseTexture = diffuse;
            this.HeightTexture = height;
            this.MixerTexture = mixer;
        }

        public override string ToString()
        {
            string info = string.Concat("id:", id, ",name:", name, base.ToString());
            return info;
        }

    }

}
