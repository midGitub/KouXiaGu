//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ProtoBuf;
//using UnityEngine;

//namespace KouXiaGu.Map
//{

//    /// <summary>
//    /// 六边形的地图结构;
//    /// 不限制大小,根据IntVector2保存地图信息,可根据Vector2获取到unity空间转换成地图空间的方向;
//    /// </summary>
//    [ProtoContract]
//    ////继承的类也进行序列化;
//    //[ProtoInclude(1, typeof(HexDictionary))]
//    public class HexDictionary<T>
//    {

//        public HexDictionary()
//        {
//            this.m_MapDictionary = new Dictionary<IntVector2, T>();
//        }

//        public HexDictionary(float innerDiameter) : this()
//        {
//            this.InnerDiameter = innerDiameter;
//        }

//        public HexDictionary(int capacity)
//        {
//            this.m_MapDictionary = new Dictionary<IntVector2, T>(capacity);
//        }

//        private static readonly float tan30 = (float)Math.Tan(30 * (Math.PI / 180));

//        /// <summary>
//        /// 保存地图地图信息的结构;
//        /// </summary>
//        [ProtoMember(1)]
//        Dictionary<IntVector2, T> m_MapDictionary;

//        /// <summary>
//        /// 六边形内径;
//        /// </summary>
//        [ProtoMember(2)]
//        public float InnerDiameter { get; set; }

//        /// <summary>
//        /// 基于这个六边形创建的地图;
//        /// </summary>
//        public Beehive hexagon { get; private set; }


//        ///// <summary>
//        ///// 从0开始,获取到这个 数 与 (间隔 * n) 最为相近的数;
//        ///// </summary>
//        ///// <param name="x"></param>
//        ///// <param name="spacing"></param>
//        ///// <returns></returns>
//        //private float GetSimilar(float x, float spacing)
//        //{
//        //    float remainder = x % spacing;

//        //    if (remainder > 0)
//        //    {
//        //        return x + (remainder > spacing ? spacing - remainder : -remainder);
//        //    }
//        //    else if (remainder < 0)
//        //    {
//        //        return x - (remainder > -spacing ? remainder : spacing + remainder);
//        //    }
//        //    else
//        //    {
//        //        return x;
//        //    }
//        //}

//        ///// <summary>
//        ///// 从0开始获取到相近的奇数间隔(0不在获取范围);
//        ///// </summary>
//        ///// <param name="x"></param>
//        ///// <param name="spacing"></param>
//        ///// <returns></returns>
//        //private float GetSimilarOdd(float x, float spacing)
//        //{
//        //    float remainder = x % (2 * spacing);

//        //    if (remainder > 0)
//        //    {
//        //        return x + spacing - remainder;
//        //    }
//        //    else if (remainder < 0)
//        //    {
//        //        return x - remainder - spacing;
//        //    }
//        //    else
//        //    {
//        //        return spacing > 0 ? x + spacing : x - spacing;
//        //    }
//        //}

//        ///// <summary>
//        ///// 从0开始,获取到这个 数 在 (间隔 * n+1) 这类数的区间;
//        ///// y1 为远离0的一端, y2 为靠近0的这端;
//        ///// </summary>
//        ///// <param name="x"></param>
//        ///// <param name="spacing"></param>
//        ///// <param name="y1"></param>
//        ///// <param name="y2"></param>
//        //private void GetInterval(float x, float spacing, out float y1, out float y2)
//        //{
//        //    float remainder = x % spacing;

//        //    if (remainder > 0)
//        //    {
//        //        y2 = x - remainder;
//        //        y1 = y2 + spacing;
//        //    }
//        //    else if (remainder < 0)
//        //    {
//        //        y2 = x - remainder;
//        //        y1 = y2 - spacing;
//        //    }
//        //    else
//        //    {
//        //        y1 = x;
//        //        y2 = x;
//        //    }
//        //}

//        ///// <summary>
//        ///// 获取到最为靠近的六边形的中心点;
//        ///// </summary>
//        ///// <param name="position"></param>
//        ///// <returns></returns>
//        //public Vector2 PointFind(Vector2 position)
//        //{
//        //    Vector2 point1;
//        //    Vector2 point2;

//        //    float x1;
//        //    float x2;
//        //    float y1;
//        //    float y2;

//        //    GetInterval(position.y, InnerDiameter / tan30, out y1, out y2);
//        //    float x = GetSimilarOdd(position.x, InnerDiameter / 2);

