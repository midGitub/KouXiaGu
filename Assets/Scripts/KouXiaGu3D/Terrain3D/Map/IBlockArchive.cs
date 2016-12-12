using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 允许进行保存的块接口;
    /// </summary>
    public interface IBlockArchive<TP, T>
    {

        /// <summary>
        /// 写入锁,防止在保存时其它线程对数据进行变更;
        /// </summary>
        object SyncWriteRoot { get; }

        /// <summary>
        /// 确认是否已经存在这个地图块;
        /// </summary>
        bool Contains(RectCoord coord);

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
        bool AddOrUpdateArchives(BlockArchive<TP, T> archive);

    }

}
