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
    /// 对场景建筑物进行构建;
    /// </summary>
    public class BuildingBuilder
    {
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
        static IEnumerable<CubicHexCoord> GetOverlayPoints(RectCoord chunkCoord)
        {
            CubicHexCoord chunkCenter = ChunkInfo.ChunkGrid.GetCenter(chunkCoord).GetTerrainCubic();
            foreach (var item in buildingOverlay)
            {
                yield return chunkCenter + item;
            }
        }


        public BuildingBuilder(IWorldData worldData, LandformManager landform)
        {
            this.worldData = worldData;
            sceneBuildings = new Dictionary<CubicHexCoord, ILandformBuilding>();
            sceneChunks = new HashSet<RectCoord>();
            readOnlySceneChunks = sceneChunks.AsReadOnlyCollection();
        }

        readonly IWorldData worldData;
        readonly Dictionary<CubicHexCoord, ILandformBuilding> sceneBuildings;
        readonly HashSet<RectCoord> sceneChunks;
        readonly IReadOnlyCollection<RectCoord> readOnlySceneChunks;

        /// <summary>
        /// 已经在场景中构建的块坐标;
        /// </summary>
        public IReadOnlyCollection<RectCoord> SceneChunks
        {
            get { return readOnlySceneChunks; }
        }

        RectGrid chunkGrid
        {
            get { return ChunkInfo.ChunkGrid; }
        }

        IDictionary<CubicHexCoord, MapNode> mapData
        {
            get { return worldData.Map.Data; }
        }

        IDictionary<int, BuildingResource> resources
        {
            get { return worldData.GameData.Terrain.BuildingInfos; }
        }

        /// <summary>
        /// 创建建筑物到该坐标,若不存在建筑物则返回false;
        /// </summary>
        bool TryCreate(CubicHexCoord coord, out ILandformBuilding building)
        {
            MapNode node;
            if (mapData.TryGetValue(coord, out node))
            {
                BuildingResource resource;
                int buildingType = node.Building.Type;
                if (resources.TryGetValue(buildingType, out resource))
                {
                    building = resource.Building.BuildAt(coord, node, null, worldData);
                    sceneBuildings.Add(coord, building);
                }
            }
            building = null;
            return false;
        }

        /// <summary>
        /// 仅创建对应块,若已经存在则返回存在的元素;
        /// </summary>
        public void Create(RectCoord chunkCoord)
        {
            var overlayPoints = GetOverlayPoints(chunkCoord);
            foreach (var point in overlayPoints)
            {
                
            }
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


        class BuildingCreateRequest : AsyncOperation<ILandformBuilding>, IRequest
        {
            public bool IsCanceled { get; private set; }


            void IRequest.Operate()
            {
                throw new NotImplementedException();
            }

            void IRequest.AddQueue()
            {
                throw new NotImplementedException();
            }

            void IRequest.OutQueue()
            {
                throw new NotImplementedException();
            }
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
