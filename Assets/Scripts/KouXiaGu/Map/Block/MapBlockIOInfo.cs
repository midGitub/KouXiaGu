using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Map
{

    public interface IMapBlockEditIOInfo
    {
        string GetBlockName(ShortVector2 address);
        string GetFullPrefabMapDirectoryPath();
        string GetFullPrefabMapFilePath(ShortVector2 address);
    }

    public interface IMapBlockIOInfo : IMapBlockEditIOInfo
    {
        string GetFullArchiveTempDirectoryPath();
        string GetFullArchiveTempFilePath(ShortVector2 address);
    }

    /// <summary>
    /// 地图块信息;
    /// </summary>
    [Serializable]
    public struct MapBlockIOInfo
    {
        /// <summary>
        /// 地图归档临时文件夹路径;
        /// </summary>
        [SerializeField]
        public string archiveTempDirectoryPath;
        /// <summary>
        /// 地图文件前缀;
        /// </summary>
        [SerializeField]
        public string addressPrefix;
        /// <summary>
        /// 地图存档到的文件夹路径;
        /// </summary>
        [SerializeField]
        public string archivedDirectoryPath;

    }

}