//        //    if (x > 0)
//        //    {
//        //        if ((x % (InnerDiameter * 1.5f)) == 0)
//        //        {
//        //            x1 = x + InnerDiameter / 2;
//        //            point1 = new Vector2(x1, y2);
//        //            x2 = x - InnerDiameter / 2;
//        //            point2 = new Vector2(x2, y1);
//        //        }
//        //        else
//        //        {
//        //            x1 = x + InnerDiameter / 2;
//        //            point1 = new Vector2(x1, y1);
//        //            x2 = x - InnerDiameter / 2;
//        //            point2 = new Vector2(x2, y2);
//        //        }
//        //    }
//        //    else
//        //    {
//        //        if ((x % (InnerDiameter * 1.5f)) == 0)
//        //        {
//        //            x1 = x + InnerDiameter / 2;
//        //            point1 = new Vector2(x1, y1);
//        //            x2 = x - InnerDiameter / 2;
//        //            point2 = new Vector2(x2, y2);
//        //        }
//        //        else
//        //        {
//        //            x1 = x + InnerDiameter / 2;
//        //            point1 = new Vector2(x1, y2);
//        //            x2 = x - InnerDiameter / 2;
//        //            point2 = new Vector2(x2, y1);
//        //        }
//        //    }
//        //    return Vector2.Distance(point1, position) < Vector2.Distance(point2, position) ? point1 : point2;
//        //}

//        ///// <summary>
//        ///// 获取到最为靠近的六边形的中心点;
//        ///// </summary>
//        ///// <param name="position"></param>
//        ///// <returns></returns>
//        //public Vector2 PointFind2(Vector2 position)
//        //{
//        //    Vector2 point1;
//        //    Vector2 point2;

//        //    float x1;
//        //    float x2;
//        //    float y1;
//        //    float y2;

//        //    GetInterval(position.x, InnerDiameter, out x1, out x2);
//        //    GetInterval(position.y, InnerDiameter / tan30, out y1, out y2);

//        //    if ((x1 % (InnerDiameter * 2)) == 0)
//        //    {
//        //        if (((int)Math.Round((y1 / (InnerDiameter / tan30))) & 1) == 0)
//        //        {
//        //            point1 = new Vector2(x1, y1);
//        //            point2 = new Vector2(x2, y2);
//        //        }
//        //        else
//        //        {
//        //            point1 = new Vector2(x1, y2);
//        //            point2 = new Vector2(x2, y1);
//        //        }
//        //    }
//        //    else
//        //    {
//        //        if (((int)Math.Round((y1 / (InnerDiameter / tan30))) & 1) == 0)
//        //        {
//        //            point1 = new Vector2(x1, y2);
//        //            point2 = new Vector2(x2, y1);
//        //        }
//        //        else
//        //        {
//        //            point1 = new Vector2(x1, y1);
//        //            point2 = new Vector2(x2, y2);
//        //        }
//        //    }

//        //    //Debug.Log(point1.ToString() + point2.ToString() + "\n" +
//        //    //   "x1:" + x1 + "x2:" + x2 + "y1:" + y1 + "y2:" + y2);
//        //    return Vector2.Distance(point1, position) < Vector2.Distance(point2, position) ? point1 : point2;
//        //}

//        ///// <summary>
//        ///// 从unity空间坐标转换成地图空间坐标;
//        ///// </summary>
//        ///// <returns>地图空间坐标</returns>
//        //public IntVector2 WorldToMap(Vector2 position)
//        //{
//        //    int x;
//        //    int y;
//        //    position = PointFind2(position);

//        //    //Debug.Log((int)Math.Round((position.y / (InnerDiameter / tan30))));
//        //    //Debug.Log((InnerDiameter / tan30));
//        //    if (((int)Math.Round((position.y / (InnerDiameter / tan30))) & 1) == 0)
//        //    {
//        //        x = (int)Math.Round((position.x / (InnerDiameter * 2)));
//        //        y = (int)Math.Round((position.y / (InnerDiameter / tan30)));
//        //    }
//        //    else
//        //    {
//        //        x = (int)Math.Round(((position.x + InnerDiameter) / (InnerDiameter * 2)));
//        //        y = (int)Math.Round((position.y / (InnerDiameter / tan30)));
//        //    }

//        //    return new IntVector2(x, y);
//        //}

//        ///// <summary>
//        ///// 从地图空间坐标转换成unity空间坐标;
//        ///// </summary>
//        ///// <param name="position"></param>
//        ///// <returns></returns>
//        //public Vector2 MapToWorld(IntVector2 position)
//        //{
//        //    throw new NotImplementedException();
//        //}



//    }

//}
