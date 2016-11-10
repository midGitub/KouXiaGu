using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏运行的状态;
    /// </summary>
    public enum GameStatus
    {
        /// <summary>
        /// 在进入到主界面,允许开始创建游戏;
        /// </summary>
        Ready,

        /// <summary>
        /// 正在创建游戏;
        /// </summary>
        Creating,

        /// <summary>
        /// 游戏正在运行;
        /// </summary>
        Running,

        /// <summary>
        /// 游戏正在保存中;
        /// </summary>
        Saving,

    }

}
