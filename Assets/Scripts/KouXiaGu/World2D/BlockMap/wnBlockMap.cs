
#define DETAILED_DEBUG

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 可以对地图进行分块存档的地图;
    /// </summary>
    [Serializable]
    public class wnBlockMap : BlockMap<WorldNode, wnBlockMap.MapBlock>, IReadOnlyMap<IntVector2, IReadOnlyWorldNode>
    {
        public wnBlockMap(
            string fullArchiveTempDirectoryPath,
            string fullPrefabMapDirectoryPath,
            ShortVector2 partitionSizes,
            ShortVector2 minRadiationRange,
            ShortVector2 maxRadiationRange) 
            : base(partitionSizes, minRadiationRange, maxRadiationRange)
        {
            this.fullArchiveTempDirectoryPath = fullArchiveTempDirectoryPath;
            this.fullPrefabMapDirectoryPath = fullPrefabMapDirectoryPath;
        }

        private string addressPrefix;
        private string fullArchiveTempDirectoryPath;
        private string fullPrefabMapDirectoryPath;

        public string AddressPrefix
        {
            get { return addressPrefix; }
            set { addressPrefix = value; }
        }
        protected string ArchivedSearchPattern
        {
            get { return addressPrefix + "*"; }
        }

        /// <summary>
        /// 临时存放归档地图路径(后期提供);
        /// </summary>
        public string FullArchiveTempDirectoryPath
        {
            get { return fullArchiveTempDirectoryPath; }
            set { fullArchiveTempDirectoryPath = value; }
        }

        /// <summary>
        /// 地图完整的文件夹路径(后期提供);
        /// </summary>
        public string FullPrefabMapDirectoryPath
        {
            get { return fullPrefabMapDirectoryPath; }
            set { fullPrefabMapDirectoryPath = value; }
        }


        IReadOnlyWorldNode IReadOnlyMap<IntVector2, IReadOnlyWorldNode>.this[IntVector2 position]
        {
            get{ return base[position]; }
        }

        bool IReadOnlyMap<IntVector2, IReadOnlyWorldNode>.TryGetValue(IntVector2 position, out IReadOnlyWorldNode item)
        {
            WorldNode worldNode;
            if (base.TryGetValue(position, out worldNode))
            {
                item = worldNode;
                return true;
            }
            item = default(IReadOnlyWorldNode);
            return false;
        }


        /// <summary>
        /// 获取到地图块名称;
        /// </summary>
        internal string GetBlockName(ShortVector2 address)
        {
            return addressPrefix + address.GetHashCode();
        }

        private string GetFullPrefabMapFilePath(ShortVector2 address)
        {
            string blockName = GetBlockName(address);
            string fullPrefabMapFilePath = Path.Combine(fullPrefabMapDirectoryPath, blockName);
            return fullPrefabMapFilePath;
        }

        private string GetFullArchiveTempFilePath(ShortVector2 address)
        {
            string blockName = GetBlockName(address);
            string fullArchiveTempFilePath = Path.Combine(fullArchiveTempDirectoryPath, blockName);
            return fullArchiveTempFilePath;
        }


        /// <summary>
        /// 强制保存加载到游戏内的所有地图块;
        /// </summary>
        public void SaveAllBlock()
        {
            foreach (var blockPair in MapCollection)
            {
                SaveArchiveMap(blockPair.Key, blockPair.Value);
            }
        }

        /// <summary>
        /// 将缓存的存档地图添加保存到预制地图内;
        /// </summary>
        /// <param name="deleteArchivedTemp">保存完毕后是否删除缓存的存档地图</param>
        public void CombineToPrefabMap(bool deleteArchivedTemp = false)
        {
            string[] archiveMapFilePaths = Directory.GetFiles(fullArchiveTempDirectoryPath, ArchivedSearchPattern);

            foreach (var archiveMapFilePath in archiveMapFilePaths)
            {
                CombineToPrefabMap(archiveMapFilePath);
            }

            if (deleteArchivedTemp)
                DeleteMapFileAll(fullArchiveTempDirectoryPath);
        }

        /// <summary>
        /// 根据这个路径在预制地图文件夹内寻找相同名的地图文件,进行合并;
        /// </summary>
        private void CombineToPrefabMap(string fullArchiveMapFilePath)
        {
            string fileName = Path.GetFileName(fullArchiveMapFilePath);
            string prefabMapFilePath = Path.Combine(fullPrefabMapDirectoryPath, fileName);
            CombineMap(fullArchiveMapFilePath, prefabMapFilePath);
        }

        /// <summary>
        /// 读取预制和缓存地图,进行合并后覆盖保存预制地图;
        /// 若预制不存在则直接拷贝缓存地图;
        /// </summary>
        private void CombineMap(string fullArchiveMapFilePath, string fullPrefabMapFilePath)
        {
            Dictionary<ShortVector2, WorldNode> archiveMap;
            Dictionary<ShortVector2, WorldNode> prefabMap;

            if (TryLoadMapBlock(fullArchiveMapFilePath, out archiveMap))
            {
                if (TryLoadMapBlock(fullPrefabMapFilePath, out prefabMap))
                {
                    prefabMap.AddOrReplace(archiveMap);
                    Save(fullPrefabMapFilePath, prefabMap);
                }
                else
                {
                    File.Copy(fullArchiveMapFilePath, fullPrefabMapFilePath);
                }
            }
        }

        /// <summary>
        /// 删除路径文件夹内的所有地图文件;
        /// </summary>
        /// <param name="fullDirectoryPath"></param>
        private void DeleteMapFileAll(string fullDirectoryPath)
        {
            FileHelper.DeleteFileInDirectory(fullDirectoryPath, ArchivedSearchPattern);
        }


        protected override MapBlock GetBlock(ShortVector2 blockAddress)
        {
            Dictionary<ShortVector2, WorldNode> prefabMap;
            Dictionary<ShortVector2, WorldNode> archiveMap;
            MapBlock mapBlock;

            if (TryLoadPrefabMapBlock(blockAddress, out prefabMap))
            {
                if (TryLoadArchiveMapBlock(blockAddress, out archiveMap))
                {
#if DETAILED_DEBUG
                    Debug.Log(blockAddress + "找到预制地图 和 存档地图");
#endif
                    mapBlock = new MapBlock(prefabMap, archiveMap);
                }
                else
                {
#if DETAILED_DEBUG
                    Debug.Log(blockAddress + "找到预制地图");
#endif
                    mapBlock = new MapBlock(prefabMap, true);
                }
            }
            else
            {
                if (TryLoadArchiveMapBlock(blockAddress, out archiveMap))
                {
#if DETAILED_DEBUG
                    Debug.Log(blockAddress + "找到存档地图");
#endif
                    mapBlock = new MapBlock(archiveMap, false);
                }
                else
                {
#if DETAILED_DEBUG
                    Debug.Log(blockAddress + "未找到地图");
#endif
                    mapBlock = new MapBlock();
                }
            }
            return mapBlock;
        }

        private bool TryLoadArchiveMapBlock(ShortVector2 blockAddress, out Dictionary<ShortVector2, WorldNode> archiveMap)
        {
            string fullArchiveTempFilePath = GetFullArchiveTempFilePath(blockAddress);
            return TryLoadMapBlock(fullArchiveTempFilePath, out archiveMap);
        }

        private bool TryLoadPrefabMapBlock(ShortVector2 blockAddress, out Dictionary<ShortVector2, WorldNode> archiveMap)
        {
            string fullPrefabMapFilePath = GetFullPrefabMapFilePath(blockAddress);
            return TryLoadMapBlock(fullPrefabMapFilePath, out archiveMap);
        }

        private Dictionary<ShortVector2, WorldNode> LoadPrefabMapBlock(ShortVector2 blockAddress)
        {
            string fullPrefabMapFilePath = GetFullPrefabMapFilePath(blockAddress);
            return LoadMapBlock(fullPrefabMapFilePath);
        }

        private bool TryLoadMapBlock(string fullFilePath, out Dictionary<ShortVector2, WorldNode> mapBlock)
        {
            try
            {
                mapBlock = LoadMapBlock(fullFilePath);
                return true;
            }
            catch (Exception e)
            {
#if DETAILED_DEBUG
                Debug.Log("尝试读取地图" + fullFilePath + "未成功;\n" + e);
#endif
                mapBlock = default(Dictionary<ShortVector2, WorldNode>);
                return false;
            }
        }

        private Dictionary<ShortVector2, WorldNode> LoadMapBlock(string fullFilePath)
        {
            var dictionary = SerializeHelper.Deserialize_ProtoBuf<Dictionary<ShortVector2, WorldNode>>(fullFilePath);
            return dictionary;
        }


        protected override void ReleaseBlock(ShortVector2 blockAddress, MapBlock block)
        {
            SaveArchiveMap(blockAddress, block);
        }

        private void SaveArchiveMap(ShortVector2 blockAddress, MapBlock block)
        {
            Dictionary<ShortVector2, WorldNode> archiveMap = block.ArchiveMap;

            if (archiveMap.Count != 0)
            {
                SaveArchiveMap(blockAddress, archiveMap);
            }
#if DETAILED_DEBUG
            else
            {
                Debug.Log("未改变的地图,跳过保存地图块 :" + blockAddress.ToString());
            }
#endif
        }

        private void SaveArchiveMap(ShortVector2 blockAddress, Dictionary<ShortVector2, WorldNode> archiveMap)
        {
            string fullArchiveTempFilePath = GetFullArchiveTempFilePath(blockAddress);
            Save(fullArchiveTempFilePath, archiveMap);
#if DETAILED_DEBUG
            Debug.Log(blockAddress + "地图保存成功;" + fullArchiveTempFilePath);
#endif
        }

        private void Save(string fullFilePath, Dictionary<ShortVector2, WorldNode> mapBlock)
        {
            SerializeHelper.Serialize_ProtoBuf(fullFilePath, mapBlock);
        }


        /// <summary>
        /// 区分修改过的地图块和原本地图块;
        /// </summary>
        public class MapBlock : IMap<ShortVector2, WorldNode>, IReadOnlyMap<ShortVector2, IReadOnlyWorldNode>
        {
            public MapBlock()
            {
                this.prefabMap = new Dictionary<ShortVector2, WorldNode>();
                this.archiveMap = new Dictionary<ShortVector2, WorldNode>();
            }

            public MapBlock(Dictionary<ShortVector2, WorldNode> map, bool isPrefabMap = true)
            {
                if (isPrefabMap)
                {
                    this.prefabMap = map;
                    this.archiveMap = new Dictionary<ShortVector2, WorldNode>(map.Count);
                }
                else
                {
                    this.prefabMap = new Dictionary<ShortVector2, WorldNode>(map);
                    this.archiveMap = map;
                }
            }
            public MapBlock(Dictionary<ShortVector2, WorldNode> prefabMap, Dictionary<ShortVector2, WorldNode> archiveMap)
            {
                this.prefabMap = prefabMap;
                this.archiveMap = archiveMap;
            }

            /// <summary>
            ///  预制的地图;
            /// </summary>
            private Dictionary<ShortVector2, WorldNode> prefabMap;
            /// <summary>
            /// 修改过的节点合集,节点数不会多余预制地图;
            /// </summary>
            private Dictionary<ShortVector2, WorldNode> archiveMap;

            /// <summary>
            /// 预制的地图;
            /// </summary>
            public Dictionary<ShortVector2, WorldNode> PrefabMap
            {
                get { return prefabMap; }
            }
            /// <summary>
            /// 修改过的节点合集,节点数不会多余预制地图;
            /// </summary>
            public Dictionary<ShortVector2, WorldNode> ArchiveMap
            {
                get { return archiveMap; }
            }

            IReadOnlyWorldNode IReadOnlyMap<ShortVector2, IReadOnlyWorldNode>.this[ShortVector2 position]
            {
                get { return this.prefabMap[position]; }
            }

            public WorldNode this[ShortVector2 position]
            {
                get
                {
                    WorldNode node;
                    if (prefabMap.TryGetValue(position, out node))
                    {
                        TryAddArchiveMap(position, node);
                        return node;
                    }
                    throw new KeyNotFoundException();
                }
                set
                {
                    if (ArchiveMap.ContainsKey(position))
                    {
                        PrefabMap[position] = value;
                        ArchiveMap[position] = value;
                    }
                    else if(PrefabMap.ContainsKey(position))
                    {
                        PrefabMap[position] = value;
                        ArchiveMap.Add(position, value);
                    }
                    throw new KeyNotFoundException();
                }
            }

            public void Add(ShortVector2 position, WorldNode item)
            {
                this.prefabMap.Add(position, item);
                this.archiveMap.Add(position, item);
            }

            public bool Remove(ShortVector2 position)
            {
                this.archiveMap.Remove(position);
                return this.prefabMap.Remove(position);
            }

            public bool Contains(ShortVector2 position)
            {
                return this.prefabMap.ContainsKey(position);
            }

            public bool TryGetValue(ShortVector2 position, out WorldNode item)
            {
                if (this.prefabMap.TryGetValue(position, out item))
                {
                    TryAddArchiveMap(position, item);
                    return true;
                }
                return false;
            }

            public void Clear()
            {
                this.prefabMap.Clear();
                this.archiveMap.Clear();
            }

            private void TryAddArchiveMap(ShortVector2 position, WorldNode node)
            {
                if(!archiveMap.ContainsKey(position))
                    archiveMap.Add(position, node);
            }


            bool IReadOnlyMap<ShortVector2, IReadOnlyWorldNode>.Contains(ShortVector2 position)
            {
                return this.prefabMap.ContainsKey(position);
            }

            bool IReadOnlyMap<ShortVector2, IReadOnlyWorldNode>.TryGetValue(ShortVector2 position, out IReadOnlyWorldNode item)
            {
                WorldNode node;
                if (prefabMap.TryGetValue(position, out node))
                {
                    item = node;
                    return true;
                }
                item = default(IReadOnlyWorldNode);
                return false;
            }

        }

    }

}
