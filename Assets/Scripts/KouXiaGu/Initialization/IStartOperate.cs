using System;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 在每个阶段起始时进行的操作;
    /// </summary>
    public interface IStartOperate : IAsyncOperation
    {
        /// <summary>
        /// 初始化内容,返回当所有内容初始化完成后的回调;
        /// </summary>
        Action Initialize();
    }

}
