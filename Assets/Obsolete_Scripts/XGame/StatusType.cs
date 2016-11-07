using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame
{

    /// <summary>
    /// 游戏当前的允许状态;
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// 游戏初始状态;
        /// </summary>
        Ready,

        /// <summary>
        /// 模组资源正在读取中;
        /// </summary>
        ModLoading,

        /// <summary>
        /// 模组资源读取完毕,允许初始化游戏;
        /// </summary>
        WaitStart,

        /// <summary>
        /// 游戏正在读取中(初始化游戏);
        /// </summary>
        GameLoading,

        /// <summary>
        /// 游戏准备完毕,允许开始游戏;
        /// </summary>
        [Obsolete]
        GameReady,

        /// <summary>
        /// 游戏允许中;
        /// </summary>
        GameRunning,

        /// <summary>
        /// 游戏正在保存;
        /// </summary>
        Saving,

        /// <summary>
        /// 正在推出游戏到主界面;
        /// </summary>
        QuittingGame,

    }

}
