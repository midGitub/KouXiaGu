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

        Task IDataInitializer.StartInitialize(Archive archive, IOperationState state)
        {
            return Task.Run(delegate ()
            {
                WorldMap = ReadMap(archive, state);
            });
        }

        WorldMap ReadMap(Archive archive, IOperationState state)
        {
            string filePath = mapDataFile.GetFileFullPath();
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("未找到地图文件:" + filePath);
            }
            MapData data = Serializer.Read(filePath);

            string archivePath = mapDataFile.GetArchiveFileFullPath(archive);
            if (File.Exists(archivePath))
            {
                MapData archiveMap = Serializer.Read(archivePath);
                return new WorldMap(data, archiveMap);
            }
            else
            {
                return new WorldMap(data);
            }
        }

        WorldMap ReadRandomMap()
        {
            throw new NotImplementedException();
        }
    }
}
