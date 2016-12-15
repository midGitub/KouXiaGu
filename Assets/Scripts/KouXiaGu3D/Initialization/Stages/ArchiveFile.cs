using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 游戏存档管理;
    /// </summary>
    public class ArchiveFile
    {

        /// <summary>
        /// 存档保存到的文件夹;
        /// </summary>
        const string ARCHIVES_DIRECTORY_NAME = "Saves";

        /// <summary>
        /// 临时存档文件夹名,先将存档保存到此路径,保存完毕后再复制到真正的存档目录;
        /// </summary>
        const string TEMP_ARCHIVES_DIRECTORY_NAME = "Saves_Temp";

        /// <summary>
        /// 获取到保存所有存档的文件夹路径;
        /// </summary>
        public static string ArchivesPath
        {
            get { return Path.Combine(Application.persistentDataPath, ARCHIVES_DIRECTORY_NAME); }
        }

        /// <summary>
        /// 临时存档文件夹名,先将存档保存到此路径,保存完毕后再复制到真正的存档目录;
        /// </summary>
        public static string TempDirectoryPath
        {
            get { return Path.Combine(Application.temporaryCachePath, TEMP_ARCHIVES_DIRECTORY_NAME); }
        }

        /// <summary>
        /// 获取到所有存档文件夹下的所有文件夹路径;
        /// </summary>
        static IEnumerable<string> GetArchivedPaths()
        {
            string[] archivedsPath = Directory.GetDirectories(ArchivesPath);
            return archivedsPath;
        }

        /// <summary>
        /// 获取到一个随机的文件名;
        /// </summary>
        static string GetRandomDirectoryName()
        {
            return Path.GetRandomFileName();
        }

        /// <summary>
        /// 获取到所有的存档路径;
        /// </summary>
        public static IEnumerable<ArchiveFile> GetArchives()
        {
            foreach (var path in GetArchivedPaths())
            {
                yield return new ArchiveFile(path);
            }
        }

        #region 实例部分;

        /// <summary>
        /// 存档文件夹路径;
        /// </summary>
        public string DirectoryPath { get; private set; }

        /// <summary>
        /// 创建一个新的存档;
        /// </summary>
        public ArchiveFile()
        {
            DirectoryPath = Path.Combine(ArchivesPath, GetRandomDirectoryName());
        }

        ArchiveFile(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
        }

        /// <summary>
        /// 创建存档目录到磁盘;
        /// </summary>
        public void Create()
        {
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);
        }

        /// <summary>
        /// 销毁磁盘上的存档内容;
        /// </summary>
        public void Destroy()
        {
            throw new System.Exception();
        }

        /// <summary>
        /// 当保存完成时调用;
        /// </summary>
        public void OnComplete()
        {
            Debug.Log("保存完成;路径:" + DirectoryPath);
            return;
        }

        #endregion

    }

}
