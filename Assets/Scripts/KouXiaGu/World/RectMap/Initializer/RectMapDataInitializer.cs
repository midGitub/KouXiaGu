using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using KouXiaGu.Concurrent;
using UnityEngine;
using KouXiaGu.Resources;
using System.Threading;
using KouXiaGu.Resources.Archive;

namespace KouXiaGu.World.RectMap
{

    /// <summary>
    /// 地图数据读取;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RectMapDataInitializer : MonoBehaviour, IDataInitializer
    {
        RectMapDataInitializer()
        {
        }

        [SerializeField]
        bool isUseRandomMap;

        /// <summary>
        /// 是否使用随机地图?
        /// </summary>
        public bool IsUseRandomMap
        {
            get { return isUseRandomMap; }
        }

        static MapDataSerializer mapDataSerializer = new MapDataSerializer(ProtoFileSerializer<MapData>.Default, new MultipleResourceSearcher("World/Data"));

        static WorldMapSerializer worldMapSerializer = new WorldMapSerializer(mapDataSerializer, ProtoFileSerializer<MapData>.Default, "World/Data");

        /// <summary>
        /// 游戏地图;
        /// </summary>
        public WorldMap WorldMap { get; private set; }

        Task IDataInitializer.StartInitialize(ArchiveInfo archive, CancellationToken token)
        {
            return Task.Run(delegate ()
            {
                WorldMap = ReadMap(archive);
            }, token);
        }

        WorldMap ReadMap(ArchiveInfo archive)
        {
            WorldMap map = worldMapSerializer.Deserialize(archive);
            return map;
        }

        WorldMap ReadRandomMap()
        {
            throw new NotImplementedException();
        }
    }
}
