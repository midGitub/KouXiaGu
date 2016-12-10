using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.HexTerrain
{


    public interface IBlockArchive<TP, T>
    {

        /// <summary>
        /// 确认是否已经存在这个地图块;
        /// </summary>
        bool Contains(ShortVector2 coord);

        /// <summary>
        /// 返回需要保存的地图块存档结构;
        /// </summary>
        BlockArchive<TP, T>[] Save();

        /// <summary>
        /// 返回所有地图块存档结构;
        /// </summary>
        BlockArchive<TP, T>[] SaveAll();

        /// <summary>
        /// 将存档结构加入到地图内;
        /// </summary>
        void Load(BlockArchive<CubicHexCoord, T> archive);
    }

}
