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
        [SerializeField]
        public string archiveTempDirectoryPath;
        [SerializeField]
        public string addressPrefix;
        [Header("存档保存"), SerializeField]
        public string archivedDirectoryPath;

    }

}
