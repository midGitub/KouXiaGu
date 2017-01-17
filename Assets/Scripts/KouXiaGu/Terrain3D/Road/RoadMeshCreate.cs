using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 根据地形信息生成道路网格;
    /// </summary>
    [Serializable]
    public class RoadMeshCreate
    {

        public RoadMeshCreate(Road road)
        {
            this.RoadInfo = road;
        }


        [SerializeField]
        int segmentPoints;

        [SerializeField]
        float roadWidth;

        public Road RoadInfo { get; set; }


        /// <summary>
        /// 创建这个店通往周围的道路网格;
        /// </summary>
        public void Create(CubicHexCoord coord)
        {
            foreach (var path in RoadInfo.FindPixelPaths(coord))
            {
                Create(path);
            }
        }

        void Create(IList<Vector3> paths)
        {
            IEnumerable<Vector3> spline = CatmullRom.GetSpline(paths, segmentPoints);
            CreateMesh(spline.ToList());
        }

        void CreateMesh(IList<Vector3> spline)
        {
            GameObject gameObject = new GameObject("Road", typeof(RoadMesh));
            RoadMesh roadMesh = gameObject.GetComponent<RoadMesh>();
            roadMesh.SetSpline(spline);
        }

    }

}
