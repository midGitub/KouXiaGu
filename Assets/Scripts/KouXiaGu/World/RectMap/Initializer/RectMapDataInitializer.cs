using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using KouXiaGu.Concurrent;
using UnityEngine;
using KouXiaGu.Resources;

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


        [SerializeField]
        WorldDataFiler mapDataFile;

        public WorldDataFiler MapDataFile
        {
            get { return mapDataFile; }
        }


        /// <summary>
        /// 游戏地图;
        /// </summary>
        public WorldMap WorldMap { get; private set; }

        public IFileSerializer<MapData> Serializer
        {
            get { return ProtoFileSerializer<MapData>.Default; }
        }

        Task IDataInitializer.StartInitialize(ArchiveFile archive, IOperationState state)
        {
            return Task.Run(delegate ()
            {
                MapData data = Serializer.Read(mapDataFile.GetFileFullPath());
                if (archive == null)
                {
                    WorldMap = new WorldMap(data);
                }
                else
                {
                    MapData archiveMap = Serializer.Read(mapDataFile.GetArchiveFileFullPath(archive));
                    WorldMap = new WorldMap(data, archiveMap);
                }
            });
        }
    }
}
