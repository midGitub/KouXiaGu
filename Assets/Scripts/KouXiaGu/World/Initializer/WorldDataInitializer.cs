using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    /// <summary>
    /// 游戏世界数据初始化;
    /// </summary>
    [Serializable]
    public class WorldDataInitializer : AsyncInitializer<IWorldData>
    {
        WorldDataInitializer()
        {
        }

        public override string Prefix
        {
            get { return "场景数据"; }
        }

        public void Start(IGameData gameData, Archive archive)
        {
            StartInitialize();

        }



    }

}
