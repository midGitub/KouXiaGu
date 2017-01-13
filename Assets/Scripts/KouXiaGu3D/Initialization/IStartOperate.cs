using System;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 在每个阶段起始时进行的操作;
    /// </summary>
    public interface IStartOperate
    {

        /// <summary>
        /// 是否完成?
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
        /// 初始化内容;
        /// </summary>
        void Initialize();
    }

}
