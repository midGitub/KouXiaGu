using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 允许进行保存的块接口;
    /// </summary>
    public interface IBlockArchive<TP, T>
    {

        /// <summary>
        /// 确认是否已经存在这个地图块;
        /// </summary>
        bool Contains(ShortVector2 coord);

        /// <summary>
        /// 返回需要保存的地图块存档结构;
        /// </summary>
        BlockArchive<TP, T>[] GetArchives();

        /// <summary>
        /// 返回所有地图块存档结构;
        /// </summary>
        BlockArchive<TP, T>[] GetArchiveAll();

        /// <summary>
        /// 将存档结构加入到地图内;
        /// </summary>
        void AddArchives(BlockArchive<TP, T> archive);

    }

}
