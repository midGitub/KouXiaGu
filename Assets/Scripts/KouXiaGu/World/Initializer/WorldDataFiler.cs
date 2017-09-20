//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO;
//using UnityEngine;
//using KouXiaGu.Resources;
//using KouXiaGu.Resources.Archives;

//namespace KouXiaGu.World
//{

//    [Serializable]
//    public class WorldDataFiler
//    {
//        /// <summary>
//        /// 地图数据路径;
//        /// </summary>
//        [SerializeField]
//        string fileName;

//        /// <summary>
//        /// 地图存档数据路径;
//        /// </summary>
//        [SerializeField]
//        string archiveFileName;

//        /// <summary>
//        /// 地图数据路径;
//        /// </summary>
//        public string FileName
//        {
//            get { return fileName; }
//        }

//        /// <summary>
//        /// 地图存档数据路径;
//        /// </summary>
//        public string ArchiveFileName
//        {
//            get { return archiveFileName; }
//        }

//        /// <summary>
//        /// 获取到完整的文件存储路径;
//        /// </summary>
//        public string GetFileFullPath()
//        {
//            return Path.Combine(Resource.ConfigDirectoryPath, FileName);
//        }

//        /// <summary>
//        /// 获取到完整的文件存档路径;
//        /// </summary>
//        public string GetArchiveFileFullPath(Archive archive)
//        {
//            return Path.Combine(archive.Directory, archiveFileName);
//        }

//        /// <summary>
//        /// 是否存在文件;
//        /// </summary>
//        public bool ExistsFile()
//        {
//            string path = GetFileFullPath();
//            return File.Exists(path);
//        }

//        /// <summary>
//        /// 是否存在存档文件;
//        /// </summary>
//        public bool ExistsArchive(Archive archive)
//        {
//            string path = GetArchiveFileFullPath(archive);
//            return File.Exists(path);
//        }
//    }
//}
