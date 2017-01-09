using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 存档文件;
    /// </summary>
    public class Archive
    {

        /// <summary>
        /// 存档保存到的文件夹;
        /// </summary>
        const string ARCHIVES_DIRECTORY_NAME = "Saves";
        const string DESCR_FILE_NAME = "Archive.xml";


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
        public static Archive Create(ArchiveDescr description)
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
        /// 创建一个新的存档;
        /// </summary>
        public static Archive Create(string directory, ArchiveDescr description)
        {
            Archive archive = new Archive(directory, description);
            WriteDescription(directory, description);
            return archive;
        }

        /// <summary>
        /// 创建描述文件到目录下;
        /// </summary>
        static void WriteDescription(string directory, ArchiveDescr description)
        {
            Directory.CreateDirectory(directory);
            string descriptionFilePath = GetDescriptionFilePath(directory);
            ArchiveDescr.Serializer.SerializeXiaGu(descriptionFilePath, description);
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
        public static IEnumerable<Archive> All()
        {
            var directorys = Directory.GetDirectories(ArchivesDirectory);
            return Find(directorys);
        }

        /// <summary>
        /// 获取到这些目录中为存档的;
        /// </summary>
        public static IEnumerable<Archive> Find(IEnumerable<string> directorys)
        {
            Archive item;
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
        public static bool TryLoad(string directory, out Archive item)
        {
            if (Exists(directory))
            {
                item = Load(directory);
                return true;
            }

            item = default(Archive);
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


        public static Archive Load(string directory)
        {
            ArchiveDescr description = ReadDescription(directory);
            return new Archive(directory, description);
        }

        /// <summary>
        /// 从目录下读取描述文件;
        /// </summary>
        static ArchiveDescr ReadDescription(string directory)
        {
            string descriptionFilePath = GetDescriptionFilePath(directory);
            ArchiveDescr description = (ArchiveDescr)ArchiveDescr.Serializer.DeserializeXiaGu(descriptionFilePath);
            return description;
        }


        Archive(string directory, ArchiveDescr description)
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
        public ArchiveDescr Description { get; private set; }

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
        public void UpdateDescription(ArchiveDescr description)
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

    }

}
