using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 从存储文件读取可归档地图块的方法;
    /// </summary>
    [Serializable]
    public class ArchiveBlockIO<T> : IMapBlockIO<ArchiveBlock<T>>, IMapBlockInfo
        where T : struct
    {

        /// <summary>
        /// 保存地图文件的前缀;
        /// </summary>
        [SerializeField]
        string addressFilePrefix = "MapData";
        /// <summary>
        /// 地图缓存文件目录;
        /// </summary>
        [SerializeField]
        string archiveTempDirectoryName = "Temp/Maps";

        /// <summary>
        /// 做出的改变保存在预制地图上;
        /// </summary>
        [SerializeField]
        bool editPrefab = false;


        public string FullPrefabMapDirectoryPath { get; private set; }

        public string AddressPrefix
        {
            get { return addressFilePrefix; }
        }
        public string FullArchiveTempDirectoryPath
        {
            get { return Path.Combine(Application.dataPath, archiveTempDirectoryName); }
        }


        public ArchiveBlock<T> Load(RectCoord address)
        {
            return this.LoadMapBlock<T>(address);
        }

        public void Unload(RectCoord address, ArchiveBlock<T> block)
        {
            SaveMapBlock(address, block);
        }

        void SaveMapBlock(RectCoord address, ArchiveBlock<T> block)
        {
            this.SaveArchiveMapBlockOrNot(address, block);
            if (editPrefab)
            {
                this.SavePrefabMapBlockOrNot(address, block);
            }
        }

        /// <summary>
        /// 当开始构建游戏时调用;
        /// </summary>
        public void OnBulidGame(string fullArchivedDirectoryPath, string fullPrefabMapDirectoryPath)
        {
            RecoveryTempData(fullArchivedDirectoryPath);
            RecoveryLoadArchived(fullPrefabMapDirectoryPath);
        }

        /// <summary>
        /// 将存档的归档地图拷贝到缓存地图文件夹下;
        /// </summary>
        void RecoveryTempData(string fullArchivedDirectoryPath)
        {
            this.DeleteMapFile(this.FullArchiveTempDirectoryPath);
            if (Directory.Exists(fullArchivedDirectoryPath))
            {
                this.MapFileCopyTo(fullArchivedDirectoryPath, this.FullArchiveTempDirectoryPath, true);
            }
        }

        /// <summary>
        /// 从存档读取信息;
        /// </summary>
        void RecoveryLoadArchived(string fullpathPrefabMapDirectoryPath)
        {
            if (!Directory.Exists(fullpathPrefabMapDirectoryPath))
                throw new FileNotFoundException("地图丢失!" + fullpathPrefabMapDirectoryPath);

            this.FullPrefabMapDirectoryPath = fullpathPrefabMapDirectoryPath;
        }

        /// <summary>
        /// 当游戏归档时进行的操作;
        /// </summary>
        public void OnGameArchive(string fullArchivedDirectoryPath, BlockMap<ArchiveBlock<T>> blockMap)
        {
            this.OnSave(blockMap);
            ArchiveCopyArchived(fullArchivedDirectoryPath);
        }

        /// <summary>
        /// 将地图保存到缓存地图文件夹内;
        /// </summary>
        void OnSave(BlockMap<ArchiveBlock<T>> blockMap)
        {
            KeyValuePair<RectCoord, ArchiveBlock<T>>[] pairs = blockMap.BlocksPair.ToArray();
            foreach (var pair in pairs)
            {
                SaveMapBlock(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// 复制缓存存档地图到存档路径下;
        /// </summary>
        void ArchiveCopyArchived(string fullArchivedDirectoryPath)
        {
            this.ArchiveMapCopyTo(fullArchivedDirectoryPath, true);
        }

    }

}
