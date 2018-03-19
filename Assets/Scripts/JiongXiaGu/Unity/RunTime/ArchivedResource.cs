using JiongXiaGu.Collections;
using JiongXiaGu.Unity.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Grids;

namespace JiongXiaGu.Unity.RunTime
{

    /// <summary>
    /// 游戏存档资源;
    /// </summary>
    public class ArchivedResource
    {

        /// <summary>
        /// 存档地图;
        /// </summary>
        public SerializableDictionary<RectCoord, ArchiveMapNode> Map { get; set; }
    }
}
