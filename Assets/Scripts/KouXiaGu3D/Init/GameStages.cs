using System;

namespace KouXiaGu.Initialization
{

    [Flags]
    public enum GameStages
    {

        /// <summary>
        /// 未知状态;
        /// </summary>
        Empty = 0,

        /// <summary>
        /// 读取游戏中;
        /// </summary>
        Loading = 1,

        /// <summary>
        /// 等待游戏开始;
        /// </summary>
        WaitStart = 2,

        /// <summary>
        /// 游戏进行中;
        /// </summary>
        InGame = 4,

        /// <summary>
        /// 正在保存游戏;
        /// </summary>
        Saving = 8,

        /// <summary>
        /// 退出游戏;
        /// </summary>
        Quitting = 16,

        /// <summary>
        /// 正在进行阶段的过度;
        /// </summary>
        Running = 32,

    }

}
