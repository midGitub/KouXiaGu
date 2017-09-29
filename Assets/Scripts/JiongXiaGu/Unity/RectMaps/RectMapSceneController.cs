using System;
using System.Threading.Tasks;
using UnityEngine;
using JiongXiaGu.Unity.Resources;
using System.Threading;
using JiongXiaGu.Unity.Resources.Archives;
using JiongXiaGu.Unity.RectTerrain.Resources;
using JiongXiaGu.Unity.Archives;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图数据读取;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RectMapSceneController : SceneSington<RectMapSceneController>, IDataInitializeHandle, ISceneArchiveHandle
    {
        RectMapSceneController()
        {
        }

        [SerializeField]
        bool isUseRandomMap;
        [SerializeField]
        int randomMapRadius;

        MapDataSerializer mapDataSerializer;
        WorldMapSerializer worldMapSerializer;

        /// <summary>
        /// 是否使用随机地图?
        /// </summary>
        public bool IsUseRandomMap
        {
            get { return isUseRandomMap; }
            set { isUseRandomMap = value; }
        }

        /// <summary>
        /// 生成的随机地图大小;
        /// </summary>
        public int RandomMapRadius
        {
            get { return randomMapRadius; }
            set { randomMapRadius = value; }
        }

        /// <summary>
        /// 游戏地图;
        /// </summary>
        public WorldMap WorldMap { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            mapDataSerializer = new MapDataSerializer(ProtoFileSerializer<MapData>.Default, new ResourcesMultipleSearcher("World/Data"));
            worldMapSerializer = new WorldMapSerializer(mapDataSerializer, ProtoFileSerializer<MapData>.Default, "World/Data");
            SetInstance(this);
        }

        Task IDataInitializeHandle.StartInitialize(Archive archive, CancellationToken token)
        {
            return Task.Run(delegate ()
            {
                if (isUseRandomMap)
                {
                    WorldMap = GetRandomMap();
                }
                else
                {
                    WorldMap = GetMap(archive);
                }
                OnCompleted();
            }, token);
        }

        WorldMap GetMap(Archive archive)
        {
            WorldMap map = worldMapSerializer.Deserialize(archive);
            return map;
        }

        WorldMap GetRandomMap()
        {
            RectTerrainResources rectTerrainResources = RectTerrainResourcesInitializer.RectTerrainResources;

            if (rectTerrainResources == null)
                throw new ArgumentException("RectTerrainResources 未初始化完成!");

            var mapGenerator = new SimpleMapGenerator(rectTerrainResources);
            MapData map = mapGenerator.Create(randomMapRadius);
            return new WorldMap(map);
        }

        [System.Diagnostics.Conditional("EDITOR_LOG")]
        void OnCompleted()
        {
            const string prefix = "[地图资源]";
            string info = "[地图:Size:" + WorldMap.Map.Count + "]";
            Debug.Log(prefix + "初始化完成;" + info);
        }

        Task ISceneArchiveHandle.WriteArchive(Archive archive, CancellationToken token)
        {
            return Task.Run(delegate ()
            {
                worldMapSerializer.Serialize(archive, WorldMap);
            }, token);
        }
    }
}
