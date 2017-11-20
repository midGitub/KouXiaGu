using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 读取资源目录;
    /// </summary>
    public class LoadableDirectoryReader
    {
        private const string DescriptionFileName = "Description";
        private readonly XmlSerializer<LoadableContentDescription> xmlSerializer;

        public LoadableDirectoryReader()
        {
            xmlSerializer = new XmlSerializer<LoadableContentDescription>();
        }

        /// <summary>
        /// 从该目录读取到存档信息,若不存在则返回异常;
        /// </summary>
        public LoadableDirectory Create(DirectoryInfo directoryInfo, LoadableContentType type)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException(nameof(directoryInfo));
            directoryInfo.ThrowIfDirectoryNotExisted();

            LoadableContentDescription description = ReadDescription(directoryInfo.FullName);
            LoadableDirectory info = new LoadableDirectory(directoryInfo.FullName, description, type);
            return info;
        }

        /// <summary>
        /// 读取描述文件;
        /// </summary>
        public LoadableContentDescription ReadDescription(string directory)
        {
            string descriptionFilePath = GetDescriptionFilePath(directory);
            LoadableContentDescription description = xmlSerializer.Read(descriptionFilePath);
            return description;
        }

        /// <summary>
        /// 获取到目录下的描述文件路径
        /// </summary>
        public string GetDescriptionFilePath(string directory)
        {
            string filePath = Path.Combine(directory, DescriptionFileName);
            filePath = Path.ChangeExtension(filePath, xmlSerializer.FileExtension);
            return filePath;
        }

        /// <summary>
        /// 将模组描述输出磁盘或更新磁盘的内容;
        /// </summary>
        public string WriteDescription(string directory, LoadableContentDescription description)
        {
            string descriptionFilePath = GetDescriptionFilePath(directory);
            xmlSerializer.Write(descriptionFilePath, description, FileMode.Create);
            return descriptionFilePath;
        }

        /// <summary>
        /// 枚举目录下的所有模组;
        /// </summary>
        /// <param name="modsDirectory">目标目录</param>
        /// <param name="type">指定找到的模组类型</param>
        /// <returns></returns>
        [Obsolete]
        public IEnumerable<LoadableContent> EnumerateModInfos(string modsDirectory, LoadableContentType type)
        {
            foreach (var directory in Directory.EnumerateDirectories(modsDirectory))
            {
                LoadableDirectory modInfo = null;
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                    modInfo = Create(directoryInfo, type);
                }
                catch (FileNotFoundException)
                {
                }

                if (modInfo != null)
                    yield return modInfo;
            }
        }
    }
}
