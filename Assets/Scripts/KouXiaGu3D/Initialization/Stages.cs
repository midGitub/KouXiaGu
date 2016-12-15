using System;

namespace KouXiaGu.Initialization
{

    [Flags]
    public enum Stages
    {

        /// <summary>
        /// 未知状态;
        /// </summary>
        Empty = 0,

        /// <summary>
        /// 正在进行阶段的过度;
        /// </summary>
        Running = 1,

        /// <summary>
        /// 开始界面;
        /// </summary>
        StartScreen = 2,

        /// <summary>
        /// 游戏进行中;
        /// </summary>
        Game = 4,

        /// <summary>
        /// 正在保存游戏;
        /// </summary>
        Saving = 8,

    }

}
