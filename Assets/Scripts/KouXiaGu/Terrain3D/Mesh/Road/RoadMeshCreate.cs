//using System;
//using System.Collections.Generic;
//using System.Linq;
//using KouXiaGu.Grids;
//using KouXiaGu.World.Map;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 根据地形信息生成道路网格;
//    /// </summary>
//    [Serializable]
//    public class RoadMeshCreate
//    {
//        public RoadMeshCreate()
//        {
//        }

//        public RoadMeshCreate(IDictionary<CubicHexCoord, MapNode> road)
//        {
//            this.Map = road;
//        }

//        [SerializeField, Range(4, 60)]
//        int segmentPoints = 16;

//        [SerializeField]
//        float roadWidth = 0.07f;

//        public IDictionary<CubicHexCoord, MapNode> Map { get; set; }


//        /// <summary>
//        /// 创建这些区域的道路网格;
//        /// </summary>
//        public List<RoadMesh> Create(IEnumerable<CubicHexCoord> coords)
//        {
//            List<RoadMesh> roadMeshs = new List<RoadMesh>();

//            foreach (var coord in coords)
//            {
//                try
//                {
//                    var meshs = Create(coord);
//                    roadMeshs.AddRange(meshs);
//                }
//                catch(Exception e)
//                {
//                    Destroy(roadMeshs);
//                    throw e;
//                }
//            }

//            return roadMeshs;
//        }

//        void Destroy(ICollection<RoadMesh> meshs)
//        {
//            foreach (var mesh in meshs)
//            {
//                GameObject.Destroy(mesh.gameObject);
//            }
//            meshs.Clear();
//        }

//        /// <summary>
//        /// 创建这个店通往周围的道路网格;
//        /// </summary>
//        public IEnumerable<RoadMesh> Create(CubicHexCoord coord)
//        {
//            foreach (var path in FindPixelPaths(coord))
//            {
//                yield return Create(path);
//            }
//        }


//        /// <summary>
//        /// 获取到这个点通向周围的像素路径;
//        /// </summary>
//        public IEnumerable<Vector3[]> FindPixelPaths(CubicHexCoord target)
//        {
//            IEnumerable<CubicHexCoord[]> paths = Map.FindPaths(target);
//            return Convert(paths);
//        }

//        /// <summary>
//        /// 转换 地图坐标 到 像素坐标;
//        /// </summary>
//        public IEnumerable<Vector3[]> Convert(IEnumerable<CubicHexCoord[]> paths)
//        {
//            return paths.Select(delegate (CubicHexCoord[] path)
//            {
//                Vector3[] newPath = path.Select(coord => coord.GetTerrainPixel()).ToArray();
//                return newPath;
//            });
//        }


//        RoadMesh Create(IList<Vector3> paths)
//        {
//            var spline = CatmullRom.GetSpline(paths, segmentPoints);
//            return CreateMesh(spline.ToList());
//        }

//        RoadMesh CreateMesh(IList<Vector3> spline)
//        {
//            GameObject gameObject = new GameObject("Road", typeof(RoadMesh));
//            RoadMesh roadMesh = gameObject.GetComponent<RoadMesh>();
//            roadMesh.SetSpline(spline, roadWidth);
//            return roadMesh;
//        }

//    }

//}
