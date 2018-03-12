using JiongXiaGu.Unity.RectTerrain;
using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RunTime
{

    /// <summary>
    /// 游戏使用的所有资源;
    /// </summary>
    public class GameResource
    {
        public IReadOnlyList<Modification> Modifications { get; internal set; }
        public LandformResource LandformResource { get; internal set; }
    }
}
