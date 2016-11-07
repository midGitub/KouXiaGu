using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame
{

    /// <summary>
    /// 模块的类型;区分初始化循序;从最小值开始初始化;
    /// </summary>
    public enum CallOrder : byte
    {

        /// <summary>
        /// 结构;不依赖其它数据的信息;如读取地图初始大小,读取默认信息,实例化类;
        /// </summary>
        Static,

        /// <summary>
        /// 存在依赖,如通过地图大小创建地图,从存档读取地图;
        /// </summary>
        Instance,

        /// <summary>
        /// 放置物体,完全依赖整个游戏的物体,比如说玩家放置的物品在读档恢复时调用;
        /// </summary>
        Place,

        /// <summary>
        /// UI;
        /// </summary>
        UI,

    }

}
