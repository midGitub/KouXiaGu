using System;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 地图块,分页信息;
    /// </summary>
    [Serializable]
    public struct DynamicPagingInfo
    {
        public DynamicPagingInfo(string dataDirectoryPath, ShortVector2 partitionSizes,
            ShortVector2 targetRadiationRange, string addressPrefix)
        {
            this.dataDirectoryPath = dataDirectoryPath;
            this.partitionSizes = partitionSizes;
            this.targetRadiationRange = targetRadiationRange;
            this.addressPrefix = addressPrefix;
        }

        [SerializeField]
        private string dataDirectoryPath;
        [SerializeField]
        private ShortVector2 partitionSizes;
        [SerializeField]
        private ShortVector2 targetRadiationRange;
        [SerializeField]
        private string addressPrefix;

        /// <summary>
        /// 地图文件路径;
        /// </summary>
        public string DataDirectoryPath
        {
            get { return dataDirectoryPath; }
        }

        /// <summary>
        /// 地图分区大小;
        /// </summary>
        public ShortVector2 PartitionSizes
        {
            get { return partitionSizes; }
        }

        /// <summary>
        /// 目标辐射范围,既以为目标中心,需要读取的地图范围;
        /// X,Y应该都为正数;
        /// </summary>
        public ShortVector2 TargetRadiationRange
        {
            get { return targetRadiationRange; }
        }

        /// <summary>
        /// 获取到地图块名
        /// </summary>
        public string GetMapPagingName(ShortVector2 address)
        {
            return addressPrefix + address.GetHashCode();
        }

    }

}
