using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 存档文件;
    /// </summary>
    public class ArchiveFile
    {

        /// <summary>
        /// 存档保存到的文件夹;
        /// </summary>
        const string ARCHIVES_DIRECTORY_NAME = "Saves";
        const string DESCR_FILE_NAME = "Archive";


        /// <summary>
        /// 获取到保存所有存档的文件夹路径;
        /// </summary>
        static string ArchivesDirectory
        {
            get { return Path.Combine(Application.persistentDataPath, ARCHIVES_DIRECTORY_NAME); }
        }


        /// <summary>
        /// 在存档目录下创建一个新的存档;
        /// </summary>
        public static ArchiveFile Create(ArchiveDescription description)
        {
            string archivePath = GetRandomArchiveDirectory();
            return Create(archivePath, description);
        }

        /// <summary>
        /// 获取到一个随机的存档文件夹目录;
        /// </summary>
        static string GetRandomArchiveDirectory()
        {
            string name = Path.GetRandomFileName();
            return Path.Combine(ArchivesDirectory, name);
        }

        /// <summary>
        /// 创建一个新的存档,到文件夹;
        /// </summary>
        public static ArchiveFile Create(string directory, ArchiveDescription description)
        {
            Directory.CreateDirectory(directory);
            ArchiveFile archive = new ArchiveFile(directory, description);
            WriteDescription(directory, description);
            return archive;
        }

        /// <summary>
        /// 创建描述文件到目录下;
        /// </summary>
        static void WriteDescription(string directory, ArchiveDescription description)
        {
            string descriptionFilePath = GetDescriptionFilePath(directory);
            ArchiveDescription.Write(descriptionFilePath, description);
        }

        /// <summary>
        /// 获取到目录下的描述文件;
        /// </summary>
        static string GetDescriptionFilePath(string directory)
        {
            return Path.Combine(directory, DESCR_FILE_NAME);
        }


        /// <summary>
        /// 获取到存档目录下的所有存档;
        /// </summary>
        public static IEnumerable<ArchiveFile> All()
        {
            var directorys = Directory.GetDirectories(ArchivesDirectory);
            return Find(directorys);
        }

        /// <summary>
        /// 获取到这些目录中为存档的;
        /// </summary>
        public static IEnumerable<ArchiveFile> Find(IEnumerable<string> directorys)
        {
            ArchiveFile item;
            foreach (var directory in directorys)
            {
                if (TryLoad(directory,out item))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// 尝试读取此路径下的存档;
        /// </summary>
        public static bool TryLoad(string directory, out ArchiveFile item)
        {
            if (Exists(directory))
            {
                item = Read(directory);
                return true;
            }

            item = default(ArchiveFile);
            return false;
        }

        /// <summary>
        /// 确认此目录是否为存档;
        /// </summary>
        public static bool Exists(string directory)
        {
            string descriptionFilePath = GetDescriptionFilePath(directory);
            return File.Exists(descriptionFilePath);
        }

        /// <summary>
        /// 读取目录下的存档信息;
        /// </summary>
        public static ArchiveFile Read(string directory)
        {
            ArchiveDescription description = ReadDescription(directory);
            return new ArchiveFile(directory, description);
        }

        /// <summary>
        /// 从目录下读取描述文件;
        /// </summary>
        static ArchiveDescription ReadDescription(string directory)
        {
            string descriptionFilePath = GetDescriptionFilePath(directory);
            var description = ArchiveDescription.Read(descriptionFilePath);
            return description;
        }



        ArchiveFile(string directory, ArchiveDescription description)
        {
            this.DirectoryPath = directory;
            this.Description = description;
            IsEffective = true;
        }

        /// <summary>
        /// 是否为有效的存档?
        /// </summary>
        public bool IsEffective { get; private set; }

        /// <summary>
        /// 存档文件夹路径;
        /// </summary>
        public string DirectoryPath { get; private set; }

        /// <summary>
        /// 描述信息;
        /// </summary>
        public ArchiveDescription Description { get; private set; }

        /// <summary>
        /// 是否允许编辑?
        /// </summary>
        public bool AllowEdit
        {
            get { return Description.AllowEdit; }
        }

        /// <summary>
        /// 更新描述文件;
        /// </summary>
        public void UpdateDescription(ArchiveDescription description)
        {
            WriteDescription(DirectoryPath, description);
            this.Description = description;
        }

        /// <summary>
        /// 销毁存档;
        /// </summary>
        public void Destroy()
        {
            if (!AllowEdit)
                throw new UnauthorizedAccessException("不允许编辑的存档内容;");

            Directory.Delete(DirectoryPath, true);
            IsEffective = false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ArchiveFile))
                return false;
            return ((ArchiveFile)obj).DirectoryPath == DirectoryPath;
        }

        public override int GetHashCode()
        {
            return DirectoryPath.GetHashCode();
        }

        public override string ToString()
        {
            return "[" + ",Path:" + DirectoryPath + ",Description:" + Description.ToString() + "]";
        }

    }

}
