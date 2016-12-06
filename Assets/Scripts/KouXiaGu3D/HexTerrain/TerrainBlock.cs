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
        
        /// <summary>
        /// 地图块大小(需要大于或等于2),不允许动态变换;
        /// </summary>
        public const int size = 4;

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

        public static readonly float BlockWidth = (float)(hexagon.OuterDiameters * 1.5f * (size - 1));
        public static readonly float BlockHeight = (float)hexagon.InnerDiameters * size;

        /// <summary>
        /// 从像素节点 获取到所属的地形块;
        /// </summary>
        public static ShortVector2 PixelToBlockCoord(Vector3 position)
        {
            short x = (short)Math.Round(position.x / BlockWidth);
            short y = (short)Math.Round(position.z / BlockHeight);
            return new ShortVector2(x, y);
        }

        /// <summary>
        /// 地图块坐标 获取到其像素中心点;
        /// </summary>
        public static Vector3 BlockCoordToPixelCenter(ShortVector2 coord)
        {
            float x = coord.x * BlockWidth;
            float z = coord.y * BlockHeight;
            return new Vector3(x, 0, z);
        }

        /// <summary>
        /// 地图块坐标 获取到其中心的六边形坐标;
        /// </summary>
        public static CubicHexCoord BlockCoordToHexCenter(ShortVector2 coord)
        {
            Vector3 pixelCenter = BlockCoordToPixelCenter(coord);
            return HexGrids.PixelToHex(pixelCenter);
        }

        /// <summary>
        /// 获取到这个地图块覆盖到的所有地图节点坐标;
        /// </summary>
        public static IEnumerable<CubicHexCoord> GetBlockCover(ShortVector2 coord)
        {
            CubicHexCoord hexCenter = BlockCoordToHexCenter(coord);
            CubicHexCoord startCoord = HexGrids.HexDirectionVector(HexDirections.Southwest) * size + hexCenter + CubicHexCoord.DIR_South;

            for (short endX = (short)-startCoord.X;
                startCoord.X <= endX;
                startCoord += (startCoord.X & 1) == 0 ?
                (((size & 1) == 0) ? CubicHexCoord.DIR_Northeast : CubicHexCoord.DIR_Southeast) :
                (((size & 1) == 0) ? CubicHexCoord.DIR_Southeast : CubicHexCoord.DIR_Northeast))
            {
                CubicHexCoord startRow = startCoord;
                for (short endY = startRow.Z;
                    startRow.Y <= endY;
                    startRow += CubicHexCoord.DIR_North)
                {
                    yield return startRow;
                }
            }
        }

    }

}
