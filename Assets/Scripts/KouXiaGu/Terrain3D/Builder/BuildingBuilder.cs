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
        GameObject gameObject { get; }
        ILandformBuilding BuildAt(CubicHexCoord coord, MapNode node, LandformManager landform, IWorldData data);
        void Destroy();
    }

    /// <summary>
    /// 对场景建筑物进行构建;
    /// </summary>
    public class BuildingBuilder
    {
        public BuildingBuilder(IWorldData worldData, Landform landform, RequestDispatcher requestDispatcher)
        {
            this.worldData = worldData;
            this.landform = landform;
            this.requestDispatcher = requestDispatcher;
            sceneBuildings = new Dictionary<CubicHexCoord, BuildingCreateRequest>();
            sceneChunks = new HashSet<RectCoord>();
            readOnlySceneChunks = sceneChunks.AsReadOnlyCollection();
        }

        readonly IWorldData worldData;
        readonly Landform landform;
        readonly RequestDispatcher requestDispatcher;
        readonly Dictionary<CubicHexCoord, BuildingCreateRequest> sceneBuildings;
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
        /// 创建建筑物到该坐标,若不存在建筑物则返回 null;
        /// </summary>
        ILandformBuilding Create(CubicHexCoord position)
        {
            MapNode node;
            if (mapData.TryGetValue(position, out node))
            {
                if (node.Building.Exist())
                {
                    BuildingResource resource;
                    int buildingType = node.Building.Type;
                    if (resources.TryGetValue(buildingType, out resource))
                    {
                        ILandformBuilding building = resource.Building.BuildAt(position, node, null, worldData);
                        return building;
                    }
                    else
                    {
                        throw new KeyNotFoundException("未找到对应的建筑物预制资源;BuildingType:" + buildingType);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 异步创建,若已经存在创建,则返回之前的;
        /// </summary>
        BuildingCreateRequest CreateAsync(CubicHexCoord position)
        {
            BuildingCreateRequest request;
            if (!sceneBuildings.TryGetValue(position, out request))
            {
                request = new BuildingCreateRequest(this, position);
                AddQueue(request);
                sceneBuildings.Add(position, request);
            }
            return request;
        }

        /// <summary>
        /// 添加到处理队列中;
        /// </summary>
        void AddQueue(IRequest request)
        {
            requestDispatcher.AddQueue(request);
        }

        /// <summary>
        /// 创建对应块的建筑物;
        /// </summary>
        public void Create(RectCoord chunkCoord)
        {
            if (!sceneChunks.Contains(chunkCoord))
            {
                sceneChunks.Add(chunkCoord);
                var overlayPoints = GetOverlayPoints(chunkCoord);
                foreach (var point in overlayPoints)
                {
                    CreateAsync(point);
                }
            }
        }

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
        IEnumerable<CubicHexCoord> GetOverlayPoints(RectCoord chunkCoord)
        {
            CubicHexCoord chunkCenter = ChunkInfo.ChunkGrid.GetCenter(chunkCoord).GetTerrainCubic();
            foreach (var item in buildingOverlay)
            {
                yield return chunkCenter + item;
            }
        }

        /// <summary>
        /// 销毁这个地图块;
        /// </summary>
        public bool Destroy(RectCoord chunkCoord)
        {
            if (sceneChunks.Contains(chunkCoord))
            {
                sceneChunks.Remove(chunkCoord);
                var overlayPoints = GetOverlayPoints(chunkCoord);
                foreach (var point in overlayPoints)
                {
                    CreateAsync(point);
                }
                return true;
            }
            return false;
        }

        void Destroy(CubicHexCoord position)
        {
            BuildingCreateRequest request;
            if (sceneBuildings.TryGetValue(position, out request))
            {
                request.Destroy();
                sceneBuildings.Remove(position);
            }
        }

        /// <summary>
        /// 更新指定地点的建筑物,若不存在建筑物,则返回null;
        /// </summary>
        public IAsyncOperation<ILandformBuilding> Update(CubicHexCoord position)
        {
            BuildingCreateRequest request;
            if (sceneBuildings.TryGetValue(position, out request))
            {
                if (!request.IsInQueue)
                {
                    request.Restart();
                    AddQueue(request);
                }
                return request;
            }
            else
            {
                return null;
            }
        }

        class BuildingCreateRequest : AsyncOperation<ILandformBuilding>, IRequest
        {
            public BuildingCreateRequest(BuildingBuilder builder, CubicHexCoord position)
            {
                this.builder = builder;
                this.position = position;
            }

            readonly BuildingBuilder builder;
            readonly CubicHexCoord position;
            public bool IsInQueue { get; private set; }
            public bool IsCanceled { get; set; }

            public CubicHexCoord Position
            {
                get { return position; }
            }

            protected override void ResetState()
            {
                base.ResetState();
                IsInQueue = false;
                IsCanceled = false;
            }

            /// <summary>
            /// 销毁这个请求;
            /// </summary>
            public void Destroy()
            {
                IsCanceled = true;
                if (Result != null)
                {
                    Result.Destroy();
                    Result = null;
                }
            }

            /// <summary>
            /// 重置所有,为重新进入队列做准备;
            /// </summary>
            public void Restart()
            {
                ResetState();
                if (Result != null)
                {
                    Result.Destroy();
                    Result = null;
                }
            }

            void IRequest.Operate()
            {
                try
                {
                    var building = builder.Create(position);
                    OnCompleted(building);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    OnFaulted(ex);
                }
            }

            void IRequest.AddQueue()
            {
                if (IsInQueue)
                {
                    Debug.LogError("重复加入队列;");
                }

                IsInQueue = true;
            }

            void IRequest.OutQueue()
            {
                if (!IsInQueue)
                {
                    Debug.LogError("重复移除队列;");
                }

                IsInQueue = false;
            }
        }
    }
}
