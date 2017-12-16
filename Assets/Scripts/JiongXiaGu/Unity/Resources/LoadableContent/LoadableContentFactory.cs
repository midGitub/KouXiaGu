using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{


    public class LoadableContentFactory
    {

        /// <summary>
        /// 创建可读内容,若目录已经存在则返回异常;
        /// </summary>
        public LoadableDirectory CreateNew(string directory, LoadableContentDescription description, bool isCore = false)
        {
            if (Directory.Exists(directory))
                throw new IOException(string.Format("目录[{0}]已经存在;", directory));

            Directory.CreateDirectory(directory);
            LoadableDirectory loadableDirectory = new LoadableDirectory(directory, description);
            loadableDirectory.InternaltBeginUpdate();
            InternalWriteDescription(loadableDirectory, description);
            loadableDirectory.InternaltCommitUpdate();
            loadableDirectory.IsCoreContent = isCore;
            return loadableDirectory;
        }

        /// <summary>
        /// 创建可读内容,若文件已经存在则返回异常;
        /// </summary>
        public LoadableZip CreateNewZip(string file, LoadableContentDescription description, bool isCore = false)
        {
            Stream stream = new FileStream(file, FileMode.CreateNew, FileAccess.ReadWrite);
            ZipFile zipFile = ZipFile.Create(stream);
            LoadableZip loadableZip = new LoadableZip(file, stream, zipFile, description);
            loadableZip.InternaltBeginUpdate();
            InternalWriteDescription(loadableZip, description);
            loadableZip.InternaltCommitUpdate();
            loadableZip.IsCoreContent = isCore;
            return loadableZip;
        }

        /// <summary>
        /// 读取内容,若目录不存在,或者不是定义的可读内容则返回异常;
        /// </summary>
        public LoadableDirectory Read(string directory, bool isCore = false)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            LoadableContentDescription description = InternalReadDescriptionFromDirectory(directory);
            LoadableDirectory loadableDirectory = new LoadableDirectory(directory, description);
            loadableDirectory.IsCoreContent = isCore;
            return loadableDirectory;
        }

        /// <summary>
        /// 读取内容,若文件不存在,或者不是定义的可读内容则返回异常;
        /// </summary>
        public LoadableZip ReadZip(string file, bool isCore = false)
        {
            Stream stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
            ZipFile zipFile = new ZipFile(stream);
            LoadableContentDescription description = InternalReadDescriptionFromZip(zipFile);
            LoadableZip loadableZip = new LoadableZip(file, stream, zipFile, description);
            loadableZip.IsCoreContent = isCore;
            return loadableZip;
        }

        /// <summary>
        /// 从内容读取到描述,并且更新实例;
        /// </summary>
        public void UpdateDescription(LoadableContent content)
        {
            LoadableContentDescription description = InternalReadDescription(content);
            content.NewDescription = description;
        }




        private const string DescriptionFileName = "LoadableContentDescr";
        private readonly XmlSerializer<LoadableContentDescription> descriptionSerializer = new XmlSerializer<LoadableContentDescription>();
        private string descriptionPath;

        private string DescriptionPath
        {
            get { return descriptionPath ?? (descriptionPath = DescriptionFileName + descriptionSerializer.FileExtension); }
        }

        /// <summary>
        /// 输出新的描述到实例;
        /// </summary>
        internal void InternalWriteDescription(LoadableContent content, LoadableContentDescription description)
        {
            using (var stream = content.InternaltCreateOutStream(DescriptionPath))
            {
                descriptionSerializer.Serialize(stream, description);
            }
        }

        internal LoadableContentDescription InternalReadDescriptionFromDirectory(string directory)
        {
            string filePath = Path.Combine(directory, DescriptionPath);
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var descr = descriptionSerializer.Deserialize(stream);
                return descr;
            }
        }

        internal LoadableContentDescription InternalReadDescriptionFromZip(Stream fStream)
        {
            using (var zipInputStream = new ZipInputStream(fStream))
            {
                zipInputStream.IsStreamOwner = false;
                ZipEntry entry;
                while ((entry = zipInputStream.GetNextEntry()) != null)
                {
                    if (entry.Name == DescriptionPath)
                    {
                        var descr = descriptionSerializer.Deserialize(zipInputStream);
                        return descr;
                    }
                }
                throw new FileNotFoundException(string.Format("未找到描述文件{0}", DescriptionPath));
            }
        }

        internal LoadableContentDescription InternalReadDescriptionFromZip(ZipFile zipFile)
        {
            ZipEntry entry = zipFile.GetEntry(DescriptionPath);
            if (entry != null)
            {
                using (var stream = zipFile.GetInputStream(entry))
                {
                    var descr = descriptionSerializer.Deserialize(stream);
                    return descr;
                }
            }
            else
            {
                throw new FileNotFoundException(string.Format("未找到描述文件{0}", DescriptionPath));
            }
        }

        /// <summary>
        /// 读取到描述;
        /// </summary>
        internal LoadableContentDescription InternalReadDescription(LoadableContent content)
        {
            using (var stream = content.GetInputStream(DescriptionPath))
            {
                var descr = descriptionSerializer.Deserialize(stream);
                return descr;
            }
        }
    }
}
