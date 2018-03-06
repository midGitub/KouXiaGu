namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组内容;
    /// </summary>
    public abstract class ModificationSubresource
    {
        /// <summary>
        /// 模组内容类型名;
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// 本地化名称;
        /// </summary>
        public string LocName { get; private set; }

        /// <summary>
        /// 是否启用该资源?
        /// </summary>
        public bool IsEnabled { get; set; }

        protected ModificationSubresource(string type, string locName, bool isEnabled)
        {
            Type = type;
            LocName = locName;
            IsEnabled = isEnabled;
        }
    }
}
