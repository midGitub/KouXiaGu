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
    public class DataOrderRecordReader : ConfigFileReader<DataOrderRecord>
    {
        /// <summary>
        /// 游戏地图存放目录;
        /// </summary>
        [PathDefinition(ResourceTypes.DataDirectory, "资源读取顺序文件")]
        internal const string DataOrderRecordFileName = "Configs/DataOrderRecord";

        public DataOrderRecordReader() : base(new XmlFileSerializer<DataOrderRecord>())
        {
        }

        public DataOrderRecordReader(ISerializer<DataOrderRecord> serializer) : base(serializer)
        {
        }

        public override string GetFilePathWithoutExtension()
        {
            return Path.Combine(Resource.CoreDataDirectory, DataOrderRecordFileName);
        }
    }
}
