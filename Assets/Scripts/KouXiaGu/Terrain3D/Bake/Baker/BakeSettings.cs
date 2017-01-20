using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    [XmlType("TerrainBakeSettings"), Serializable]
    public sealed class BakeSettings
    {

        /// <summary>
        /// 烘焙时的边框比例(需要裁剪的部分比例);
        /// </summary>
        static readonly float OutlineScale = 1f / 12f;

        /// <summary>
        /// 完整预览整个地图块的摄像机旋转角度;
        /// </summary>
        public static readonly Quaternion CameraRotation = Quaternion.Euler(90, 0, 0);

        /// <summary>
        /// 完整预览整个地图块的摄像机大小;
        /// </summary>
        public static readonly float CameraSize =
            ((TerrainChunk.CHUNK_HEIGHT + (TerrainChunk.CHUNK_HEIGHT * OutlineScale)) / 2);

        /// <summary>
        /// 完整预览整个地图块的摄像机比例(W/H);
        /// </summary>
        public static readonly float CameraAspect =
            (TerrainChunk.CHUNK_WIDTH + TerrainChunk.CHUNK_WIDTH * OutlineScale) /
            (TerrainChunk.CHUNK_HEIGHT + TerrainChunk.CHUNK_HEIGHT * OutlineScale);


        BakeSettings()
        {
        }
        
        public BakeSettings(float textureSize, float diffuseMapRatios, float heightMapRatios)
        {
            this.textureSize = textureSize;
            this.diffuseMapRatios = diffuseMapRatios;
            this.heightMapRatios = heightMapRatios;
            UpdataTextureSize();
        }


        [XmlIgnore, SerializeField, Range(80, 500)]
        float textureSize = 120;

        [XmlIgnore, SerializeField, Range(0.1f, 1)]
        float diffuseMapRatios = 1;

        [XmlIgnore, SerializeField, Range(0.1f, 1)]
        float heightMapRatios = 1;

        /// <summary>
        /// 图片裁剪后的尺寸;
        /// </summary>
        [XmlIgnore]
        public int DiffuseTexWidth { get; private set; }
        [XmlIgnore]
        public int DiffuseTexHeight { get; private set; }
        [XmlIgnore]
        public int HeightMapWidth { get; private set; }
        [XmlIgnore]
        public int HeightMapHeight { get; private set; }

        /// <summary>
        /// 烘焙时的尺寸;
        /// </summary>
        [XmlIgnore]
        public int rDiffuseTexWidth { get; private set; }
        [XmlIgnore]
        public int rDiffuseTexHeight { get; private set; }
        [XmlIgnore]
        public int rHeightMapWidth { get; private set; }
        [XmlIgnore]
        public int rHeightMapHeight { get; private set; }

        /// <summary>
        /// 裁剪区域;
        /// </summary>
        [XmlIgnore]
        public Rect DiffuseReadPixel { get; private set; }
        [XmlIgnore]
        public Rect HeightReadPixel { get; private set; }


        /// <summary>
        /// 贴图大小 推荐 80 ~ 500;
        /// </summary>
        [XmlElement("TextureSize")]
        public float TextureSize
        {
            get { return textureSize; }
            set { textureSize = value; UpdataTextureSize(); }
        }

        /// <summary>
        /// 贴图分辨率百分比 0.1~1
        /// </summary>
        [XmlElement("DiffuseMapRatios")]
        public float DiffuseMapRatios
        {
            get { return diffuseMapRatios; }
            set { diffuseMapRatios = value; UpdataTextureSize(); }
        }

        /// <summary>
        /// 贴图分辨率百分比 0.1~1
        /// </summary>
        [XmlElement("HeightMapRatios")]
        public float HeightMapRatios
        {
            get { return heightMapRatios; }
            set { heightMapRatios = value; UpdataTextureSize(); }
        }


        public void UpdataTextureSize()
        {
            float chunkWidth = TerrainChunk.CHUNK_WIDTH * textureSize;
            float chunkHeight = TerrainChunk.CHUNK_HEIGHT * textureSize;

            DiffuseTexWidth = (int)Math.Round(chunkWidth * diffuseMapRatios);
            DiffuseTexHeight = (int)Math.Round(chunkHeight * diffuseMapRatios);
            HeightMapWidth = (int)Math.Round(chunkWidth * heightMapRatios);
            HeightMapHeight = (int)Math.Round(chunkHeight * heightMapRatios);

            rDiffuseTexWidth = (int)Math.Round(DiffuseTexWidth + DiffuseTexWidth * OutlineScale);
            rDiffuseTexHeight = (int)Math.Round(DiffuseTexHeight + DiffuseTexHeight * OutlineScale);
            rHeightMapWidth = (int)Math.Round(HeightMapWidth + HeightMapWidth * OutlineScale);
            rHeightMapHeight = (int)Math.Round(HeightMapHeight + HeightMapHeight * OutlineScale);

            DiffuseReadPixel =
                new Rect(
                    DiffuseTexWidth * OutlineScale / 2,
                    DiffuseTexHeight * OutlineScale / 2,
                    DiffuseTexWidth,
                    DiffuseTexHeight);

            HeightReadPixel =
                new Rect(
                    HeightMapWidth * OutlineScale / 2,
                    HeightMapHeight * OutlineScale / 2,
                    HeightMapWidth,
                    HeightMapHeight);
        }

    }

}
