using System;
using System.Collections.Generic;
using System.Linq;
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
        public RoadMeshCreate()
        {
        }

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
        /// 创建这些区域的道路网格;
        /// </summary>
        public List<RoadMesh> Create(IEnumerable<CubicHexCoord> coords)
        {
            List<RoadMesh> roadMeshs = new List<RoadMesh>();

            foreach (var coord in coords)
            {
                try
                {
                    var meshs = Create(coord);
                    roadMeshs.AddRange(meshs);
                }
                catch(Exception e)
                {
                    Destroy(roadMeshs);
                    throw e;
                }
            }

            return roadMeshs;
        }

        void Destroy(ICollection<RoadMesh> meshs)
        {
            foreach (var mesh in meshs)
            {
                GameObject.Destroy(mesh.gameObject);
            }
            meshs.Clear();
        }

        /// <summary>
        /// 创建这个店通往周围的道路网格;
        /// </summary>
        public IEnumerable<RoadMesh> Create(CubicHexCoord coord)
        {
            foreach (var path in RoadInfo.FindPixelPaths(coord))
            {
                yield return Create(path);
            }
        }

        RoadMesh Create(IList<Vector3> paths)
        {
            var spline = CatmullRom.GetSpline(paths, segmentPoints);
            return CreateMesh(spline.ToList());
        }

        RoadMesh CreateMesh(IList<Vector3> spline)
        {
            GameObject gameObject = new GameObject("Road", typeof(RoadMesh));
            RoadMesh roadMesh = gameObject.GetComponent<RoadMesh>();
            roadMesh.SetSpline(spline, roadWidth);
            return roadMesh;
        }

    }

}
