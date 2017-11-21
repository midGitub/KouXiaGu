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
    /// 读取资源压缩包;
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
        /// 读取模组信息,若不存在则返回异常;
        /// </summary>
        public LoadableZipFile Create(string filePath, LoadableContentType type)
        {
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            ZipFile zipFile = new ZipFile(stream);
            return Create(zipFile, type);
        }

        /// <summary>
        /// 读取模组信息,若不存在则返回异常;
        /// </summary>
        public LoadableZipFile Create(ZipFile zipFile, LoadableContentType type)
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));

            LoadableContentDescription description = ReadDescription(zipFile);
            LoadableZipFile info = new LoadableZipFile(zipFile, description, type);
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
                using (var stream = zipFile.GetInputStream(zipEntry))
                {
                    LoadableContentDescription description = xmlSerializer.Deserialize(stream);
                    return description;
                }
            }
            else
            {
                throw new FileNotFoundException("未找到描述文件;");
            }
        }

        /// <summary>
        /// 获取到目录下的描述文件路径;
        /// </summary>
        public string GetDescriptionFileName()
        {
            string filePath = DescriptionFileName + xmlSerializer.FileExtension;
            return filePath;
        }
    }
}
