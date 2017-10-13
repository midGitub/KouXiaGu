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
        private static readonly ModSearcher modSearcher = new ModSearcher();
        private static readonly ModOrderRecordReader modOrderRecordReader = new ModOrderRecordReader();

        /// <summary>
        /// 核心数据和配置文件的文件夹;
        /// </summary>
        public static ModInfo CoreDirectoryInfo { get; private set; }

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
        private static List<ModInfo> modInfos;

        /// <summary>
        /// 按优先顺序排列的游戏数据目录信息;
        /// </summary>
        private static List<ModInfo> orderedModInfos;

        /// <summary>
        /// 在游戏开始时初始化;
        /// </summary>
        internal static Task Initialize()
        {
            if (!isInitialized)
            {
                CoreDirectoryInfo = ReadCoreDirectoryInfo();
                UserConfigDirectoryInfo = ReadUserConfigDirectoryInfo();
                ArchivesDirectoryInfo = ReadArchivesDirectoryInfo();
                modInfos = SearchModDirectoryInfos();
                orderedModInfos = GetOrderedModDirectoryInfos();
                isInitialized = true;
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 主要数据和配置文件的文件夹;
        /// </summary>
        public static string CoreDirectory
        {
            get { return CoreDirectoryInfo.DirectoryInfo.FullName; }
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
        public static IEnumerable<ModInfo> ModDirectoryInfos
        {
            get { return modInfos; }
        }

        /// <summary>
        /// 获取到核心数据和配置文件的文件夹;
        /// </summary>
        public static ModInfo ReadCoreDirectoryInfo()
        {
            string directory = Application.streamingAssetsPath;
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.ThrowIfDirectoryNotExisted();
            var dataDirectoryInfo = new ModInfo(directoryInfo);
            return dataDirectoryInfo;
        }

        public static DirectoryInfo ReadUserConfigDirectoryInfo()
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", Application.productName);
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            return directoryInfo;
        }

        public static DirectoryInfo ReadArchivesDirectoryInfo()
        {
            var userConfigDirectory = UserConfigDirectory;
            if (userConfigDirectory == null)
            {
                userConfigDirectory = ReadUserConfigDirectoryInfo().FullName;
            }

            string directory = Path.Combine(userConfigDirectory, "Saves");
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            return directoryInfo;
        }

        internal static List<ModInfo> SearchModDirectoryInfos()
        {
            var expandedDataDirectoryInfos = modSearcher.Search();
            return expandedDataDirectoryInfos;
        }

        internal static List<ModInfo> GetOrderedModDirectoryInfos()
        {
            try
            {
                ModOrderRecord dataOrderRecord = modOrderRecordReader.Read();
                var orderedDataDirectoryInfos = dataOrderRecord.Sort(modInfos);
                return orderedDataDirectoryInfos;
            }
            catch (Exception ex)
            {
                Debug.LogWarning(string.Format("读取资源排列顺序文件失败:{0}", ex));
                return new List<ModInfo>();
            }
        }

        /// <summary>
        /// 设置数据排列顺序;
        /// </summary>
        public static void UpdateModOrder(IEnumerable<ModInfo> orderedModDirectoryInfos)
        {
            ModOrderRecord dataOrderRecord = new ModOrderRecord(orderedModDirectoryInfos);
            Resource.orderedModInfos = new List<ModInfo>(orderedModDirectoryInfos);
            modOrderRecordReader.Write(dataOrderRecord);
        }

        /// <summary>
        /// 设置数据排列顺序;
        /// </summary>
        public static void UpdateModOrder(ModOrderRecord modOrderRecord)
        {
            orderedModInfos = modOrderRecord.Sort(modInfos);
            modOrderRecordReader.Write(modOrderRecord);
        }

        /// <summary>
        /// 根据优先顺序返回所有数据目录;
        /// </summary>
        public static IEnumerable<ModInfo> GetDataDirectoryInfos()
        {
            yield return CoreDirectoryInfo;
            foreach (var dataDirectoryInfo in orderedModInfos)
            {
                yield return dataDirectoryInfo;
            }
        }

        /// <summary>
        /// 根据优先顺序返回所有数据目录(不包括核心资源信息);
        /// </summary>
        public static IEnumerable<ModInfo> GetDataDirectoryInfosWithoutCore()
        {
            return orderedModInfos;
        }

        [ConsoleMethod("log_resource_path_info", "显示所有资源路径;", IsDeveloperMethod = true)]
        public static void LogInfoAll()
        {
            string str =
                "\nDataDirectoryPath : " + CoreDirectory +
                "\nUserConfigDirectoryPath" + UserConfigDirectory +
                "\nArchiveDirectoryPath : " + ArchivesDirectory;
            Debug.Log(str);
        }
    }
}
