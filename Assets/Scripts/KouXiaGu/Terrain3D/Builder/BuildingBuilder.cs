using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 建筑物创建接口,需要挂载在预制物体上;
    /// </summary>
    public interface IBuilding
    {
        void Build(CubicHexCoord coord, Landform landform, IWorldData data);
        void Destroy();
    }

    /// <summary>
    /// 对场景建筑物进行构建;
    /// </summary>
    public class BuildingBuilder
    {
        public BuildingBuilder()
        {
        }

        /// <summary>
        /// 仅创建对应块,若已经存在则返回存在的元素;
        /// </summary>
        public void Create(RectCoord chunkCoord)
        {
        }

        /// <summary>
        /// 仅更新对应地形块,若不存在对应地形块,则返回Null;
        /// </summary>
        public void Update(RectCoord chunkCoord)
        {
        }
    }

    public class BuildingChunk
    {
        public BuildingChunk()
        {
            buildingGroup = new List<Building>();
        }

        readonly List<Building> buildingGroup;

    }

    class Building
    {
        readonly IBuilding builder;
        readonly GameObject gameObject;
    }
}
