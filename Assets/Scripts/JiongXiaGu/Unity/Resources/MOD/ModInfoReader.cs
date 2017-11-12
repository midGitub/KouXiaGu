using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组信息读写器;
    /// </summary>
    public class ModInfoReader
    {
        private const string DescriptionFileName = "Description";
        private readonly XmlSerializer<ModDescription> xmlSerializer;

        public ModInfoReader()
        {
            xmlSerializer = new XmlSerializer<ModDescription>();
        }

        /// <summary>
        /// 创建一个新的模组到指定目录,若该目录已经存在,则返回异常;
        /// </summary>
        public ModInfo Create(DirectoryInfo directoryInfo, ModDescription description)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException(nameof(directoryInfo));
            if (directoryInfo.Exists)
                throw new IOException(string.Format("目录 {0} 已经存在", directoryInfo.FullName));

            directoryInfo.Create();
            WriteDescription(directoryInfo.FullName, description);
            ModInfo info = new ModInfo(directoryInfo, description);
            return info;
        }

        /// <summary>
        /// 将模组描述输出磁盘或更新磁盘的内容;
        /// </summary>
        public string WriteDescription(string directory, ModDescription description)
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
        public ModInfo Read(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException(nameof(directoryInfo));
            directoryInfo.ThrowIfDirectoryNotExisted();

            ModDescription description = ReadDescription(directoryInfo.FullName);
            ModInfo info = new ModInfo(directoryInfo, description);
            return info;
        }

        /// <summary>
        /// 读取描述文件;
        /// </summary>
        public ModDescription ReadDescription(string directory)
        {
            string descriptionFilePath = GetDescriptionFilePath(directory);
            ModDescription description = xmlSerializer.Read(descriptionFilePath);
            return description;
        }

        /// <summary>
        /// 枚举目录下的所有模组;
        /// </summary>
        public IEnumerable<State<ModInfo>> EnumerateModInfos(string directory)
        {
            foreach (var item in Directory.EnumerateDirectories(directory))
            {

            }
            throw new NotImplementedException();
        }
    }
}
