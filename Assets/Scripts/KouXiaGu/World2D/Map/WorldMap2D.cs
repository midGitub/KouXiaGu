using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 游戏地图;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldMap2D : MonoBehaviour, IMapBlockInfo, IMapBlockIO<MapBlock<WorldNode>>, IStartGameEvent
    {

        /// <summary>
        /// 保存到存档的位置;
        /// </summary>
        [SerializeField]
        private string archivedDirectoryName;
        /// <summary>
        /// 每个地图块的大小;
        /// </summary>
        [SerializeField]
        ShortVector2 partitionSizes;
        /// <summary>
        /// 地图读取的范围;
        /// </summary>
        [SerializeField]
        IntVector2 loadRang;
        /// <summary>
        /// 保存地图文件的前缀;
        /// </summary>
        [SerializeField]
        string addressFilePrefix;
        /// <summary>
        /// 地图缓存文件目录;
        /// </summary>
        [SerializeField]
        private string archiveTempDirectoryName;

        BlockLoader<WorldNode, MapBlock<WorldNode>> mapCollection;
        public IMap<IntVector2, WorldNode> Map { get { return mapCollection; } }

        public string AddressPrefix { get { return addressFilePrefix; } }
        public string FullArchiveTempDirectoryPath { get; private set; }
        public string FullPrefabMapDirectoryPath { get; private set; }

        void Awake()
        {
            FullArchiveTempDirectoryPath = Path.Combine(Application.dataPath, archiveTempDirectoryName);
        }

        MapBlock<WorldNode> IMapBlockIO<MapBlock<WorldNode>>.Load(ShortVector2 address)
        {
            return this.LoadMapBlock<WorldNode>(address);
        }

        void IMapBlockIO<MapBlock<WorldNode>>.Save(ShortVector2 address, MapBlock<WorldNode> block)
        {
            this.SaveArchiveMapBlockOrNot(address, block);
        }

        /// <summary>
        /// 获取到完整的归档地图文件夹路径;
        /// </summary>
        private string GetFullArchivedDirectoryPath(ArchivedGroup item)
        {
            string fullArchivedDirectoryPath = Path.Combine(item.ArchivedPath, archivedDirectoryName);
            return fullArchivedDirectoryPath;
        }

        /// <summary>
        /// 开始游戏时调用;
        /// </summary>
        IEnumerator IConstruct<BuildGameData>.Construction(BuildGameData item)
        {
            string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
            string fullPrefabMapDirectory = item.ArchivedData.Archived.World2D.PathPrefabMapDirectory;

            RecoveryTempData(fullArchivedDirectoryPath);
            RecoveryLoadArchived(fullPrefabMapDirectory);

            mapCollection = new BlockLoader<WorldNode, MapBlock<WorldNode>>(partitionSizes, loadRang, this);
            yield break;
        }

        /// <summary>
        /// 将存档的归档地图拷贝到缓存地图文件夹下;
        /// </summary>
        private void RecoveryTempData(string fullArchivedDirectoryPath)
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
        private void RecoveryLoadArchived(string fullpathPrefabMapDirectory)
        {
            if (!Directory.Exists(fullpathPrefabMapDirectory))
                throw new FileNotFoundException("地图丢失!" + fullpathPrefabMapDirectory);

            this.FullPrefabMapDirectoryPath = fullpathPrefabMapDirectory;
        }

    }

}
