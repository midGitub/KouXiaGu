using System.Collections;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 进行的阶段;
    /// </summary>
    public interface IPeriod
    {
        /// <summary>
        /// 代表当前游戏进行的阶段;
        /// </summary>
        GameStages Deputy { get; }

        /// <summary>
        /// 一个瞬时的状态,如保存游戏,和读取游戏;
        /// 值执行 OnEnter() ,执行完毕后恢复之前的状态;
        /// </summary>
        bool Instant { get; }

        /// <summary>
        /// 是否允许进入当前阶段?允许返回true;
        /// </summary>
        bool Premise();

        /// <summary>
        /// 当进入状态栈时调用;
        /// </summary>
        IAsyncOperate OnEnter();

        /// <summary>
        /// 当弹出状态栈时调用;
        /// </summary>
        IAsyncOperate OnLeave();
    }


}
