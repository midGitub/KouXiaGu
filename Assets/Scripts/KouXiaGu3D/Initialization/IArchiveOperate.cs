using System;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 游戏存档操作;
    /// </summary>
    public interface IArchiveOperate
    {

        /// <summary>
        /// 游戏是否保存完成?
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// 是否由于未经处理异常的原因而完成;
        /// </summary>
        bool IsFaulted { get; }

        /// <summary>
        /// 导致提前结束的异常;
        /// </summary>
        Exception Ex { get; }

        /// <summary>
        /// 进行保存;
        /// </summary>
        void Save(Archive archive);
    }

}
