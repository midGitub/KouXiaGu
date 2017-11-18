using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可读取资源压缩包;
    /// </summary>
    public class LoadableZipFileReader
    {
        private const string DescriptionFileName = "Description";
        private readonly XmlSerializer<LoadableContentDescription> xmlSerializer;

        public LoadableZipFileReader()
        {
            xmlSerializer = new XmlSerializer<LoadableContentDescription>();
        }

        /// <summary>
        /// 获取到目录下的描述文件路径;
        /// </summary>
        public string GetDescriptionFileName()
        {
            string filePath = DescriptionFileName + xmlSerializer.FileExtension;
            return filePath;
        }

        /// <summary>
        /// 从该目录读取到存档信息,若不存在则返回异常;
        /// </summary>
        public LoadableContentInfo Create(ZipFile zipFile, LoadableContentType type)
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));

            LoadableContentDescription description = ReadDescription(zipFile);
            LoadableContentInfo info = new LoadableContentInfo(new LoadableZipFile(zipFile), description, type);
            return info;
        }

        /// <summary>
        /// 读取描述文件;
        /// </summary>
        public LoadableContentDescription ReadDescription(ZipFile zipFile)
        {
            string descriptionFileName = GetDescriptionFileName();
            ZipEntry zipEntry = zipFile.GetEntry(descriptionFileName);
            if (zipEntry != null)
            {
                var stream = zipFile.GetInputStream(zipEntry);
                LoadableContentDescription description = xmlSerializer.Deserialize(stream);
                return description;
            }
            else
            {
                throw new FileNotFoundException("未找到描述文件;");
            }
        }

        /// <summary>
        /// 将模组描述输出磁盘或更新磁盘的内容;
        /// </summary>
        public string WriteDescription(ZipFile zipFile, LoadableContentDescription description)
        {


            //string descriptionFilePath = GetDescriptionFilePath(directory);
            //xmlSerializer.Write(descriptionFilePath, description, FileMode.Create);
            //return descriptionFilePath;
        }
    }
}
