using System;

namespace JiongXiaGu.Unity.Initializers
{

    [Flags]
    public enum InitializeOptions
    {
        None = 1 << 0,

        /// <summary>
        /// 忽略初始化过程中的异常;
        /// </summary>
        IgnoreException = 1 << 1,

        /// <summary>
        /// 仅在Unity线程初始化,若不是Unity线程则转到;
        /// </summary>
        RunInUnityThread = 1 << 2,
    }
}
