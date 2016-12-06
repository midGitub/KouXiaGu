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
        public static readonly float HalfBlockWidth = BlockWidth / 2;
        public static readonly float HalfBlockHeight = BlockHeight / 2;

        /// <summary>
        /// 从像素节点 获取到所属的地形块;
        /// </summary>
        public static ShortVector2 PixelToBlockCoord(Vector3 position)
        {
            short x = (short)Math.Floor(position.x / BlockWidth);
            short y = (short)Math.Floor(position.z / BlockHeight);
            return new ShortVector2(x, y);
        }

        /// <summary>
        /// 地图块坐标 获取到其像素中心点;
        /// </summary>
        public static Vector3 BlockCoordToCenter(ShortVector2 coord)
        {
            float x = coord.x * HalfBlockWidth + (coord.x >= 0 ? HalfBlockWidth : 0);
            float z = coord.y * HalfBlockHeight + (coord.y >= 0 ? HalfBlockHeight : 0);
            return new Vector3(x, 0, z);
        }




    }

}
