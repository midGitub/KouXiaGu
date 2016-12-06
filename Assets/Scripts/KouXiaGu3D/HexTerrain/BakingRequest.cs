using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 地形烘焙请求;
    /// 烘焙的为一个正六边形网格内的区域;
    /// </summary>
    public struct BakingRequest
    {

        const float cameraSize = 5f;

        /// <summary>
        /// 请求烘焙中心点的位置;
        /// </summary>
        public CubicHexCoord Position { get; set; }

        //烘焙参数;
        public int DiffuseMapWidth { get; set; }
        public int DiffuseMapHeight { get; set; }
        public int DiffuseMapDepthBuffer { get; set; }
        public RenderTextureFormat DiffuseMapFormat { get; set; }
        public RenderTextureReadWrite DiffuseMapReadWrite { get; set; }
        public int DiffuseMapAntiAliasing { get; set; }

        public int HeightMapWidth { get; set; }
        public int HeightMapHeight { get; set; }
        public int HeightMapDepthBuffer { get; set; }
        public RenderTextureFormat HeightMapFormat { get; set; }
        public RenderTextureReadWrite HeightMapReadWrite { get; set; }
        public int HeightMapAntiAliasing { get; set; }

        /// <summary>
        /// 摄像机正交大小;
        /// </summary>
        public float CameraSize
        {
            get { return cameraSize; }
        }

        /// <summary>
        /// 摄像机位置;
        /// </summary>
        public Vector3 CameraPosition
        {
            get { return HexGrids.HexToPixel(Position); }
        }

        /// <summary>
        /// 获取到放置贴图的位置;
        /// </summary>
        public IEnumerable<KeyValuePair<Vector3, Landform>> BakingRange()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 当完成时调用;
        /// </summary>
        public void OnComplete(Texture2D diffuse, Texture2D height)
        {
            throw new NotImplementedException();
        }

    }

}
