using System;
using System.Collections.Generic;
using System.Linq;
using JiongXiaGu.Grids;
using UnityEngine;

namespace JiongXiaGu.Terrain3D
{

    /// <summary>
    /// 地图信息转换;
    /// </summary>
    public static class LandformConvert
    {

        /// <summary>
        /// 六边形半径定义(单位像素);
        /// </summary>
        public const int OuterRadius = 1;

        /// <summary>
        /// 提供的六边形运算;
        /// </summary>
        public static readonly Hexagon hexagon = new Hexagon(OuterRadius);

        /// <summary>
        /// 六边形起点;
        /// </summary>
        public static readonly RectCoord Origin = new RectCoord(0, 0);
        /// <summary>
        /// 六边形的像素坐标起点;
        /// </summary>
        public static readonly Vector3 OriginPixelPoint = Vector3.zero;

        static readonly CubicHexGrid grid = new CubicHexGrid(OuterRadius);

        /// <summary>
        /// 在场景使用的网格;
        /// </summary>
        public static CubicHexGrid Grid
        {
            get { return grid; }
        }


        /// <summary>
        /// 转换为地形使用的六边形坐标;
        /// </summary>
        public static CubicHexCoord GetTerrainCubic(this Vector3 pos)
        {
            return Grid.GetCubic(pos);
        }

        /// <summary>
        /// 转换为地形使用的坐标;
        /// </summary>
        public static Vector3 GetTerrainPixel(this CubicHexCoord coord)
        {
            return Grid.GetPixel(coord);
        }

        /// <summary>
        /// 转换为地形使用的坐标,并指定y轴;
        /// </summary>
        public static Vector3 GetTerrainPixel(this CubicHexCoord coord, float y)
        {
            return Grid.GetPixel(coord, y);
        }


        /// <summary>
        /// 六边形方向对应的欧拉角;
        /// </summary>
        static readonly Dictionary<int, float> hexAngleDictionary = new Dictionary<int, float>()
        {
            {(int)HexDirections.North, 0},
            {(int)HexDirections.Northeast, 60},
            {(int)HexDirections.Southeast, 120},
            {(int)HexDirections.South, 180},
            {(int)HexDirections.Southwest, 240},
            {(int)HexDirections.Northwest, 300},
        };

        /// <summary>
        /// 获取到六边形对应方向的角度(z轴指向),本身返回异常;
        /// </summary>
        public static float GetAngle(HexDirections direction)
        {
            return hexAngleDictionary[(int)direction];
        }


        /// <summary>
        /// 矩形方向对应的欧拉角;
        /// </summary>
        static readonly Dictionary<int, float> rectAngleDictionary = new Dictionary<int, float>()
        {
            {(int)RecDirections.North, 0},
            {(int)RecDirections.Northeast, 45},
            {(int)RecDirections.East, 90},
            {(int)RecDirections.Southeast, 135},
            {(int)RecDirections.South, 180},
            {(int)RecDirections.Southwest, 225},
            {(int)RecDirections.West, 270},
            {(int)RecDirections.Northwest, 315},
        };

        /// <summary>
        /// 获取到矩形对应方向的角度(z轴指向),本身返回异常;
        /// </summary>
        public static float GetAngle(RecDirections direction)
        {
            return rectAngleDictionary[(int)direction];
        }

    }

}