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
    [ExecuteInEditMode]
    public class CatmullRomSpline : MonoBehaviour
    {

        public Vector3[] Points;

        public int SegmentPoints;

        public List<Vector3> NewPoints;

        LineRenderer lineRednerer;

        //void Awake()
        //{
        //    lineRednerer = GetComponent<LineRenderer>();
        //    //lineRednerer.numPositions = SegmentPoints;
        //}

        //[ContextMenu("更新")]
        //void Update()
        //{
        //    Vector3[] array = NewPoints.ToArray();
        //    lineRednerer.numPositions = array.Length;
        //    lineRednerer.SetPositions(array);
        //}

            [ContextMenu("设置网格;")]
        void Set()
        {
            GetComponent<RoadRenderer>().SetSpline(NewPoints);

        }

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
