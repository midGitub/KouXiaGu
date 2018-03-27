using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace JiongXiaGu.Unity.Resources.BindingSerialization
{

    /// <summary>
    /// 使用ProtoBuf方式读取数据;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ProtoAssetAttribute : AssetAttribute
    {
        public const string defaultFileExtension = ".proto";
        public override string DefaultFileExtension => DefaultFileExtension;

        public ProtoAssetAttribute(string relativePath) : base(relativePath)
        {
        }

        public ProtoAssetAttribute(string relativePath, bool useDefaultExtension) : base(relativePath, useDefaultExtension)
        {
        }

        public ProtoAssetAttribute(string relativePath, string name, string message) : base(relativePath, name, message)
        {
        }

        public ProtoAssetAttribute(string relativePath, string name, string message, bool useDefaultExtension) : base(relativePath, name, message, useDefaultExtension)
        {
        }

        public override ISerializer CreateSerializer(Type type)
        {
            return new ProtoSerializer(type);
        }
    }
}
