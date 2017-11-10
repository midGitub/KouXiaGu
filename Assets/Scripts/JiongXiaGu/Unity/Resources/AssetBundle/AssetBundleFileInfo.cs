//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO;

//namespace JiongXiaGu.Unity.Resources
//{


//    public class AssetBundleFileInfo
//    {
//        /// <summary>
//        /// 数据类型信息;
//        /// </summary>
//        public ModInfo DataInfo { get; private set; }

//        /// <summary>
//        /// AssetBundle 文件信息;
//        /// </summary>
//        public FileInfo FileInfo { get; private set; }

//        public AssetBundleFileInfo(ModInfo dataInfo, FileInfo fileInfo)
//        {
//            if (dataInfo == null)
//                throw new ArgumentNullException(nameof(dataInfo));
//            if (fileInfo == null)
//                throw new ArgumentNullException(nameof(fileInfo));

//            DataInfo = dataInfo;
//            FileInfo = fileInfo;
//        }
//    }
//}
