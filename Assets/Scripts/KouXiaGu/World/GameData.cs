using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.World
{

    /// <summary>
    /// 游戏数据,启动程序时读取;
    /// </summary>
    public class GameData
    {

        /// <summary>
        /// 基础信息;
        /// </summary>
        public WorldElementManager ElementInfo { get; private set; }

        /// <summary>
        /// 地形资源读取;
        /// </summary>
        public TerrainResource Terrain { get; private set; }

        void Initialize()
        {
            ElementInfo = WorldElementManager.Read();
            Terrain = new TerrainResource(ElementInfo);
        }

    }

}
