using System;

namespace JiongXiaGu.Unity.Resources.Binding
{
    /// <summary>
    /// 使用Json方式读取数据;
    /// </summary>
    [Obsolete("还未实现")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class JsonAssetAttribute : AssetAttribute
    {
        public override string FileExtension => ".json";

        public JsonAssetAttribute(string relativePath) : base(relativePath)
        {
        }

        public JsonAssetAttribute(string relativePath, string message) : base(relativePath, message)
        {
        }

        public override ISerializer GetSerializer(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
