using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectMaps
{


    public static class RectMap
    {
        /// <summary>
        /// 所有可用的地图(在进行初始化之后,仅提供Unity线程对此内容进行变更);
        /// </summary>
        internal static List<MapFileInfo> AvailableMaps { get; set; }
    }
}
