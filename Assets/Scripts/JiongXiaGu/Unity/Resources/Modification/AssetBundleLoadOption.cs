using System;

namespace JiongXiaGu.Unity.Resources
{

    [Flags]
    public enum AssetBundleLoadOption
    {
        None = 0,
        Important = 1 << 0,
        NotImportant = 1 << 1,
    }
}
