using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 全局定义地形块大小,和部分渲染参数;
    /// </summary>
    public static class TerrainBlock
    {

        /// <summary>
        /// 地图节点所使用的六边形参数;
        /// </summary>
        static Hexagon hexagon
        {
            get { return HexGrids.hexagon; }
        }

        const int size = 4;

        /// <summary>
        /// 完整预览整个地图块的摄像机比例;
        /// </summary>
        public static readonly float CameraAspect = (float)((hexagon.OuterDiameters + hexagon.OuterRadius / 4) / hexagon.InnerDiameters);

        /// <summary>
        /// 完整预览整个地图块的摄像机大小;
        /// </summary>
        public static readonly float CameraSize = (float)hexagon.InnerDiameters * (size / 2);

        /// <summary>
        /// 完整预览整个地图块的摄像机旋转角度;
        /// </summary>
        public static readonly Quaternion CameraRotation = Quaternion.Euler(90, 0, 0);


        public static readonly float BlockWidth = (float)(hexagon.OuterDiameters * size + hexagon.OuterRadius);

        public static readonly float BlockHeight = (float)hexagon.InnerDiameters * size;

    }

}
