using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public static class CatmullRom
    {

        /// <summary>
        /// 获取到样条曲线路径;
        /// </summary>
        /// <param name="segmentPoints">分段点数</param>
        /// <returns>迭代结构;</returns>
        public static IEnumerable<Vector3> GetPath(IList<Vector3> path, int segmentPoints)
        {
            if (path == null)
                throw new ArgumentNullException();

            float segment = Math.Abs(1f / segmentPoints);
            int endIndex = path.Count - 1;

            for (var i = 0; i < endIndex; i++)
            {
                Vector3 p0 = path[Math.Max(0, i - 1)];
                Vector3 p1 = path[i];
                Vector3 p2 = path[Math.Min(i + 1, endIndex)];
                Vector3 p3 = path[Math.Min(i + 2, endIndex)];

                for (float t = 0; t < 1; t += segment)
                {
                    var pos = InterpolatedPoint(p0, p1, p2, p3, t);
                    yield return pos;
                }
            }
            yield return path[endIndex];
        }

        /// <summary>
        /// 获取到插值; 0 <= t <= 1
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

    }

}
