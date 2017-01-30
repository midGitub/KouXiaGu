using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 顶点坐标转换,仅绕y轴进行变换;
    /// </summary>
    public class WallSection : List<CheckPoint>
    {

        public WallSection(int id, Vector3 anchorPoint)
        {
            this.ID = id;
            this.AnchorPoint = anchorPoint;
        }


        /// <summary>
        /// 在曲线上的编号;
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 锚点;
        /// </summary>
        public Vector3 AnchorPoint { get; set; }


        /// <summary>
        /// 重新计算并且返回检查点;
        /// </summary>
        public IEnumerable<CheckPoint> RecalculatePoints(Vector3 nextPoint)
        {
            float rotationAngle = AngleY(AnchorPoint, nextPoint);
            return RecalculatePoints(rotationAngle);
        }

        /// <summary>
        /// 重新计算并且返回检查点;
        /// </summary>
        public IEnumerable<CheckPoint> RecalculatePoints(float rotationAngle)
        {
            foreach (var checkPoint in this)
            {
                Vector3 newPoint = TransfromPoint(checkPoint.Point, rotationAngle);
                yield return new CheckPoint(checkPoint.ID, newPoint);
            }
        }

        /// <summary>
        /// 转换为旋转后的坐标;
        /// </summary>
        Vector3 TransfromPoint(Vector3 point, float rotationAngle)
        {
            float angle = AngleY(AnchorPoint, point);
            float radius = Distance(AnchorPoint, point);
            float height = point.y - AnchorPoint.y;

            Vector3 result = Circle(radius, rotationAngle + angle) + AnchorPoint;
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
