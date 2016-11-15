using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Map
{

    public class BlockEditMap<T> : MapBlockEditIO<T>, IMapBlockEditIOInfo
    {
        public BlockEditMap(string fullMapDirectoryPath, string addressPrefix, BlocksMapInfo blocksMapInfo) : base(blocksMapInfo)
        {
            this.fullMapDirectoryPath = fullMapDirectoryPath;
            this.addressPrefix = addressPrefix;

            base.MapBlockIOInfo = this;
        }

        /// <summary>
        /// 完整的地图文件夹路径;
        /// </summary>
        private string fullMapDirectoryPath;
        private string addressPrefix;




        public string GetBlockName(ShortVector2 address)
        {
            return addressPrefix + address.GetHashCode();
        }

        public string GetFullPrefabMapDirectoryPath()
        {
            return fullMapDirectoryPath;
        }

        public string GetFullPrefabMapFilePath(ShortVector2 address)
        {
            string blockName = GetBlockName(address);
            string fullPrefabMapFilePath = Path.Combine(fullMapDirectoryPath, blockName);
            return fullPrefabMapFilePath;
        }

    }

}
