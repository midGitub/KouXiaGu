using System;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    /// <summary>
    /// 地形烘焙时的贴图大小参数;
    /// </summary>
    [Serializable]
    public struct BakingParameter
    {

        /// <summary>
        /// 烘焙时的边框比例(需要裁剪的部分比例);
        /// </summary>
        public static readonly float OutlineScale = 1f / 12f;

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

        [SerializeField, Range(50, 500)]
        float textureSize;

        [SerializeField, Range(0, 3)]
        int diffuseTexDownsample;

        [SerializeField, Range(0, 3)]
        int heightMapDownsample;

        /// <summary>
        /// 贴图大小;
        /// </summary>
        public float TextureSize
        {
            get { return textureSize; }
            private set { textureSize = value; }
        }

        /// <summary>
        /// 图片裁剪后的尺寸;
        /// </summary>
        public int DiffuseTexWidth { get; private set; }
        public int DiffuseTexHeight { get; private set; }
        public int HeightMapWidth { get; private set; }
        public int HeightMapHeight { get; private set; }

        /// <summary>
        /// 烘焙时的尺寸;
        /// </summary>
        public int rDiffuseTexWidth { get; private set; }
        public int rDiffuseTexHeight { get; private set; }
        public int rHeightMapWidth { get; private set; }
        public int rHeightMapHeight { get; private set; }

        /// <summary>
        /// 裁剪区域;
        /// </summary>
        public Rect DiffuseReadPixel { get; private set; }
        public Rect HeightReadPixel { get; private set; }

        public BakingParameter(float textureSize, int diffuseTexDownsample, int heightMapDownsample) : this()
        {
            this.diffuseTexDownsample = diffuseTexDownsample;
            this.heightMapDownsample = heightMapDownsample;
            SetTextureSize(textureSize);
        }

        public void Reset()
        {
            SetTextureSize(this.textureSize);
        }

        void SetTextureSize(float size)
        {
            float chunkWidth = TerrainChunk.CHUNK_WIDTH * size;
            float chunkHeight = TerrainChunk.CHUNK_HEIGHT * size;

            this.DiffuseTexWidth = (int)(chunkWidth) >> diffuseTexDownsample;
            this.DiffuseTexHeight = (int)(chunkHeight) >> diffuseTexDownsample;
            this.HeightMapWidth = (int)(chunkWidth) >> heightMapDownsample;
            this.HeightMapHeight = (int)(chunkHeight) >> heightMapDownsample;

            this.rDiffuseTexWidth = (int)(DiffuseTexWidth + DiffuseTexWidth * OutlineScale);
            this.rDiffuseTexHeight = (int)(DiffuseTexHeight + DiffuseTexHeight * OutlineScale);
            this.rHeightMapWidth = (int)(HeightMapWidth + HeightMapWidth * OutlineScale);
            this.rHeightMapHeight = (int)(HeightMapHeight + HeightMapHeight * OutlineScale);

            this.DiffuseReadPixel =
                new Rect(
                    DiffuseTexWidth * OutlineScale / 2,
                    DiffuseTexHeight * OutlineScale / 2,
                    DiffuseTexWidth,
                    DiffuseTexHeight);

            this.HeightReadPixel =
                new Rect(
                    HeightMapWidth * OutlineScale / 2,
                    HeightMapHeight * OutlineScale / 2,
                    HeightMapWidth,
                    HeightMapHeight);

            this.TextureSize = size;
        }

    }

}
