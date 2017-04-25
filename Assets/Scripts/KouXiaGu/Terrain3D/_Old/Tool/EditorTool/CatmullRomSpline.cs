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

        [SerializeField,Range(0.01f, 2f)]
        public float roadWidth;

        [ContextMenu("初始化完整网格;")]
        void SetFull()
        {
            IEnumerable<Vector3> spline = CatmullRom.GetFullSpline(Points, SegmentPoints);
            List<Vector3> path = new List<Vector3>(spline);
            GetComponent<RoadMesh>().SetSpline(path, roadWidth);
        }

        [ContextMenu("初始化网格;")]
        void Set()
        {
            IEnumerable<Vector3> spline = CatmullRom.GetSpline(Points, SegmentPoints);
            List<Vector3> path = new List<Vector3>(spline);
            GetComponent<RoadMesh>().SetSpline(path, roadWidth);
            Debug.Log(path.ToLog());
        }

        //void OnValidate()
        //{
        //    SetFull();
        //}

    }

}
