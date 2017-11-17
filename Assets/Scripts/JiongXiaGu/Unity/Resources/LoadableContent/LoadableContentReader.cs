using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可加载资源信息读写器;
    /// </summary>
    public class LoadableContentReader
    {
        private const string DescriptionFileName = "Description";
        private readonly XmlSerializer<LoadableContentDescription> xmlSerializer;

        public LoadableContentReader()
        {
            xmlSerializer = new XmlSerializer<LoadableContentDescription>();
        }

        /// <summary>
        /// 创建一个新的模组到指定目录,若该目录已经存在,则返回异常;
        /// </summary>
        public LoadableContentInfo Create(DirectoryInfo directoryInfo, LoadableContentDescription description, LoadableContentType type)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException(nameof(directoryInfo));
            if (directoryInfo.Exists)
                throw new IOException(string.Format("目录 {0} 已经存在", directoryInfo.FullName));

            directoryInfo.Create();
            WriteDescription(directoryInfo.FullName, description);
            LoadableContentInfo info = new LoadableContentInfo(new LoadableDirectory(directoryInfo), description, type);
            return info;
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
        /// 获取到目录下的描述文件路径
        /// </summary>
        public string GetDescriptionFilePath(string directory) 
        {
            string filePath = Path.Combine(directory, DescriptionFileName);
            filePath = Path.ChangeExtension(filePath, xmlSerializer.FileExtension);
            return filePath;
        }

        /// <summary>
        /// 从该目录读取到存档信息,若不存在则返回异常;
        /// </summary>
        public LoadableContentInfo Read(DirectoryInfo directoryInfo, LoadableContentType type)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException(nameof(directoryInfo));
            directoryInfo.ThrowIfDirectoryNotExisted();

            LoadableContentDescription description = ReadDescription(directoryInfo.FullName);
            LoadableContentInfo info = new LoadableContentInfo(new LoadableDirectory(directoryInfo), description, type);
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
        /// 枚举目录下的所有模组;
        /// </summary>
        /// <param name="modsDirectory">目标目录</param>
        /// <param name="type">指定找到的模组类型</param>
        /// <returns></returns>
        public IEnumerable<LoadableContentInfo> EnumerateModInfos(string modsDirectory, LoadableContentType type)
        {
            foreach (var directory in Directory.EnumerateDirectories(modsDirectory))
            {
                LoadableContentInfo modInfo = null;
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                    modInfo = Read(directoryInfo, type);
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
