﻿using System;
using UnityEngine;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形烘培质量;
    /// </summary>
    [XmlType("LandformQuality"), Serializable]
    public sealed class LandformQuality
    {
        /// <summary>
        /// 烘焙时的边框比例(需要裁剪的部分比例);
        /// </summary>
        private static readonly float OutlineScale = 1f / 24f;

        /// <summary>
        /// 完整预览整个地图块的摄像机旋转角度;
        /// </summary>
        public static readonly Quaternion CameraRotation = Quaternion.Euler(90, 0, 0);

        /// <summary>
        /// 相机高度;
        /// </summary>
        public static readonly float CameraHight = 5;

        /// <summary>
        /// 完整预览整个地图块的摄像机大小;
        /// </summary>
        public static readonly float CameraSize =
            ((LandformChunkInfo.ChunkHeight + (LandformChunkInfo.ChunkHeight * OutlineScale)) / 2);

        /// <summary>
        /// 完整预览整个地图块的摄像机比例(W/H);
        /// </summary>
        public static readonly float CameraAspect =
            (LandformChunkInfo.ChunkWidth + LandformChunkInfo.ChunkWidth * OutlineScale) /
            (LandformChunkInfo.ChunkHeight + LandformChunkInfo.ChunkHeight * OutlineScale);

        [XmlIgnore, SerializeField, Range(1, 300)]
        private float textureSize = 120;

        [XmlIgnore, SerializeField, Range(0.1f, 1)]
        private float diffuseMapRatios = 1;

        [XmlIgnore, SerializeField, Range(0.1f, 1)]
        private float heightMapRatios = 1;

        public BakeTextureInfo LandformDiffuseMap { get; private set; }
        public BakeTextureInfo LandformHeightMap { get; private set; }

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
        /// DiffuseMap比例;
        /// </summary>
        [XmlElement("DiffuseMapRatios")]
        public float LandformDiffuseMapRatios
        {
            get { return diffuseMapRatios; }
            set { diffuseMapRatios = value; }
        }

        /// <summary>
        /// HeightMap比例;
        /// </summary>
        [XmlElement("HeightMapRatios")]
        public float LandformHeightMapRatios
        {
            get { return heightMapRatios; }
            set { heightMapRatios = value; }
        }

        public LandformQuality()
        {
            Updata();
        }

        public LandformQuality(float textureSize, float diffuseMapRatios, float heightMapRatios)
        {
            this.textureSize = textureSize;
            this.diffuseMapRatios = diffuseMapRatios;
            this.heightMapRatios = heightMapRatios;
            Updata();
        }

        /// <summary>
        /// 更新数据,在每次设置完值后需要手动调用;
        /// </summary>
        public void Updata()
        {
            int chunkWidth = (int)Math.Round(LandformChunkInfo.ChunkWidth * textureSize);
            int chunkHeight = (int)Math.Round(LandformChunkInfo.ChunkHeight * textureSize);

            LandformDiffuseMap = new BakeTextureInfo(chunkWidth, chunkHeight, diffuseMapRatios, OutlineScale);
            LandformHeightMap = new BakeTextureInfo(chunkWidth, chunkHeight, heightMapRatios, OutlineScale);
        }
    }


    public class BakeTextureInfo
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
