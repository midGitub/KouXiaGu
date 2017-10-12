using JiongXiaGu.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 对游戏资源文件路径进行定义,需要手动初始化;
    /// </summary>
    [ConsoleMethodsClass]
    public static class Resource
    {
        private static bool isInitialized = false;
        private static readonly ExpandedDataSearcher expandedDataSearcher = new ExpandedDataSearcher();
        private static readonly DataOrderRecordReader dataOrderRecordReader = new DataOrderRecordReader();

        /// <summary>
        /// 存放数据和配置文件的文件夹;
        /// </summary>
        public static DataDirectoryInfo CoreDataDirectoryInfo { get; private set; }

        /// <summary>
        /// 用于存放用户配置文件的文件夹;
        /// </summary>
        public static DirectoryInfo UserConfigDirectoryInfo { get; private set; }

        /// <summary>
        /// 存放存档的文件夹路径;
        /// </summary>
        public static DirectoryInfo ArchivesDirectoryInfo { get; private set; }

        /// <summary>
        /// 所有的游戏拓展数据目录信息;
        /// </summary>
        private static List<DataDirectoryInfo> expandedDataDirectoryInfos;

        /// <summary>
        /// 按优先顺序排列的游戏数据目录信息;
        /// </summary>
        private static List<DataDirectoryInfo> orderedDataDirectoryInfos;

        /// <summary>
        /// 在游戏开始时初始化;
        /// </summary>
        internal static Task Initialize()
        {
            if (!isInitialized)
            {
                CoreDataDirectoryInfo = GetDataDirectoryInfo();
                UserConfigDirectoryInfo = CreateUserConfigDirectoryInfo();
                ArchivesDirectoryInfo = CreateArchivesDirectoryInfo();
                expandedDataDirectoryInfos = SearchExpandedDataDirectoryInfos();
                orderedDataDirectoryInfos = GetOrderedDataDirectoryInfos();
                isInitialized = true;
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 主要数据和配置文件的文件夹;
        /// </summary>
        public static string CoreDataDirectory
        {
            get { return CoreDataDirectoryInfo.DirectoryInfo.FullName; }
        }

        /// <summary>
        /// 用于存放用户配置文件的文件夹;
        /// </summary>
        public static string UserConfigDirectory
        {
            get { return UserConfigDirectoryInfo.FullName; }
        }

        /// <summary>
        /// 存放存档的文件夹路径;
        /// </summary>
        public static string ArchivesDirectory
        {
            get { return ArchivesDirectoryInfo.FullName; }
        }

        /// <summary>
        /// 所有的游戏拓展数据目录信息;
        /// </summary>
        public static IEnumerable<DataDirectoryInfo> ExpandedDataDirectoryInfos
        {
            get { return expandedDataDirectoryInfos; }
        }

        private static DataDirectoryInfo GetDataDirectoryInfo()
        {
            string directory = Application.streamingAssetsPath;
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.ThrowIfDirectoryNotExisted();
            return new DataDirectoryInfo(directoryInfo, true);
        }

        private static DirectoryInfo CreateUserConfigDirectoryInfo()
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", Application.productName);
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            return directoryInfo;
        }

        private static DirectoryInfo CreateArchivesDirectoryInfo()
        {
            string directory = Path.Combine(UserConfigDirectory, "Saves");
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            return directoryInfo;
        }

        private static List<DataDirectoryInfo> SearchExpandedDataDirectoryInfos()
        {
            var expandedDataDirectoryInfos = expandedDataSearcher.Search();
            return expandedDataDirectoryInfos;
        }

        private static List<DataDirectoryInfo> GetOrderedDataDirectoryInfos()
        {
            try
            {
                DataOrderRecord dataOrderRecord = dataOrderRecordReader.Read();
                var orderedDataDirectoryInfos = dataOrderRecord.Sort(expandedDataDirectoryInfos);
                return orderedDataDirectoryInfos;
            }
            catch (Exception ex)
            {
                Debug.LogWarning(string.Format("读取资源排列顺序文件失败:{0}", ex));
                return new List<DataDirectoryInfo>();
            }
        }

        /// <summary>
        /// 设置数据排列顺序;
        /// </summary>
        public static void SetDataOrder(IEnumerable<DataDirectoryInfo> orderedDataDirectoryInfos)
        {
            DataOrderRecord dataOrderRecord = new DataOrderRecord(orderedDataDirectoryInfos);
            Resource.orderedDataDirectoryInfos = new List<DataDirectoryInfo>(orderedDataDirectoryInfos);
            dataOrderRecordReader.Write(dataOrderRecord);
        }

        /// <summary>
        /// 设置数据排列顺序;
        /// </summary>
        public static void SetDataOrder(DataOrderRecord dataOrderRecord)
        {
            orderedDataDirectoryInfos = dataOrderRecord.Sort(expandedDataDirectoryInfos);
            dataOrderRecordReader.Write(dataOrderRecord);
        }

        /// <summary>
        /// 根据优先顺序返回所有数据目录;
        /// </summary>
        public static IEnumerable<DataDirectoryInfo> GetDataDirectoryInfos()
        {
            yield return CoreDataDirectoryInfo;
            foreach (var dataDirectoryInfo in orderedDataDirectoryInfos)
            {
                yield return dataDirectoryInfo;
            }
        }

        /// <summary>
        /// 根据优先顺序返回所有数据目录(不包括核心资源信息);
        /// </summary>
        public static IEnumerable<DataDirectoryInfo> GetDataDirectoryInfosWithoutCore()
        {
            return orderedDataDirectoryInfos;
        }

        [ConsoleMethod("log_resource_path_info", "显示所有资源路径;", IsDeveloperMethod = true)]
        public static void LogInfoAll()
        {
            string str =
                "\nDataDirectoryPath : " + CoreDataDirectory +
                "\nUserConfigDirectoryPath" + UserConfigDirectory +
                "\nArchiveDirectoryPath : " + ArchivesDirectory;
            Debug.Log(str);
        }
    }
}
