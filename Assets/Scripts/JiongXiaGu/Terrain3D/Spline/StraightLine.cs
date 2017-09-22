using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.Terrain3D
{


    public class StraightLine : ISpline
    {
        public StraightLine(Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
        }

        Vector3 start, end;

        public Vector3 InterpolatedPoint(float t)
        {
            return Vector3.LerpUnclamped(start, end, t);
        }
    }
}
