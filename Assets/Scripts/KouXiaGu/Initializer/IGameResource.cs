using KouXiaGu.Terrain3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏资源;在游戏场景开始时初始化的资源;
    /// </summary>
    public interface IGameResource
    {
        TerrainResources Terrain { get; }
    }
}
