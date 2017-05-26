using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 游戏基本资源;
    /// </summary>
    public class BasicResource
    {
        public BasicResource()
        {
        }

        public BasicTerrainResource Terrain { get; internal set; }
    }
}
