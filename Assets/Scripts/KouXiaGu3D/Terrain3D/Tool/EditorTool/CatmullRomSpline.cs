using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 用于演示曲线;
    /// </summary>
    public class CatmullRomSpline : MonoBehaviour
    {

        public Vector3[] Points;

        public int SegmentPoints;

        public List<Vector3> NewPoints;


        void Reset()
        {
            Points = new Vector3[]
                {
                    new Vector3(1,0,0),
                    new Vector3(2,0,0),
                    new Vector3(3,3,0),
                    new Vector3(4,4,0),
                    new Vector3(3,2,0),
                };
        }


    }

}
