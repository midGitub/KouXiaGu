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
        /// 创建到建筑,若已经存在建筑了,则返回其,若不存在建筑则创建一个空的;
        /// </summary>
        CreateRequest CreateAt(CubicHexCoord position)
        {
            lock (unityThreadLock)
            {
                CreateRequest request;
                if (!sceneBuildings.TryGetValue(position, out request))
                {
                    MapNode node;
                    if (map.TryGetValue(position, out node))
                    {
                        if (node.Building.Exist())
                        {
                            request = new CreateRequest(this, position, node.Building);
                            sceneBuildings.Add(position, request);
                            RequestDispatcher.Add(request);
                            return request;
                        }
                    }

                    request = new CreateRequest(this, position);
                    sceneBuildings.Add(position, request);
                }
                return request;
            }
        }

        /// <summary>
        /// 更新指定地点的建筑物,若不存在建筑物,则返回null;
        /// </summary>
        public CreateRequest UpdateAt(CubicHexCoord position, BuildingNode node)
        {
            CreateRequest request = null;
            if (sceneBuildings.TryGetValue(position, out request))
            {
                if (request.IsCompleted)
                {
                    request.ResetNode(node);
                    RequestDispatcher.Add(request);
                }
                else
                {
                    request.ResetNode(node);
                }
            }
            return request;
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
            lock (unityThreadLock)
            {
                CreateRequest request;
                if (sceneBuildings.TryGetValue(position, out request))
                {
                    if (request.IsCompleted && request.Result.Count != 0)
                    {
                        request.Cancele();
                        DestroyRequest destroyRequest = new DestroyRequest(this, request.Result);
                        RequestDispatcher.Add(destroyRequest);
                    }
                    else
                    {
                        request.Cancele();
                    }
                    sceneBuildings.Remove(position);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 在Unity线程的,销毁所有建筑实例;
        /// </summary>
        public void DestroyAll()
        {
            foreach (var buildings in sceneBuildings.Values)
            {
                foreach (var building in buildings.Result)
                {
                    building.Destroy();
                }
            }
            sceneBuildings.Clear();
            sceneChunks.Clear();
        }



        List<IBuilding> CreateBuilding_internal(CubicHexCoord position, IList<BuildingItem> buildingItems)
        {
            List<IBuilding> buildings = new List<IBuilding>();
            foreach (var buildingItem in buildingItems)
            {
                IBuilding building = CreateBuilding_internal(position, buildingItem);
                buildings.Add(building);
            }
            return buildings;
        }

        IBuilding CreateBuilding_internal(CubicHexCoord position, BuildingItem buildingItem)
        {
            BuildingInfo info;
            if (infos.TryGetValue(buildingItem.BuildingType, out info))
            {
                IBuildingPrefab prefab = info.Terrain.BuildingPrefab;
                IBuilding building = prefab.BuildAt(World, position, buildingItem.Angle, info);
                return building;
            }
            else
            {
                Debug.LogWarning("未找到对应的建筑物资源;BuildingType:" + buildingItem.BuildingType);
                return null;
            }
        }

        /// <summary>
        /// 销毁这些建筑物,并且清空这个合集;
        /// </summary>
        void DestroyBuilding_internal(ICollection<IBuilding> buildings)
        {
            foreach (var building in buildings)
            {
                DestroyBuilding_internal(building);
            }
            buildings.Clear();
        }

        /// <summary>
        /// 销毁这个建筑物实例;
        /// </summary>
        void DestroyBuilding_internal(IBuilding building)
        {
            building.Destroy();
        }


        /// <summary>
        /// 更新已经创建的建筑物;
        /// </summary>
        List<IBuilding> UpdateBuilding_internal(CubicHexCoord position, IList<BuildingItem> buildingItems, IList<IBuilding> buildings)
        {
            List<IBuilding> newBuildings = new List<IBuilding>();
            if (buildingItems != null)
            {
                for (int i = 0; i < buildingItems.Count; i++)
                {
                    BuildingItem buildingItem = buildingItems[i];
                    int index = buildings.FindIndex(item => item.Info.ID == buildingItem.BuildingType);
                    if (index >= 0)
                    {
                        IBuilding building = buildings[index];
                        building.Angle = buildingItem.Angle;
                        buildings.RemoveAt(index);
                        newBuildings.Add(building);
                    }
                    else
                    {
                        IBuilding building = CreateBuilding_changed(position, buildingItem);
                        newBuildings.Add(building);
                    }
                }
            }
            DestroyBuilding_changed(buildings);
            return newBuildings;
        }

        IBuilding CreateBuilding_changed(CubicHexCoord position, BuildingItem buildingItem)
        {
            IBuilding building = CreateBuilding_internal(position, buildingItem);
            if (building.Info.TerrainInfo.AssociativeNeighbor)
            {
                NotifyNeighbors(building.Position, building.Info.ID);
            }
            return building;
        }

        void DestroyBuilding_changed(ICollection<IBuilding> buildings)
        {
            foreach (var building in buildings)
            {
                DestroyBuilding_changed(building);
            }
            buildings.Clear();
        }

        void DestroyBuilding_changed(IBuilding building)
        {
            if (building.Info.TerrainInfo.AssociativeNeighbor)
            {
                NotifyNeighbors(building.Position, building.Info.ID);
            }
            DestroyBuilding_internal(building);
        }

        /// <summary>
        /// 通知到邻居节点;
        /// </summary>
        void NotifyNeighbors(CubicHexCoord position, int buildingType)
        {
            foreach (var neighbor in position.GetNeighbours())
            {
                CreateRequest request;
                if (sceneBuildings.TryGetValue(neighbor, out request))
                {
                    if (request.IsCompleted)
                    {
                        foreach (var building in request.Result)
                        {
                            if (building.Info.ID == buildingType)
                            {
                                building.NeighborChanged(position);
                                break;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 创建建筑;
        /// </summary>
        internal class CreateRequest : AsyncOperation<List<IBuilding>>, IAsyncRequest
        {
            /// <summary>
            /// 创建一个空的建筑;
            /// </summary>
            public CreateRequest(BuildingBuilder parent, CubicHexCoord position)
            {
                Parent = parent;
                Position = position;
                OnCompleted(new List<IBuilding>());
            }

            public CreateRequest(BuildingBuilder parent, CubicHexCoord position, BuildingNode node)
            {
                Parent = parent;
                Position = position;
                Node = node;
            }

            public BuildingBuilder Parent { get; private set; }
            public CubicHexCoord Position { get; private set; }
            public BuildingNode Node { get; private set; }
            public bool IsInQueue { get; private set; }
            public bool IsCanceled { get; private set; }

            public List<BuildingItem> buildingItems
            {
                get { return Node.Items; }
            }

            IWorld world
            {
                get { return Parent.World; }
            }

            IDictionary<int, BuildingInfo> infos
            {
                get { return Parent.World.BasicData.BasicResource.Terrain.Building; }
            }

            /// <summary>
            /// 设置新的建筑信息,并且重置状态;
            /// </summary>
            public void ResetNode(BuildingNode node)
            {
                Node = node;
                base.ResetState();
                IsCanceled = false;
            }

            internal void Cancele()
            {
                IsCanceled = true;
            }

            void IAsyncRequest.Operate()
            {
                lock (Parent.unityThreadLock)
                {
                    try
                    {
                        if (IsCanceled)
                        {
                            if (Result != null)
                            {
                                Parent.DestroyBuilding_internal(Result);
                                Result = null;
                            }
                            return;
                        }

                        if (Result == null)
                        {
                            List<IBuilding> buildings = new List<IBuilding>();
                            buildings = Parent.CreateBuilding_internal(Position, buildingItems);
                            OnCompleted(buildings);
                        }
                        else
                        {
                            List<IBuilding> buildings = Result;
                            buildings = Parent.UpdateBuilding_internal(Position, buildingItems, buildings);
                            OnCompleted(buildings);
                        }
                    }
                    catch (Exception ex)
                    {
                        OnFaulted(ex);
                        Debug.LogError(ex);
                    }
                    finally
                    {
                        IsInQueue = false;
                    }
                }
            }

            void IAsyncRequest.AddQueue()
            {
                IsInQueue = true;
            }
        }

        /// <summary>
        /// 销毁建筑;
        /// </summary>
        class DestroyRequest : IAsyncRequest
        {
            public DestroyRequest(BuildingBuilder parent, IEnumerable<IBuilding> buildings)
            {
                Parent = parent;
                Buildings = buildings;
            }

            public BuildingBuilder Parent { get; private set; }
            public IEnumerable<IBuilding> Buildings { get; private set; }

            void IAsyncRequest.Operate()
            {
                foreach (var building in Buildings)
                {
                    Parent.DestroyBuilding_internal(building);
                }
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
                            foreach (var item in building.Result)
                            {
                                item.UpdateHeight();
                            }
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
