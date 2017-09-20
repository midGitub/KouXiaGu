﻿using System;
using System.Threading.Tasks;
using UnityEngine;
using KouXiaGu.Resources;
using System.Threading;
using KouXiaGu.Resources.Archive;
using KouXiaGu.RectTerrain.Resources;

namespace KouXiaGu.World.RectMap
{

    /// <summary>
    /// 地图数据读取;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RectMapDataInitializer : SceneSington<RectMapDataInitializer>, IDataInitializer
    {
        RectMapDataInitializer()
        {
        }

        [SerializeField]
        bool isUseRandomMap;
        [SerializeField]
        int randomMapRadius;

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

        MapDataSerializer mapDataSerializer
        {
            get { return new MapDataSerializer(ProtoFileSerializer<MapData>.Default, new MultipleResourceSearcher("World/Data")); }
        }

        WorldMapSerializer worldMapSerializer
        {
            get { return new WorldMapSerializer(mapDataSerializer, ProtoFileSerializer<MapData>.Default, "World/Data"); }
        }

        /// <summary>
        /// 游戏地图;
        /// </summary>
        public WorldMap WorldMap { get; private set; }

        void Awake()
        {
            SetInstance(this);
        }

        Task IDataInitializer.StartInitialize(ArchiveInfo archive, CancellationToken token)
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

        WorldMap GetMap(ArchiveInfo archive)
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
    }
}
