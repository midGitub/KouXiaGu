using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using KouXiaGu.Grids;
using UnityEngine;
using System.Collections;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 建筑物创建接口,需要挂载在预制物体上;
    /// </summary>
    public interface IBuilding
    {
        void Build(CubicHexCoord coord, Landform landform, IWorldData data);
    }

    /// <summary>
    /// 对场景建筑物进行构建;
    /// </summary>
    public class BuildingBuilder
    {
        public BuildingBuilder()
        {
            sceneChunks = new Dictionary<RectCoord, BuildingChunk>();
        }

        readonly Dictionary<RectCoord, BuildingChunk> sceneChunks;

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

    public class BuildingChunk : IEnumerable<BuildingChunk.BuildingItem>
    {
        public BuildingChunk()
        {
            buildingGroup = new List<BuildingItem>();
        }

        readonly List<BuildingItem> buildingGroup;

        public void Add(CubicHexCoord position, GameObject buildingObject)
        {
            BuildingItem item = new BuildingItem(position, buildingObject);
            buildingGroup.Add(item);
        }

        public GameObject GetAt(CubicHexCoord position)
        {
            int index = FindIndex(position);
            if (index < 0)
            {
                throw new KeyNotFoundException();
            }
            return buildingGroup[index].BuildingObject;
        }

        int FindIndex(CubicHexCoord position)
        {
            return buildingGroup.FindIndex(item => item.Position == position);
        }

        public IEnumerator<BuildingItem> GetEnumerator()
        {
            return buildingGroup.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct BuildingItem
        {
            public BuildingItem(CubicHexCoord position, GameObject buildingObject)
            {
                this.position = position;
                this.buildingObject = buildingObject;
            }

            readonly CubicHexCoord position;
            readonly GameObject buildingObject;

            public CubicHexCoord Position
            {
                get { return position; }
            }

            public GameObject BuildingObject
            {
                get { return buildingObject; }
            }
        }
    }
}
