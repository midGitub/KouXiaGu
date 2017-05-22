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


        [XmlIgnore, SerializeField, Range(40, 500)]
        float textureSize = 120;

        [XmlIgnore, SerializeField, Range(0.1f, 1)]
        float diffuseMapRatios = 1;

        [XmlIgnore, SerializeField, Range(0.1f, 1)]
        float heightMapRatios = 1;

        internal BakeTextureInfo LandformDiffuseMap { get; private set; }
        internal BakeTextureInfo LandformHeightMap { get; private set; }

        ///// <summary>
        ///// 图片裁剪后的尺寸;
        ///// </summary>
        //[XmlIgnore]
        //public int DiffuseTexWidth { get; private set; }
        //[XmlIgnore]
        //public int DiffuseTexHeight { get; private set; }
        //[XmlIgnore]
        //public int HeightMapWidth { get; private set; }
        //[XmlIgnore]
        //public int HeightMapHeight { get; private set; }

        ///// <summary>
        ///// 烘焙时的尺寸;
        ///// </summary>
        //[XmlIgnore]
        //public int rDiffuseTexWidth { get; private set; }
        //[XmlIgnore]
        //public int rDiffuseTexHeight { get; private set; }
        //[XmlIgnore]
        //public int rHeightMapWidth { get; private set; }
        //[XmlIgnore]
        //public int rHeightMapHeight { get; private set; }

        ///// <summary>
        ///// 漫反射图裁剪区域;
        ///// </summary>
        //[XmlIgnore]
        //public Rect DiffuseReadPixel { get; private set; }
        ///// <summary>
        ///// 高度图裁剪区域;
        ///// </summary>
        //[XmlIgnore]
        //public Rect HeightReadPixel { get; private set; }


        /// <summary>
        /// 贴图大小 推荐 80 ~ 500;
        /// </summary>
        [XmlElement("TextureSize")]
        public float TextureSize
        {
            get { return textureSize; }
            set { textureSize = value; }
        }

        /// <summary>
        /// 贴图分辨率百分比 0.1~1
        /// </summary>
        [XmlElement("DiffuseMapRatios")]
        public float DiffuseMapRatios
        {
            get { return diffuseMapRatios; }
            set { diffuseMapRatios = value; }
        }

        /// <summary>
        /// 贴图分辨率百分比 0.1~1
        /// </summary>
        [XmlElement("HeightMapRatios")]
        public float HeightMapRatios
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

            //DiffuseTexWidth = (int)Math.Round(chunkWidth * diffuseMapRatios);
            //DiffuseTexHeight = (int)Math.Round(chunkHeight * diffuseMapRatios);
            //HeightMapWidth = (int)Math.Round(chunkWidth * heightMapRatios);
            //HeightMapHeight = (int)Math.Round(chunkHeight * heightMapRatios);

            //rDiffuseTexWidth = DiffuseTexWidth + (int)(DiffuseTexWidth * OutlineScale);
            //rDiffuseTexHeight = DiffuseTexHeight + (int)(DiffuseTexHeight * OutlineScale);
            //rHeightMapWidth = HeightMapWidth + (int)(HeightMapWidth * OutlineScale);
            //rHeightMapHeight = HeightMapHeight + (int)(HeightMapHeight * OutlineScale);

            //DiffuseReadPixel = new Rect(
            //        (int)(DiffuseTexWidth * OutlineScale / 2),
            //        (int)(DiffuseTexHeight * OutlineScale / 2),
            //        DiffuseTexWidth,
            //        DiffuseTexHeight);

            //HeightReadPixel = new Rect(
            //        (int)(HeightMapWidth * OutlineScale / 2),
            //        (int)(HeightMapHeight * OutlineScale / 2),
            //        HeightMapWidth,
            //        HeightMapHeight);

            LandformDiffuseMap = new BakeTextureInfo(chunkWidth, chunkHeight, OutlineScale);
            LandformHeightMap = new BakeTextureInfo(chunkWidth, chunkHeight, OutlineScale);
        }
    }


    class BakeTextureInfo
    {
        /// <summary>
        /// 构造;
        /// </summary>
        /// <param name="width">最终宽度</param>
        /// <param name="height">最终高度;</param>
        /// <param name="outlineScale">烘焙时的边框比例(需要裁剪的部分比例);</param>
        public BakeTextureInfo(int width, int height, float outlineScale)
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
