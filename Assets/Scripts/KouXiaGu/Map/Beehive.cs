using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 2D 蜂巢结构;
    /// 用于表示地图坐标和世界坐标的相对位置;
    /// </summary>
    [Serializable]
    [ProtoContract]
    public struct Beehive
    {
        public Beehive(float innerDiameter)
        {
            this.m_InnerDiameter = innerDiameter;
        }

        private static readonly float tan30 = (float)Math.Tan(30 * (Math.PI / 180));

        private static readonly float cos30 = (float)Math.Cos(30 * (Math.PI / 180));


        [SerializeField]
        [ProtoMember(1)]
        private float m_InnerDiameter;

        public float InnerDiameter
        {
            get { return m_InnerDiameter; }
            set { m_InnerDiameter = value; }
        }

        public float OuterDiameter
        {
            get { return (m_InnerDiameter / 2) / cos30; }
        }

        /// <summary>
        /// 边长;
        /// </summary>
        public float Length
        {
            get { return OuterDiameter; }
        }

        /// <summary>
        /// Y 轴方向的间距;
        /// </summary>
        public float LengthY
        {
            get { return (InnerDiameter / 2) / tan30; }
        }

        /// <summary>
        /// 将世界点转换成 六边形中心点 和 六边形编号;
        /// </summary>
        /// <param name="point"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public KeyValuePair<Vector2, IntVector2> WorldToNumber(Vector2 point)
        {
            int x1, x2, y1, y2;
            KeyValuePair<Vector2, IntVector2>[] points = new KeyValuePair<Vector2, IntVector2>[3];

            GetInterval(point.x, InnerDiameter, out x1, out x2);
            GetInterval(point.y, LengthY, out y1, out y2);

            //若 y1 不属于偶数行;
            if ((y1 & 1) == 1)
            {
                y1 = y1 ^ y2;
                y2 = y2 ^ y1;
                y1 = y1 ^ y2;
            }

            points[0] = new KeyValuePair<Vector2, IntVector2>(NumberToPosition(x1, y1), new IntVector2(x1, y1));
            points[1] = new KeyValuePair<Vector2, IntVector2>(NumberToPosition(x2, y1), new IntVector2(x2, y1));
            x1 = Math.Min(x1, x2);
            points[2] = new KeyValuePair<Vector2, IntVector2>(NumberToPosition(x1, y2), new IntVector2(x1, y2));
            return GetMin(point, points);
        }

        /// <summary>
        /// 将 六边形编号 转换成 六边形中心点;
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector2 NumberToPosition(int x, int y)
        {
            Vector2 position = new Vector2();
            position.x = InnerDiameter * x;
            position.y = LengthY * y;

            //若y为奇数;
            if ((y & 1) == 1)
                position.x += (InnerDiameter / 2);
            return position;
        }

        /// <summary>
        /// 获取到这个数所在的整数区间;
        /// intPoint1 为靠近0的点, intPoint2 为远离0的点;
        /// </summary>
        /// <param name="point"></param>
        /// <param name="spacing"></param>
        /// <param name="intPoint1"></param>
        /// <param name="intPoint2"></param>
        private void GetInterval(float point, float spacing, out int intPoint1, out int intPoint2)
        {
            intPoint1 = (int)(point / spacing);
            if (point > 0)
            {
                intPoint2 = intPoint1 + 1;
            }
            else if (point < 0)
            {
                intPoint2 = intPoint1 - 1;
            }
            else
            {
                intPoint2 = intPoint1;
            }
        }

        /// <summary>
        /// 获取到距离目标最近的点;
        /// </summary>
        /// <param name="target"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        private KeyValuePair<Vector2, IntVector2> GetMin(Vector2 target, params KeyValuePair<Vector2, IntVector2>[] points)
        {
            float minDis = InnerDiameter;
            KeyValuePair<Vector2, IntVector2> min = default(KeyValuePair<Vector2, IntVector2>);
            foreach (var point in points)
            {
                float dis = Vector2.Distance(target, point.Key);
                if (dis < minDis)
                {
                    minDis = dis;
                    min = point;
                }
            }
            return min;
        }



    }

}
