using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 顶点坐标合集转换,仅绕y轴进行变换;
    /// 默认旋转角度为0,即下一点坐标为+(0, 1, 0);
    /// </summary>
    [Serializable]
    public class WallSection
    {

        public WallSection()
        {
            this.AnchorPoint = Vector3.zero;
            checkPoints = new List<CheckPoint>();
        }

        public WallSection(Vector3 anchorPoint)
        {
            this.AnchorPoint = anchorPoint;
            checkPoints = new List<CheckPoint>();
        }

        public WallSection(Vector3 anchorPoint, IEnumerable<CheckPoint> checkPoints)
        {
            this.AnchorPoint = anchorPoint;
            checkPoints = new List<CheckPoint>(checkPoints);
        }


        [SerializeField]
        Vector3 anchorPoint;

        /// <summary>
        /// 所有记录点;
        /// </summary>
        [SerializeField]
        List<CheckPoint> checkPoints;


        /// <summary>
        /// 所有记录点;
        /// </summary>
        public IList<CheckPoint> CheckPoints
        {
            get { return checkPoints; }
        }

        /// <summary>
        /// 锚点;
        /// </summary>
        public Vector3 AnchorPoint
        {
            get { return anchorPoint; }
            private set { anchorPoint = value; }
        }

        /// <summary>
        /// 顶点总数;
        /// </summary>
        public int Count
        {
            get { return checkPoints.Count; }
        }


        /// <summary>
        /// 重新计算并且返回检查点;
        /// </summary>
        /// <param name="newAnchorPoint">新的锚点</param>
        /// <param name="nextPoint">下一个点,面向的坐标;</param>
        public IEnumerable<CheckPoint> Recalculate(Vector3 newAnchorPoint, Vector3 nextPoint)
        {
            float rotationAngle = AngleY(newAnchorPoint, nextPoint);
            return Recalculate(newAnchorPoint, rotationAngle);
        }

        /// <summary>
        /// 重新计算并且返回检查点;
        /// </summary>
        public IEnumerable<CheckPoint> Recalculate(Vector3 newAnchorPoint, float rotationAngle)
        {
            foreach (var checkPoint in checkPoints)
            {
                Vector3 newPoint = Transfrom(checkPoint.Point, rotationAngle) + newAnchorPoint;
                yield return new CheckPoint(checkPoint.ID, newPoint);
            }
        }

        /// <summary>
        /// 转换为旋转后的坐标,原点(0,0,0);
        /// </summary>
        Vector3 Transfrom(Vector3 point, float rotationAngle)
        {
            float angle = AngleY(AnchorPoint, point);
            float radius = Distance(AnchorPoint, point);
            float height = point.y - AnchorPoint.y;

            Vector3 result = Circle(radius, rotationAngle + angle);
            result.y += height;
            return result;
        }


        /// <summary>
        /// 获取到半径圆上任何一点,忽略Y轴;
        /// </summary>
        Vector3 Circle(float radius, float angle)
        {
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;
            return new Vector3(x, 0, y);
        }

        /// <summary>
        /// 获取到忽略Y轴的距离;
        /// </summary>
        float Distance(Vector3 point1, Vector3 point2)
        {
            return Mathf.Sqrt(
                Mathf.Pow(point2.x - point1.x, 2) +
                Mathf.Pow(point2.z - point1.z, 2));
        }

        /// <summary>
        /// 返回弧度;
        /// </summary>
        float AngleY(Vector3 from, Vector3 to)
        {
            return Mathf.Atan2((to.x - from.x), (to.z - from.z));
        }

    }

}
