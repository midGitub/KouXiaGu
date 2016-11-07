using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using UnityEngine;

namespace XGame.Running.Map
{

    /// <summary>
    /// 地图信息;
    /// </summary>
    [ProtoContract]
    [Serializable]
    public struct MapInfo : IEquatable<MapInfo>
    {

        /// <summary>
        /// 指定最 西南 和 东北 的点;
        /// </summary>
        /// <param name="southWesternPoint"></param>
        /// <param name="northEastPoint"></param>
        public MapInfo(IntVector2 southWesternPoint, IntVector2 northEastPoint)
        {
            IsArgumentRangeOfRange(southWesternPoint, northEastPoint);

            this.southWesternPoint = southWesternPoint;
            this.northEastPoint = northEastPoint;
        }

        /// <summary>
        /// 指定地图大小,创建中心点为 CenterPoint 的地图;
        /// </summary>
        /// <param name="size"></param>
        public MapInfo(IntVector2 size)
        {
            GetPoint(ref size, out this.southWesternPoint, out this.northEastPoint);
        }

        [Header("地图大小;")]

        /// <summary>
        ///  最东北 方向的点,x,y都为正数;
        /// </summary>
        [SerializeField]
        [ProtoMember(2)]
        public IntVector2 northEastPoint;

        /// <summary>
        /// 最 西南 方向的点,x,y都为负数;
        /// </summary>
        [SerializeField]
        [ProtoMember(1)]
        public IntVector2 southWesternPoint;


        ///// <summary>
        ///// 地图最左下角的点;
        ///// </summary>
        //public IntVector2 SouthWesternPoint { get { return southWesternPoint; } }
        //public IntVector2 NorthEastPoint { get { return northEastPoint; } }


        /// <summary>
        /// 这个点是否存在于边界之外;若处于边界之外返回true,否则返回false;
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsOuterBoundary(IntVector2 position)
        {
            if (position.x > northEastPoint.x || position.x < southWesternPoint.x ||
                position.y > northEastPoint.y || position.y < southWesternPoint.y)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 若超出地图边界则返回异常OuterBoundaryException;
        /// </summary>
        /// <param name="position"></param>
        public void OuterBoundary(IntVector2 position)
        {
            if (IsOuterBoundary(position))
            {
                throw new OuterBoundaryException("传入的位置超出地图的定义的大小;");
            }
        }

        /// <summary>
        /// 检查输入值是否符合要求;若不符合则返回异常 ArgumentOutOfRangeException;
        /// </summary>
        /// <param name="southWesternPoint"></param>
        /// <param name="northEastPoint"></param>
        public static void IsArgumentRangeOfRange(IntVector2 southWesternPoint, IntVector2 northEastPoint)
        {
            if (southWesternPoint.x > 0 || southWesternPoint.y > 0 ||
                northEastPoint.x < 0 || northEastPoint.y < 0)
            {
                throw new ArgumentOutOfRangeException("输入值不符合要求!");
            }
        }

        /// <summary>
        /// 获取到地图大小;
        /// </summary>
        /// <returns></returns>
        public IntVector2 GetSize()
        {
            IntVector2 size = GetSize(southWesternPoint, northEastPoint);
            return size;
        }

        /// <summary>
        /// 获取到地图大小;
        /// </summary>
        /// <param name="southWesternPoint"></param>
        /// <param name="northEastPoint"></param>
        /// <returns></returns>
        public static IntVector2 GetSize(IntVector2 southWesternPoint, IntVector2 northEastPoint)
        {
            int x = Math.Abs((short)(southWesternPoint.x - northEastPoint.x));
            int y = Math.Abs((short)(southWesternPoint.y - northEastPoint.y));
            return new IntVector2(
                x > 0 ? ++x : x,
                y > 0 ? ++y : y);
        }

        /// <summary>
        /// 根据 大小 获取到最 西南 和 东北 方向的点;
        /// </summary>
        /// <param name="size"></param>
        /// <param name="southWesternPoint"></param>
        /// <param name="northEastPoint"></param>
        public static void GetPoint(ref IntVector2 size, out IntVector2 southWesternPoint, out IntVector2 northEastPoint)
        {
            size.x = Math.Abs(size.x);
            size.y = Math.Abs(size.y);

            int sizeX = size.x / 2;
            int sizeY = size.y / 2;

            northEastPoint = new IntVector2(sizeX, sizeY);
            southWesternPoint = new IntVector2(-sizeX, -sizeY);
        }

        /// <summary>
        /// 获取地图存在的 点 数目;
        /// </summary>
        public int GetPointCount()
        {
            IntVector2 size = GetSize();
            return size.x * size.y;
        }

        /// <summary>
        /// 获取到包含所有的点;
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IntVector2> GetEnumerablePoint()
        {
            for (int y = northEastPoint.y; y >= southWesternPoint.y; y--)
            {
                for (int x = southWesternPoint.x; x <= northEastPoint.x; x++)
                {
                    yield return new IntVector2(x, y);
                }
            }
        }

        /// <summary>
        /// 获取到包含所有的点;
        /// </summary>
        /// <param name="southWesternPoint"></param>
        /// <param name="northEastPoint"></param>
        /// <returns></returns>
        public static IEnumerable<IntVector2> GetEnumerablePoint(IntVector2 southWesternPoint, IntVector2 northEastPoint)
        {
            for (int y = northEastPoint.y; y >= southWesternPoint.y; y++)
            {
                for (int x = southWesternPoint.x; x <= northEastPoint.x; x++)
                {
                    yield return new IntVector2(x, y);
                }
            }
        }

        public override string ToString()
        {
            string str =
                "地图大小为 :" + GetSize() +
                "  最东北方向的点 :" + northEastPoint +
                "  最西南方向的点 :" + southWesternPoint + 
                "\n存在点总数 :" + GetPointCount();

            return str;
        }

        public bool Equals(MapInfo other)
        {
            bool isSame = southWesternPoint == other.southWesternPoint && northEastPoint == other.northEastPoint;
            return isSame;
        }

    }

}
