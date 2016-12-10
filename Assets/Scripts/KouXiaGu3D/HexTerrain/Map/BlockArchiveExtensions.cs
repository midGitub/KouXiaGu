using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 对 IBlockArchive 接口的拓展;
    /// </summary>
    public static class BlockArchiveExtensions
    {

        /// <summary>
        /// 保存到这个文件夹下;
        /// </summary>
        public static void Save<TP, T>(this BlockArchive<TP, T> block, string directoryPath)
        {
            
        }

    }

}
