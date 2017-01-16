using System;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 在每个阶段起始时进行的操作;
    /// </summary>
    public interface IStartOperate : IOperateAsync
    {
        /// <summary>
        /// 初始化内容;
        /// </summary>
        void Initialize();
    }

}
