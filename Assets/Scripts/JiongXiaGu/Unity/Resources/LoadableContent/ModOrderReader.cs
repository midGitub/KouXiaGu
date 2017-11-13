//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JiongXiaGu.Unity.Resources
//{

//    /// <summary>
//    /// 资源读取顺序文件读取;
//    /// </summary>
//    internal class ModOrderReader : ConfigFileReader<ModOrder>
//    {
//        /// <summary>
//        /// 游戏地图存放目录;
//        /// </summary>
//        [PathDefinition(PathDefinition.DataDirectory, "资源读取顺序文件")]
//        internal const string DataOrderRecordFileName = "Configs/ModOrderRecord";

//        public ModOrderReader() : base(new XmlSerializer<ModOrder>())
//        {
//        }

//        public ModOrderReader(ISerializer<ModOrder> serializer) : base(serializer)
//        {
//        }

//        public override string GetFilePathWithoutExtension()
//        {
//            return Path.Combine(ResourcePath.CoreDirectory.FullName, DataOrderRecordFileName);
//        }
//    }
//}
