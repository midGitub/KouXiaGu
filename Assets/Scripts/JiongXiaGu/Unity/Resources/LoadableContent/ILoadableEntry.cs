﻿namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// 表示可读资源入口;
    /// </summary>
    public interface ILoadableEntry
    {
        /// <summary>
        /// 相对路径,分隔符必须为 '\';
        /// </summary>
        string RelativePath { get; }
    }
}