using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// CatmullRom 样条曲线;
    /// </summary>
    public class CatmullRomSpline : ISpline
    {
        public CatmullRomSpline(params Vector3[] points)
        {
            if (points.Length < 4)
            {
                throw new ArgumentException("点数量小余4;");
            }

            this.points = points;
        }

        readonly Vector3[] points;

        public Vector3 InterpolatedPoint(float t)
        {
            const float one = 1;
            int segmentCount = points.Length - 3;
            int segmentIndex = (int)(t / (one / segmentCount));
            segmentIndex = MathI.Clamp(segmentIndex, 0, segmentCount - 1);
            float localT = t * segmentCount - segmentIndex;

            Vector3 p0 = points[segmentIndex];
            Vector3 p1 = points[segmentIndex + 1];
            Vector3 p2 = points[segmentIndex + 2];
            Vector3 p3 = points[segmentIndex + 3];

            return InterpolatedPoint(p0, p1, p2, p3, localT);
        }

        /// <summary>
        /// 获取到插值;
        /// </summary>
        public static Vector3 InterpolatedPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            double u3 = t * t * t;
            double u2 = t * t;

            double f1 = -0.5 * u3 + u2 - 0.5 * t;
            double f2 = 1.5 * u3 - 2.5 * u2 + 1.0;
            double f3 = -1.5 * u3 + 2.0 * u2 + 0.5 * t;
            double f4 = 0.5 * u3 - 0.5 * u2;

            double x = p0.x * f1 + p1.x * f2 + p2.x * f3 + p3.x * f4;
            double z = p0.z * f1 + p1.z * f2 + p2.z * f3 + p3.z * f4;

            return new Vector3((float)x, 0f, (float)z);
        }

        /// <summary>
        /// 获取到完整的样条曲线路径;与 GetSpline() 不同于 points 参数;
        /// </summary>
        /// <param name="points">最少需要2个点</param>
        /// <param name="segmentPoints">分段点数</param>
        /// <returns>迭代结构;</returns>
        public static IEnumerable<Vector3> GetFullSpline(IList<Vector3> points, int segmentPoints)
        {
            if (points == null)
                throw new ArgumentNullException();
            if (points.Count < 2)
                throw new ArgumentException();

            float segment = Math.Abs(1f / segmentPoints);
            int endIndex = points.Count - 1;

            for (var i = 0; i < endIndex; i++)
            {
                Vector3 p0 = points[Math.Max(0, i - 1)];
                Vector3 p1 = points[i];
                Vector3 p2 = points[Math.Min(i + 1, endIndex)];
                Vector3 p3 = points[Math.Min(i + 2, endIndex)];

                for (float t = 0; t < 1; t += segment)
                {
                    var pos = InterpolatedPoint(p0, p1, p2, p3, t);
                    yield return pos;
                }
            }
            yield return points[endIndex];
        }


        /// <summary>
        /// 获取到曲线;
        /// </summary>
        /// <param name="points">最少需要4个点</param>
        /// <param name="segmentPoints">分段点数</param>
        /// <returns>迭代结构;</returns>
        public static IEnumerable<Vector3> GetSpline(IList<Vector3> points, int segmentPoints)
        {
            if (points == null)
                throw new ArgumentNullException();
            if (points.Count < 4)
                throw new ArgumentException();

            float segment = Math.Abs(1f / segmentPoints);
            int endIndex = points.Count - 2;

            for (var i = 1; i < endIndex; i++)
            {
                Vector3 p0 = points[i - 1];
                Vector3 p1 = points[i];
                Vector3 p2 = points[i + 1];
                Vector3 p3 = points[i + 2];

                for (float t = 0; t < 1; t += segment)
                {
                    var pos = InterpolatedPoint(p0, p1, p2, p3, t);
                    yield return pos;
                }
            }
            yield return points[endIndex];
        }

    }

}
