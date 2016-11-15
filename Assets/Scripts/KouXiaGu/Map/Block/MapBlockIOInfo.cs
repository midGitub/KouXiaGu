using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Map
{

    public interface IMapBlockIOInfo
    {
        string GetBlockName(ShortVector2 address);
        string GetFullArchiveTempDirectoryPath();
        string GetFullArchiveTempFilePath(ShortVector2 address);
        string GetFullPrefabMapDirectoryPath();
        string GetFullPrefabMapFilePath(ShortVector2 address);
    }

    /// <summary>
    /// 地图块信息;
    /// </summary>
    [Serializable]
    public struct MapBlockIOInfo
    {

        [Header("运行时")]
        /// <summary>
        /// 预制地图文件夹路径;
        /// </summary>
        [SerializeField]
        public string prefabMapDirectoryPath;
        /// <summary>
        /// 归档地图路径(零时存放的路径);
        /// </summary>
        [SerializeField]
        public string archiveTempDirectoryPath;
        /// <summary>
        /// 地图块前缀;
        /// </summary>
        [SerializeField]
        public string addressPrefix;

        /// <summary>
        /// 保存到存档的位置;
        /// </summary>
        [Header("存档保存")]
        [SerializeField]
        public string archivedDirectoryPath;


        public string GetFullPrefabMapDirectoryPath()
        {
            string fullPrefabMapDirectoryPath = Path.Combine(Application.dataPath, this.prefabMapDirectoryPath);
            return fullPrefabMapDirectoryPath;
        }

        public string GetFullPrefabMapFilePath(ShortVector2 address)
        {
            string fullPrefabMapDirectoryPath = GetFullPrefabMapDirectoryPath();
            string blockName = GetBlockName(address);
            string fullPrefabMapFilePath = Path.Combine(fullPrefabMapDirectoryPath, blockName);
            return fullPrefabMapFilePath;
        }

        public string GetFullArchiveTempDirectoryPath()
        {
            string fullArchiveTempirectoryPath = Path.Combine(Application.dataPath, this.archiveTempDirectoryPath);
            return fullArchiveTempirectoryPath;
        }

        public string GetFullArchiveTempFilePath(ShortVector2 address)
        {
            string fullArchiveTempirectoryPath = GetFullArchiveTempDirectoryPath();
            string blockName = GetBlockName(address);
            string fullArchiveTempFilePath = Path.Combine(fullArchiveTempirectoryPath, blockName);
            return fullArchiveTempFilePath;
        }

        public string GetBlockName(ShortVector2 address)
        {
            return addressPrefix + address.GetHashCode();
        }


        public string GetFullArchivedDirectoryPath(ArchivedGroup item)
        {
            string fullArchivedDirectoryPath = Path.Combine(item.ArchivedPath, archivedDirectoryPath);
            return fullArchivedDirectoryPath;
        }

    }

}
