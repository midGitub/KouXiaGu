using KouXiaGu.Concurrent;
using KouXiaGu.Grids;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 场景建筑物合集;
    /// </summary>
    public class SceneBuildingCollection
    {
        public SceneBuildingCollection()
        {
            SceneChunks = new HashSet<RectCoord>();
            SceneBuildings = new Dictionary<CubicHexCoord, BuildingBuilder.CreateRequest>();
            ReadOnlySceneChunks = SceneChunks.AsReadOnlyCollection();
            ReadOnlySceneBuildings = SceneBuildings.AsReadOnlyDictionary(item => item as IAsyncOperation<IBuilding>);
        }

        internal HashSet<RectCoord> SceneChunks { get; private set; }
        internal Dictionary<CubicHexCoord, BuildingBuilder.CreateRequest> SceneBuildings { get; private set; }
        public IReadOnlyCollection<RectCoord> ReadOnlySceneChunks { get; private set; }
        public IReadOnlyDictionary<CubicHexCoord, IAsyncOperation<IBuilding>> ReadOnlySceneBuildings { get; private set; }

        public RectGrid chunkGrid
        {
            get { return ChunkInfo.ChunkGrid; }
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
        public IEnumerable<CubicHexCoord> GetOverlayPoints(RectCoord chunkCoord)
        {
            CubicHexCoord chunkCenter = ChunkInfo.ChunkGrid.GetCenter(chunkCoord).GetTerrainCubic();
            foreach (var item in buildingOverlay)
            {
                yield return chunkCenter + item;
            }
        }
    }

    /// <summary>
    /// 对场景建筑物进行构建,仅由地形更新器调用;
    /// </summary>
    class BuildingBuilder 
    {
        public BuildingBuilder(IWorld world, LandformBuilder landformBuilder, IRequestDispatcher requestDispatcher)
        {
            World = world;
            BuildingCollection = world.Components.Landform.Buildings;
            RequestDispatcher = requestDispatcher;
            landformObserver = new LandformObserver(this, landformBuilder.CompletedChunkSender);
        }

        public IWorld World { get; private set; }
        public SceneBuildingCollection BuildingCollection { get; private set; }
        public IRequestDispatcher RequestDispatcher { get; private set; }
        readonly LandformObserver landformObserver;
        readonly object unityThreadLock = new object();

        HashSet<RectCoord> sceneChunks
        {
            get { return BuildingCollection.SceneChunks; }
        }

        Dictionary<CubicHexCoord, CreateRequest> sceneBuildings
        {
            get { return BuildingCollection.SceneBuildings; }
        }

        IReadOnlyDictionary<CubicHexCoord, MapNode> map
        {
            get { return World.WorldData.MapData.ReadOnlyMap; }
        }

        IDictionary<int, BuildingInfo> infos
        {
            get { return World.BasicData.BasicResource.Terrain.Building; }
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
                    CreateAt(point);
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
        public IEnumerable<CubicHexCoord> GetOverlayPoints(RectCoord chunkCoord)
        {
            CubicHexCoord chunkCenter = ChunkInfo.ChunkGrid.GetCenter(chunkCoord).GetTerrainCubic();
            foreach (var item in buildingOverlay)
            {
                yield return chunkCenter + item;
            }
        }

        /// <summary>
        /// 创建到建筑,若已经存在建筑了,则返回Null;
        /// </summary>
        CreateRequest CreateAt(CubicHexCoord position)
        {
            if (!sceneBuildings.ContainsKey(position))
            {
                IBuildingPrefab prefab;
                float angle;
                if (TryGetBuildingInfo(position, out prefab, out angle))
                {
                    CreateRequest request = new CreateRequest(this, position, prefab, angle);
                    sceneBuildings.Add(position, request);
                    RequestDispatcher.Add(request);
                    return request;
                }
            }
            return null;
        }

        bool TryGetBuildingInfo(CubicHexCoord position, out IBuildingPrefab prefab, out float angle)
        {
            MapNode node;
            if (map.TryGetValue(position, out node))
            {
                if (node.Building.Exist())
                {
                    BuildingInfo info;
                    int buildingType = node.Building.BuildingType;
                    if (infos.TryGetValue(buildingType, out info))
                    {
                        prefab = info.Terrain.BuildingPrefab;
                        angle = node.Building.Angle;
                        return true;
                    }
                    else
                    {
                        Debug.LogWarning("未找到对应的建筑物资源;BuildingType:" + buildingType);
                    }
                }
            }
            prefab = default(IBuildingPrefab);
            angle = default(float);
            return false;
        }

        /// <summary>
        /// 更新指定地点的建筑物,若不存在建筑物,则返回null;
        /// </summary>
        public IAsyncOperation<IBuilding> UpdateAt(CubicHexCoord position)
        {
            CreateRequest request;
            if (sceneBuildings.TryGetValue(position, out request))
            {
                IBuildingPrefab prefab;
                float angle;
                if (TryGetBuildingInfo(position, out prefab, out angle))
                {
                    request.Prefab = prefab;
                    request.Angle = angle;

                    if (request.IsCompleted)
                    {
                        RequestDispatcher.Add(request);
                    }
                    return request;
                }
            }
            return null;
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
                    DestroyAt(point);
                }
                return true;
            }
            return false;
        }

        bool DestroyAt(CubicHexCoord position)
        {
            CreateRequest request;
            if (sceneBuildings.TryGetValue(position, out request))
            {
                if (request.IsCompleted && !request.IsFaulted)
                {
                    DestroyRequest destroyRequest = new DestroyRequest(request.Result);
                    RequestDispatcher.Add(destroyRequest);
                }
                sceneBuildings.Remove(position);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 在Unity线程的,销毁所有建筑实例;
        /// </summary>
        public void DestroyAll()
        {
            foreach (var building in sceneBuildings.Values)
            {
                building.Result.Destroy();
            }
            sceneBuildings.Clear();
            sceneChunks.Clear();
        }

        /// <summary>
        /// 创建建筑;
        /// </summary>
        internal class CreateRequest : AsyncOperation<IBuilding>, IAsyncRequest
        {
            public CreateRequest(BuildingBuilder parent, CubicHexCoord position, IBuildingPrefab prefab, float angle)
            {
                Parent = parent;
                Position = position;
                Prefab = prefab;
                Angle = angle;
            }

            public BuildingBuilder Parent { get; private set; }
            public CubicHexCoord Position { get; private set; }
            public IBuildingPrefab Prefab { get; internal set; }
            public float Angle { get; internal set; }
            public bool IsCanceled { get; private set; }

            public IWorld World
            {
                get { return Parent.World; }
            }

            internal void Cancele()
            {
                IsCanceled = true;
            }

            void IAsyncRequest.Operate()
            {
                if (!IsCanceled)
                {
                    try
                    {
                        if (Result != null)
                        {
                            Result.Destroy();
                            Result = null;
                        }

                        IBuilding building = Prefab.BuildAt(World, Position, Angle);
                        OnCompleted(building);
                    }
                    catch (Exception ex)
                    {
                        OnFaulted(ex);
                    }
                }
            }

            void IAsyncRequest.AddQueue() { }
        }

        /// <summary>
        /// 销毁建筑;
        /// </summary>
        class DestroyRequest : IAsyncRequest
        {
            public DestroyRequest(IBuilding building)
            {
                Building = building;
            }

            public IBuilding Building { get; private set; }

            void IAsyncRequest.Operate()
            {
                Building.Destroy();
            }

            void IAsyncRequest.AddQueue() { }
        }

        /// <summary>
        /// 观察地形发生变化;
        /// </summary>
        class LandformObserver : IObserver<RectCoord>
        {
            public LandformObserver(BuildingBuilder parent, IObservable<RectCoord> landformChanged)
            {
                this.parent = parent;
                landformChangedDisposer = landformChanged.Subscribe(this);
            }

            readonly BuildingBuilder parent;
            IDisposable landformChangedDisposer;

            public bool IsSubscribed
            {
                get { return landformChangedDisposer != null; }
            }

            void IObserver<RectCoord>.OnCompleted()
            {
                Unsubscribe();
            }

            void IObserver<RectCoord>.OnError(Exception error)
            {
                return;
            }

            void IObserver<RectCoord>.OnNext(RectCoord chunkCoord)
            {
                var overlayPoints = GetOverlayPoints(chunkCoord);
                foreach (var point in overlayPoints)
                {
                    CreateRequest building;
                    if (parent.sceneBuildings.TryGetValue(point, out building))
                    {
                        if (building.IsCompleted)
                        {
                            building.Result.Rebuild();
                        }
                    }
                }
            }

            /// <summary>
            /// (0, 0)对应覆盖的节点(依赖地图块大小);
            /// </summary>
            static readonly CubicHexCoord[] buildingOverlay = new CubicHexCoord[]
                {
                    new CubicHexCoord(-2, 3),
                    new CubicHexCoord(-2, 2),
                    new CubicHexCoord(-2, 1),
                    new CubicHexCoord(-2, 0),
                    new CubicHexCoord(-2, -1),

                    new CubicHexCoord(-1, 2),
                    new CubicHexCoord(-1, 1),
                    new CubicHexCoord(-1, 0),
                    new CubicHexCoord(-1, -1),

                    new CubicHexCoord(0, 2),
                    new CubicHexCoord(0, 1),
                    new CubicHexCoord(0, 0),
                    new CubicHexCoord(0, -1),
                    new CubicHexCoord(0, -2),

                    new CubicHexCoord(1, 1),
                    new CubicHexCoord(1, 0),
                    new CubicHexCoord(1, -1),
                    new CubicHexCoord(1, -2),

                    new CubicHexCoord(2, 1),
                    new CubicHexCoord(2, 0),
                    new CubicHexCoord(2, -1),
                    new CubicHexCoord(2, -2),
                    new CubicHexCoord(2, -3),
                };

            /// <summary>
            /// 获取到地形块对应需要更新的建筑物坐标,比 BuildingBuilder.GetOverlayPoints() 范围要大;
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
            /// 取消订阅;
            /// </summary>
            public void Unsubscribe()
            {
                if (landformChangedDisposer != null)
                {
                    landformChangedDisposer.Dispose();
                    landformChangedDisposer = null;
                }
            }
        }
    }
}
