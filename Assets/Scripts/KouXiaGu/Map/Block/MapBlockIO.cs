using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 地图块 保存和读取信息;
    /// </summary>
    [Serializable]
    public struct MapBlockIOInfo
    {
        /// <summary>
        /// 预制地图文件夹路径;
        /// </summary>
        [SerializeField]
        private string prefabMapDirectoryPath;
        /// <summary>
        /// 归档地图路径(零时存放的路径);
        /// </summary>
        [SerializeField]
        private string archiveMapirectoryPath;
        /// <summary>
        /// 地图块前缀;
        /// </summary>
        [SerializeField]
        private string addressPrefix;

        /// <summary>
        /// 获取到预制地图的文件路径;
        /// </summary>
        public string GetPrefabMapDirectoryPath()
        {
            return Path.Combine(Application.dataPath, prefabMapDirectoryPath);
        }

        /// <summary>
        /// 获取到归档地图的文件路径;
        /// </summary>
        /// <returns></returns>
        public string GetArchiveMapirectoryPath()
        {
            return Path.Combine(Application.dataPath, archiveMapirectoryPath);
        }

        /// <summary>
        /// 获取到地图块名
        /// </summary>
        public string GetBlockName(ShortVector2 address)
        {
            return addressPrefix + address.GetHashCode();
        }

    }

    /// <summary>
    /// 地图读取保存;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MapBlockIO<T> : IMapBlockIO<MapBlock<T>, T>
    {

        public MapBlockIO(MapBlockIOInfo mapBlockIOInfo)
        {
            this.mapBlockIOInfo = mapBlockIOInfo;
        }

        private MapBlockIOInfo mapBlockIOInfo;

        public MapBlockIOInfo MapBlockIOInfo
        {
            get { return mapBlockIOInfo; }
        }

        public bool TryLoad(ShortVector2 address, out MapBlock<T> mapPaging)
        {
            throw new NotImplementedException();
        }

        public MapBlock<T> Load(ShortVector2 address)
        {
            throw new NotImplementedException();
        }

        public string Save(MapBlock<T> mapPaging)
        {
            throw new NotImplementedException();
        }

        public void SaveAsyn(MapBlock<T> mapPaging)
        {
            throw new NotImplementedException();
        }

        public void Delete(ShortVector2 address)
        {
            throw new NotImplementedException();
        }

    }

}
