using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public class BezierCurve : MonoBehaviour
    {

        public Vector3[] points;

        void Reset()
        {
            points = new Vector3[]
            {
                new Vector3(1,0,0),
                new Vector3(2,0,0),
                new Vector3(3,0,0),
                new Vector3(4,0,0),
            };
        }

    }

}
