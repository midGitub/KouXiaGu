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
    public class WNodeBlockMap : BlockMap<WorldNode, WNodeBlockMap.MapBlock>, IReadOnlyMap<IntVector2, IReadOnlyWorldNode>
    {
        public WNodeBlockMap(
            string fullArchiveTempDirectoryPath,
            string fullMapDirectoryPath,
            ShortVector2 partitionSizes,
            ShortVector2 minRadiationRange,
            ShortVector2 maxRadiationRange) 
            : base(partitionSizes, minRadiationRange, maxRadiationRange)
        {
            this.fullArchiveTempDirectoryPath = fullArchiveTempDirectoryPath;
            this.fullMapDirectoryPath = fullMapDirectoryPath;
        }

        private string addressPrefix;
        private string fullArchiveTempDirectoryPath;
        private string fullMapDirectoryPath;

        public string AddressPrefix
        {
            get { return addressPrefix; }
            set { addressPrefix = value; }
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
            get { return fullMapDirectoryPath; }
            set { fullMapDirectoryPath = value; }
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
        private string GetBlockName(ShortVector2 address)
        {
            return addressPrefix + address.GetHashCode();
        }

        private string GetFullPrefabMapFilePath(ShortVector2 address)
        {
            string blockName = GetBlockName(address);
            string fullPrefabMapFilePath = Path.Combine(fullMapDirectoryPath, blockName);
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

        protected override MapBlock GetBlock(ShortVector2 blockAddress)
        {
            Dictionary<ShortVector2, WorldNode> prefabMap;
            Dictionary<ShortVector2, WorldNode> archiveMap;
            MapBlock mapBlock;

            prefabMap = LoadPrefabMapBlock(blockAddress);

            if (TryLoadArchiveMapBlock(blockAddress, out archiveMap))
            {
                mapBlock = new MapBlock(prefabMap, archiveMap);
            }
            else
            {
                mapBlock = new MapBlock(prefabMap);
            }

            return mapBlock;
        }

        private bool TryLoadArchiveMapBlock(ShortVector2 blockAddress, out Dictionary<ShortVector2, WorldNode> archiveMap)
        {
            string fullArchiveTempFilePath = GetFullArchiveTempFilePath(blockAddress);
            return TryLoadMapBlock(blockAddress, fullArchiveTempFilePath, out archiveMap);
        }

        private Dictionary<ShortVector2, WorldNode> LoadPrefabMapBlock(ShortVector2 blockAddress)
        {
            string fullPrefabMapFilePath = GetFullPrefabMapFilePath(blockAddress);
            return LoadMapBlock(blockAddress, fullPrefabMapFilePath);
        }

        private bool TryLoadMapBlock(ShortVector2 blockAddress, string fullFilePath, out Dictionary<ShortVector2, WorldNode> dictionary)
        {
            try
            {
                dictionary = LoadMapBlock(blockAddress, fullFilePath);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("读取存档地图" + blockAddress.ToString() + "未成功;\n" + e);
            }

            dictionary = default(Dictionary<ShortVector2, WorldNode>);
            return false;
        }

        private Dictionary<ShortVector2, WorldNode> LoadMapBlock(ShortVector2 blockAddress, string fullFilePath)
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
            else
            {
                Debug.Log("未改变的地图,跳过保存地图块 :" + blockAddress.ToString());
            }
        }

        private void SaveArchiveMap(ShortVector2 blockAddress, Dictionary<ShortVector2, WorldNode> archiveMap)
        {
            string fullArchiveTempFilePath = GetFullArchiveTempFilePath(blockAddress);
            Save(fullArchiveTempFilePath, archiveMap);
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
            public MapBlock(Dictionary<ShortVector2, WorldNode> prefabMap)
            {
                this.prefabMap = prefabMap;
                this.archiveMap = new Dictionary<ShortVector2, WorldNode>(prefabMap.Count);
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
            /// 修改过的节点合集;元素不会多于预制地图;
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
            /// 修改过的节点合集;元素不会多于预制地图;
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
