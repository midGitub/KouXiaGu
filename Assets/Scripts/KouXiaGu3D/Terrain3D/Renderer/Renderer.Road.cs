using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public sealed partial class Renderer : UnitySington<Renderer>
    {

        /// <summary>
        /// 地形道路装饰;
        /// </summary>
        [Serializable]
        class RoadDecorate : IDecorate
        {
            [SerializeField]
            MeshDisplay displayMeshPool;

            public RenderTexture DiffuseAdjustRT { get; private set; }

            public RenderTexture HeightAdjustRT { get; private set; }

            public void Awake()
            {
                displayMeshPool.Awake();
            }

            public IEnumerator Rander(IBakeRequest request, IEnumerable<CubicHexCoord> bakingPoints)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// 获取到这些点的道路信息;
            /// </summary>
            List<RoadNode> GetRoads(IDictionary<CubicHexCoord, TerrainNode> map, IEnumerable<CubicHexCoord> coords)
            {
                List<RoadNode> roads = new List<RoadNode>();
                foreach (var coord in coords)
                {
                    try
                    {
                        var road = new RoadNode(map, coord);
                        roads.Add(road);
                    }
                    catch (ObjectNotExistedException)
                    {
                        continue;
                    }
                }
                return roads;
            }

            public void ReleaseAll()
            {
                RenderTexture.ReleaseTemporary(DiffuseAdjustRT);
                RenderTexture.ReleaseTemporary(HeightAdjustRT);
            }
        }

    }

}
