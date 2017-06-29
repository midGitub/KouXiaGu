﻿using System;
using UnityEngine;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 烘培细节设置;
    /// </summary>
    [XmlType("TerrainBakeSettings"), Serializable]
    public sealed class QualitySettings
    {

        /// <summary>
        /// 烘焙时的边框比例(需要裁剪的部分比例);
        /// </summary>
        static readonly float OutlineScale = 1f / 24f;

        /// <summary>
        /// 完整预览整个地图块的摄像机旋转角度;
        /// </summary>
        public static readonly Quaternion CameraRotation = Quaternion.Euler(90, 0, 0);

        /// <summary>
        /// 完整预览整个地图块的摄像机大小;
        /// </summary>
        public static readonly float CameraSize =
            ((ChunkInfo.ChunkHeight + (ChunkInfo.ChunkHeight * OutlineScale)) / 2);

        /// <summary>
        /// 完整预览整个地图块的摄像机比例(W/H);
        /// </summary>
        public static readonly float CameraAspect =
            (ChunkInfo.ChunkWidth + ChunkInfo.ChunkWidth * OutlineScale) /
            (ChunkInfo.ChunkHeight + ChunkInfo.ChunkHeight * OutlineScale);


        QualitySettings()
        {
        }
        
        public QualitySettings(float textureSize, float diffuseMapRatios, float heightMapRatios)
        {
            this.textureSize = textureSize;
            this.diffuseMapRatios = diffuseMapRatios;
            this.heightMapRatios = heightMapRatios;
            Updata();
        }


        [XmlIgnore, SerializeField, Range(1, 300)]
        float textureSize = 120;

        [XmlIgnore, SerializeField, Range(0.1f, 1)]
        float diffuseMapRatios = 1;

        [XmlIgnore, SerializeField, Range(0.1f, 1)]
        float heightMapRatios = 1;

        internal BakeTextureInfo LandformDiffuseMap { get; private set; }
        internal BakeTextureInfo LandformHeightMap { get; private set; }

        /// <summary>
        /// 贴图大小 推荐 40 ~ 300;
        /// </summary>
        [XmlElement("TextureSize")]
        public float TextureSize
        {
            get { return textureSize; }
            set { textureSize = value; }
        }

        /// <summary>
        /// 贴图比例;
        /// </summary>
        [XmlElement("DiffuseMapRatios")]
        public float LandformDiffuseMapRatios
        {
            get { return diffuseMapRatios; }
            set { diffuseMapRatios = value; }
        }

        /// <summary>
        /// 贴图比例;
        /// </summary>
        [XmlElement("HeightMapRatios")]
        public float LandformHeightMapRatios
        {
            get { return heightMapRatios; }
            set { heightMapRatios = value; }
        }

        /// <summary>
        /// 更新数据,在每次设置完值后需要手动调用;
        /// </summary>
        public void Updata()
        {
            int chunkWidth = (int)Math.Round(ChunkInfo.ChunkWidth * textureSize);
            int chunkHeight = (int)Math.Round(ChunkInfo.ChunkHeight * textureSize);

            LandformDiffuseMap = new BakeTextureInfo(chunkWidth, chunkHeight, diffuseMapRatios, OutlineScale);
            LandformHeightMap = new BakeTextureInfo(chunkWidth, chunkHeight, heightMapRatios, OutlineScale);
        }
    }


    class BakeTextureInfo
    {
        public BakeTextureInfo(int width, int height, float ratios, float outlineScale)
        {
            width = (int)(width * ratios);
            height = (int)(height * ratios);
            Init(width, height, outlineScale);
        }

        /// <summary>
        /// 构造;
        /// </summary>
        /// <param name="width">最终宽度</param>
        /// <param name="height">最终高度;</param>
        /// <param name="outlineScale">烘焙时的边框比例(需要裁剪的部分比例);</param>
        public BakeTextureInfo(int width, int height, float outlineScale)
        {
            Init(width, height, outlineScale);
        }

        void Init(int width, int height, float outlineScale)
        {
            Width = width;
            Height = height;
            BakeWidth = Width + (int)(Width * outlineScale);
            BakeHeight = Height + (int)(Height * outlineScale);
            ClippingRect = new Rect(
                (int)(Width * outlineScale / 2),
                (int)(Height * outlineScale / 2),
                Width,
                Height);
        }

        /// <summary>
        /// 图片裁剪后的宽度;
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// 图片裁剪后的高度;
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// 烘焙时的宽度;
        /// </summary>
        public int BakeWidth { get; private set; }

        /// <summary>
        /// 烘焙时的高度;
        /// </summary>
        public int BakeHeight { get; private set; }

        /// <summary>
        /// 裁剪区域;
        /// </summary>
        public Rect ClippingRect { get; private set; }
    }

}