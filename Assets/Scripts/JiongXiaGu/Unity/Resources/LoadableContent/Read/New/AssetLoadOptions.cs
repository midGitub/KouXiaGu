namespace JiongXiaGu.Unity.Resources
{
    public enum AssetLoadOptions
    {
        None = 0,

        /// <summary>
        /// 重新读取,当 TaskStatus 为 Running 或 RanToCompletion 则重新读取;
        /// 为其它状态则不进行重新读取;
        /// </summary>
        RereadOrReplace = 1 >> 1,
    }
}
