using System;

namespace JiongXiaGu.Unity.Resources
{

    [Flags]
    public enum AssetBundleLoadOption
    {
        None = 0,
        Main = 1 << 0,
        Secondary = 1 << 1,
    }
}
