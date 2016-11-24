using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D
{


    public static partial class WorldConvert
    {

        #region HexDirection

        /// <summary>
        /// 按标记为从 低位到高位 循序排列的数组;
        /// </summary>
        static readonly HexDirection[] DirectionMarked = new HexDirection[] 
        {
            HexDirection.North,
            HexDirection.Northeast,
            HexDirection.Southeast,
            HexDirection.South,
            HexDirection.Southwest,
            HexDirection.Northwest,
        };

        /// <summary>
        /// 返回按标记为从 低位到高位 循序返回的迭代结构;
        /// </summary>
        public static IEnumerable<HexDirection> HexDirections()
        {
            return DirectionMarked;
        }

        #endregion

    }

}
