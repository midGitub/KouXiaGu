using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源读取顺序文件读取;
    /// </summary>
    internal class LoadOrderFileReader : ConfigFileReader<LoadOrder>
    {
        /// <summary>
        /// 游戏地图存放目录;
        /// </summary>
        [PathDefinition(PathDefinition.DataDirectory, "资源读取顺序文件")]
        internal const string DataOrderRecordFileName = "Configs/LoadOrder";

        public LoadOrderFileReader() : base(new XmlSerializer<LoadOrder>())
        {
        }

        public LoadOrderFileReader(ISerializer<LoadOrder> serializer) : base(serializer)
        {
        }

        public override string GetFilePathWithoutExtension()
        {
            return Path.Combine(ResourcePath.CoreDirectory.FullName, DataOrderRecordFileName);
        }
    }
}
