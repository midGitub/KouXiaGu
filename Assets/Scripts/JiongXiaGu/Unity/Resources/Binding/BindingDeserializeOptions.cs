using System;

namespace JiongXiaGu.Unity.Resources.Binding
{
    [Flags]
    public enum BindingDeserializeOptions
    {
        None = 0,

        /// <summary>
        /// 忽略不存在的文件;
        /// </summary>
        IgnoreNonexistent = 1,
    }
}
