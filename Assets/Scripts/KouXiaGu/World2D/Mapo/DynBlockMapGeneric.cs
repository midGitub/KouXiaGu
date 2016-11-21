//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using UnityEngine;

//namespace KouXiaGu.World2D
//{

//    public class DynBlockMapGeneric<T, TRead> : DynBlockMapEx<T, TRead, MapBlock<T, TRead>>,
//            IMapBlockInfo
//        where T : class, TRead
//        where TRead : class, IReadOnly<T>
//    {

//        protected DynBlockMapGeneric() { }

//        /// <summary>
//        /// 地图当前状态;
//        /// </summary>
//        [SerializeField]
//        private MapState mapState;
//        /// <summary>
//        /// 是否保也保存到预览地图;
//        /// </summary>
//        [SerializeField]
//        private bool alsoSavePrefabMap;
//        /// <summary>
//        /// 地图块前缀;
//        /// </summary>
//        [SerializeField]
//        private string addressPrefix;
//        /// <summary>
//        /// 地图缓存文件目录;
//        /// </summary>
//        [SerializeField]
//        private string archiveTempDirectoryName;

//        public string AddressPrefix
//        {
//            get { return addressPrefix; }
//        }
//        public string FullArchiveTempDirectoryPath { get; set; }
//        public string FullPrefabMapDirectoryPath { get; set; }

//        public MapState MapState
//        {
//            get { return mapState; }
//        }

//        public override void Awake()
//        {
//            mapState = MapState.None;
//            base.Awake();
//            FullArchiveTempDirectoryPath = Path.Combine(Application.dataPath, archiveTempDirectoryName);
//        }

//        private void SaveMapBlock(ShortVector2 blockAddress, MapBlock<T, TRead> block)
//        {
//            if (alsoSavePrefabMap)
//            {
//                this.SavePrefabMapBlockOrNot(blockAddress, block);
//            }
//            this.SaveArchiveMapBlockOrNot(blockAddress, block);
//        }

//        public override MapBlock<T, TRead> GetBlock(ShortVector2 blockAddress)
//        {
//            return this.LoadMapBlock<T, TRead>(blockAddress);
//        }

//        public override void ReleaseBlock(ShortVector2 blockAddress, MapBlock<T, TRead> block)
//        {
//            this.SaveMapBlock(blockAddress, block);
//        }

//        /// <summary>
//        /// 更新地图的数据;
//        /// </summary>
//        public void OnMapUpdate(IntVector2 mapPoint)
//        {
//            ShortVector2 address;
//            if (NeedToUpdate(mapPoint, out address) || mapState == MapState.None)
//            {
//                lock (thisLock)
//                {
//                    mapState = MapState.Loading;
//                    WaitCallback waitCallback = delegate
//                    {
//                        UpdateBlocks(mapPoint);
//                        mapState = MapState.None;
//                    };
//                    ThreadPool.QueueUserWorkItem(waitCallback);
//                }
//            }
//        }


//        /// <summary>
//        /// 开始游戏初始化内容;
//        /// </summary>
//        public void OnGameStart(string fullArchivedDirectoryPath, string fullPrefabMapDirectory)
//        {
//            RecoveryTempData(fullArchivedDirectoryPath);
//            RecoveryLoadArchived(fullPrefabMapDirectory);
//        }

//        /// <summary>
//        /// 将存档的归档地图拷贝到缓存地图文件夹下;
//        /// </summary>
//        private void RecoveryTempData(string fullArchivedDirectoryPath)
//        {
//            this.DeleteMapFile(this.FullArchiveTempDirectoryPath);
//            if (Directory.Exists(fullArchivedDirectoryPath))
//            {
//                this.MapFileCopyTo(fullArchivedDirectoryPath, this.FullArchiveTempDirectoryPath, true);
//            }
//        }

//        /// <summary>
//        /// 从存档读取信息;
//        /// </summary>
//        private void RecoveryLoadArchived(string fullpathPrefabMapDirectory)
//        {
//            if (!Directory.Exists(fullpathPrefabMapDirectory))
//                throw new FileNotFoundException("地图丢失!" + fullpathPrefabMapDirectory);

//            this.FullPrefabMapDirectoryPath = fullpathPrefabMapDirectory;
//        }


//        /// <summary>
//        /// 当游戏归档时进行的操作;
//        /// </summary>
//        public string OnGameArchive(string fullArchivedDirectoryPath)
//        {
//            this.OnSave();
//            ArchiveCopyArchived(fullArchivedDirectoryPath);
//            return FullPrefabMapDirectoryPath;
//        }

//        /// <summary>
//        /// 将地图保存到缓存地图文件夹内;
//        /// </summary>
//        public void OnSave()
//        {
//            lock (thisLock)
//            {
//                mapState = MapState.Saving;
//                KeyValuePair<ShortVector2, MapBlock<T, TRead>>[] pairs = MapCollection.ToArray();
//                foreach (var pair in pairs)
//                {
//                    this.SaveMapBlock(pair.Key, pair.Value);
//                }
//                mapState = MapState.None;
//            }
//        }

//        /// <summary>
//        /// 复制缓存存档地图到存档路径下;
//        /// </summary>
//        private void ArchiveCopyArchived(string fullArchivedDirectoryPath)
//        {
//            this.ArchiveMapCopyTo(fullArchivedDirectoryPath, true);
//        }


//        /// <summary>
//        /// 当退出时清除地图信息;
//        /// </summary>
//        public void OnGameQuit()
//        {
//            base.Clear();
//        }


//    }

//}
