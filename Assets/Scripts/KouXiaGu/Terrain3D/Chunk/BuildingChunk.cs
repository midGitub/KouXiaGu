using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 根据地形块创建其上的建筑物;
    /// </summary>
    public class BuildingChunk
    {

        /// <summary>
        /// 创建建筑块,并且实例化其建筑;
        /// </summary>
        BuildingChunk(RectCoord chunkCoord, BuildingData data)
        {
            this.ChunkCoord = chunkCoord;
            buildings = new Dictionary<CubicHexCoord, GameObject>(TerrainOverlayer.BuildingCount);
            Build(data);
        }


        public RectCoord ChunkCoord { get; private set; }
        Dictionary<CubicHexCoord, GameObject> buildings;


        public IEnumerable<GameObject> Buildings
        {
            get { return buildings.Values; }
        }


        void Build(BuildingData data)
        {
            IEnumerable<CubicHexCoord> overlayes = GetOverlaye(ChunkCoord);
            Build(data, overlayes);
        }

        void Build(BuildingData data, IEnumerable<CubicHexCoord> overlayes)
        {
            foreach (var coord in overlayes)
            {
                Build(data, coord);
            }
        }

        void Build(BuildingData data, CubicHexCoord coord)
        {
            BuildingNode node;
            if (data.TryGetValue(coord, out node))
            {
                GameObject prefab = GetBuildRes(node.ID).Prefab;
                Vector3 position = coord.GetTerrainPixel();
                float angle = node.Angle;

                var gameObject = GameObject.Instantiate(prefab, position, Quaternion.Euler(0, angle, 0));
                buildings.Add(coord, gameObject);
            }
            return;
        }

        BuildingRes GetBuildRes(int id)
        {
            try
            {
                return BuildingRes.initializedInstances[id];
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("缺少建筑资源 :" + id, ex);
            }
        }

        /// <summary>
        /// 获取到覆盖的坐标;
        /// </summary>
        IEnumerable<CubicHexCoord> GetOverlaye(RectCoord coord)
        {
            return TerrainOverlayer.GetBuilding(coord);
        }

        /// <summary>
        /// 单个建筑信息;
        /// </summary>
        class BuildingGroup
        {


            public GameObject BuildingObject { get; private set; }




        }

    }

}
