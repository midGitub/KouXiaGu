﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.Grids
{

    /// <summary>
    /// 铺满在场景的矩形网格;中心点为(0,0)
    /// </summary>
    public struct RectGrid
    {

        readonly float width;
        readonly float height;
        readonly float widthHalf;
        readonly float heightHalf;

        /// <summary>
        /// 矩形宽度;
        /// </summary>
        public float Width
        {
            get { return width; }
        }

        /// <summary>
        /// 矩形高度;
        /// </summary>
        public float Height
        {
            get { return height; }
        }

        public RectGrid(float size)
        {
            this.width = this.height = size;
            this.widthHalf = this.heightHalf = size / 2;
        }

        public RectGrid(float width, float height)
        {
            this.width = width;
            this.height = height;

            this.widthHalf = width / 2;
            this.heightHalf = height / 2;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RectGrid))
                return false;
            return (RectGrid)obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "(" + width + "," + height + ")";
        }

        /// <summary>
        /// 获取到所属的矩形;
        /// </summary>
        public RectCoord GetCoord(Vector3 position)
        {
            short x = (short)Math.Round(position.x / width);
            short y = (short)Math.Round(position.z / height);
            return new RectCoord(x, y);
        }

        /// <summary>
        /// 获取到矩形像素中心点;
        /// </summary>
        public Vector3 GetCenter(RectCoord coord)
        {
            float x = coord.X * width;
            float z = coord.Y * height;
            return new Vector3(x, 0, z);
        }

        /// <summary>
        /// 获取到矩形节点的矩形表示;
        /// </summary>
        public Rect GetRect(RectCoord coord)
        {
            Vector3 center = GetCenter(coord);
            return CenterToRect(center);
        }

        /// <summary>
        /// 获取到矩形的本地坐标;
        /// </summary>
        public Vector2 GetLocal(Vector3 position, out RectCoord coord)
        {
            coord = GetCoord(position);
            Vector3 center = GetCenter(coord);
            Vector2 southwestPoint = new Vector2(center.x - widthHalf, center.z - heightHalf);
            Vector2 local = new Vector2(position.x - southwestPoint.x, position.z - southwestPoint.y);
            return local;
        }

        /// <summary>
        /// 获取到指定矩形的本地坐标;
        /// </summary>
        public Vector2 GetLocal(RectCoord clamp, Vector3 position)
        {
            Vector3 center = GetCenter(clamp);
            Vector2 southwestPoint = new Vector2(center.x - widthHalf, center.z - heightHalf);
            Vector2 local = new Vector2(position.x - southwestPoint.x, position.z - southwestPoint.y);
            return local;
        }

        /// <summary>
        /// 获取到UV坐标并且限制在指定的矩形之内;
        /// </summary>
        public Vector2 GetUV(RectCoord clamp, Vector3 position)
        {
            Vector2 local = GetLocal(clamp, position);
            Vector2 uv = new Vector2(local.x / width, local.y / height);
            return Clamp01(uv);
        }

        /// <summary>
        /// 将其值限制在0~1之间;
        /// </summary>
        Vector2 Clamp01(Vector2 v1)
        {
            v1.x = Mathf.Clamp01(v1.x);
            v1.y = Mathf.Clamp01(v1.y);
            return v1;
        }

        /// <summary>
        /// 获取到矩形的UV坐标;
        /// </summary>
        public Vector2 GetUV(Vector3 position, out RectCoord coord)
        {
            Vector2 local = GetLocal(position, out coord);
            Vector2 uv = new Vector2(local.x / width, local.y / height);
            return uv;
        }

        /// <summary>
        /// 获取到这个矩形范围内的所有矩形元素;
        /// </summary>
        public IEnumerable<RectCoord> RectRange(Rect rect)
        {
            RectCoord southwest = GetCoord(rect.min);
            RectCoord northeast = GetCoord(rect.max);
            return RectCoord.Range(southwest, northeast);
        }

        /// <summary>
        /// 获取到矩形最左下角的坐标;
        /// </summary>
        Vector2 GetSouthwest(Vector3 blockCenter)
        {
            Vector2 southwestPoint = new Vector2(blockCenter.x - widthHalf, blockCenter.z - heightHalf);
            return southwestPoint;
        }

        /// <summary>
        /// 从矩形中心坐标 获取到其在场景中的矩形大小;
        /// </summary>
        Rect CenterToRect(Vector3 blockCenter)
        {
            Vector2 southwestPoint = GetSouthwest(blockCenter);
            Vector2 size = new Vector2(width, height);
            return new Rect(southwestPoint, size);
        }

        public static bool operator ==(RectGrid a, RectGrid b)
        {
            return a.width == b.width 
                && a.height == b.height;
        }

        public static bool operator !=(RectGrid a, RectGrid b)
        {
            return !(a == b);
        }

        public static RectGrid operator *(RectGrid rect, short n)
        {
            return new RectGrid(rect.width * n, rect.height * n);
        }

        public static RectGrid operator /(RectGrid rect, short n)
        {
            return new RectGrid(rect.width / n, rect.height / n);
        }

    }


}
