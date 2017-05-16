using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using KouXiaGu.Grids;
using UnityEngine;
using System.Collections;
using KouXiaGu.World.Map;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 建筑物创建接口,需要挂载在预制物体上;
    /// </summary>
    public interface ILandformBuilding
    {
        GameObject Build(CubicHexCoord coord, MapNode node, LandformManager landform, IWorldData data);
    }

    /// <summary>
    /// 对场景建筑物进行构建;
    /// </summary>
    public class BuildingBuilder
    {
        public BuildingBuilder(IWorldData worldData, LandformManager landform)
        {
            this.worldData = worldData;
            sceneChunks = new Dictionary<RectCoord, BuildingChunk>();
            readOnlySceneChunks = sceneChunks.AsReadOnlyDictionary();
        }

        readonly IWorldData worldData;
        readonly Dictionary<RectCoord, BuildingChunk> sceneChunks;
        readonly IReadOnlyDictionary<RectCoord, BuildingChunk> readOnlySceneChunks;

        public IReadOnlyDictionary<RectCoord, BuildingChunk> SceneChunks
        {
            get { return readOnlySceneChunks; }
        }

        TerrainResource Resources
        {
            get { return worldData.GameData.Terrain; }
        }

        RectGrid ChunkGrid
        {
            get { return ChunkInfo.ChunkGrid; }
        }

        #region 建筑物覆盖节点;

        /// <summary>
        /// (0, 0)对应覆盖的节点(依赖地图块大小);
        /// </summary>
        static readonly CubicHexCoord[] buildingOverlay = new CubicHexCoord[]
            {
                new CubicHexCoord(-2, 2),
                new CubicHexCoord(-2, 1),
                new CubicHexCoord(-2, 0),

                new CubicHexCoord(-1, 2),
                new CubicHexCoord(-1, 1),
                new CubicHexCoord(-1, 0),

                new CubicHexCoord(0, 1),
                new CubicHexCoord(0, 0),
                new CubicHexCoord(0, -1),

                new CubicHexCoord(1, 1),
                new CubicHexCoord(1, 0),
                new CubicHexCoord(1, -1),
            };

        /// <summary>
        /// 获取到地形块对应覆盖到的建筑物坐标;
        /// </summary>
        public static IEnumerable<CubicHexCoord> GetOverlayPoints(RectCoord chunkCoord)
        {
            CubicHexCoord chunkCenter = ChunkInfo.ChunkGrid.GetCenter(chunkCoord).GetTerrainCubic();
            foreach (var item in buildingOverlay)
            {
                yield return chunkCenter + item;
            }
        }

        #endregion

        MapNode GetAt(CubicHexCoord coord)
        {
            return worldData.Map.Data[coord];
        }

        BuildingResource FindResource(int id)
        {
            Dictionary<int, BuildingResource> resources = worldData.GameData.Terrain.BuildingInfos;
            return resources[id];
        }

        /// <summary>
        /// 仅创建对应块,若已经存在则返回存在的元素;
        /// </summary>
        public BuildingChunk Create(RectCoord chunkCoord)
        {
            BuildingChunk chunk;
            if (!sceneChunks.TryGetValue(chunkCoord, out chunk))
            {
                chunk = new BuildingChunk();
                IEnumerable<CubicHexCoord> overlayPoints = GetOverlayPoints(chunkCoord);
                foreach (var overlayPoint in overlayPoints)
                {
                    MapNode node = GetAt(overlayPoint);
                    BuildingResource resource = FindResource(node.Building.Type);
                    var building = resource.Building.Build(overlayPoint, node, null, worldData);
                    chunk.Add(overlayPoint, building);
                }
                sceneChunks.Add(chunkCoord, chunk);
            }
            return chunk;
        }

        /// <summary>
        /// 仅更新对应地形块,若不存在对应地形块,则返回Null;
        /// </summary>
        public void Update(RectCoord chunkCoord)
        {
        }

        /// <summary>
        /// 销毁这个地图块;
        /// </summary>
        public void Destroy(RectCoord chunkCoord)
        {

        }

        /// <summary>
        /// 清除所有实例化的物体;
        /// </summary>
        void Clear(BuildingChunk chunk)
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
