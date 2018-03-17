using System;
using System.Xml.Serialization;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{
    public class Quality
    {
        [XmlIgnore, SerializeField, Range(1, 300)]
        private float textureSize = 120;

        [XmlIgnore, SerializeField, Range(0.1f, 1)]
        private float diffuseMapRatios = 1;

        public int DiffuseMapWidth => (int)Math.Round(textureSize * diffuseMapRatios * ChunkInfo.Width);
        public int DiffuseMapHeight => (int)Math.Round(textureSize * diffuseMapRatios * ChunkInfo.Height);

        [XmlIgnore, SerializeField, Range(0.1f, 1)]
        private float heightMapRatios = 1;

        public int HeightMapWidth => (int)Math.Round(textureSize * heightMapRatios * ChunkInfo.Width);
        public int HeightMapHeight => (int)Math.Round(textureSize * heightMapRatios * ChunkInfo.Height);


        /// <summary>
        /// 贴图大小 推荐 40 ~ 300;
        /// </summary>
        public float TextureSize
        {
            get { return textureSize; }
            set { textureSize = value; }
        }

        /// <summary>
        /// DiffuseMap比例;
        /// </summary>
        public float DiffuseMapRatios
        {
            get { return diffuseMapRatios; }
            set { diffuseMapRatios = value; }
        }

        /// <summary>
        /// HeightMap比例;
        /// </summary>
        public float HeightMapRatios
        {
            get { return heightMapRatios; }
            set { heightMapRatios = value; }
        }
    }
}
