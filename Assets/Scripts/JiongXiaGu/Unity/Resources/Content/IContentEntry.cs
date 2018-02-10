using System;

namespace JiongXiaGu.Unity.Resources
{
    public interface IContentEntry
    {
        string Name { get; }
        DateTime LastWriteTime { get; }
    }
}
