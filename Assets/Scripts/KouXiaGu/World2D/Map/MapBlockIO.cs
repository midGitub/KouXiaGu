using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 从存储文件读取地图块的方法;
    /// </summary>
    public class MapBlockIO<T> : IMapBlockIO<MapBlock<T>>, IMapBlockInfo
        where T : struct
    {

        public string AddressPrefix { get; private set; }
        public string FullArchiveTempDirectoryPath { get; private set; }
        public string FullPrefabMapDirectoryPath { get; private set; }

        public MapBlock<T> Load(ShortVector2 address)
        {
            return this.LoadMapBlock<T>(address);
        }

        public void Save(ShortVector2 address, MapBlock<T> block)
        {
            this.SaveArchiveMapBlockOrNot(address, block);
        }

    }

}
