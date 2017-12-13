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
        public LoadableDirectory CreateNew(string directory, LoadableContentDescription description)
        {
            if (Directory.Exists(directory))
                throw new IOException(string.Format("目录[{0}]已经存在;", directory));

            Directory.CreateDirectory(directory);
            LoadableDirectory loadableDirectory = new LoadableDirectory(directory, description);
            loadableDirectory.BeginUpdate();
            WriteDescription(loadableDirectory, description);
            loadableDirectory.CommitUpdate();
            return loadableDirectory;
        }

        /// <summary>
        /// 创建可读内容,若文件已经存在则返回异常;
        /// </summary>
        public LoadableZip CreateNewZip(string file, LoadableContentDescription description)
        {
            Stream stream = new FileStream(file, FileMode.CreateNew, FileAccess.ReadWrite);
            ZipFile zipFile = ZipFile.Create(stream);
            LoadableZip loadableZip = new LoadableZip(file, stream, zipFile, description);
            loadableZip.BeginUpdate();
            WriteDescription(loadableZip, description);
            loadableZip.CommitUpdate();
            return loadableZip;
        }

        /// <summary>
        /// 读取内容,若目录不存在,或者不是定义的可读内容则返回异常;
        /// </summary>
        public LoadableDirectory Read(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            LoadableContentDescription description = ReadDescriptionFromDirectory(directory);
            LoadableDirectory loadableDirectory = new LoadableDirectory(directory, description);
            return loadableDirectory;
        }

        /// <summary>
        /// 读取内容,若文件不存在,或者不是定义的可读内容则返回异常;
        /// </summary>
        public LoadableZip ReadZip(string file)
        {
            Stream stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
            ZipFile zipFile = new ZipFile(stream);
            LoadableContentDescription description = ReadDescriptionFromZip(zipFile);
            LoadableZip loadableZip = new LoadableZip(file, stream, zipFile, description);
            return loadableZip;
        }


        private const string DescriptionFileName = "LoadableContentDescr";
        private readonly XmlSerializer<LoadableContentDescription> descriptionSerializer = new XmlSerializer<LoadableContentDescription>();
        private string descriptionPath;

        private string DescriptionPath
        {
            get { return descriptionPath ?? (descriptionPath = DescriptionFileName + descriptionSerializer.FileExtension); }
        }

        internal void WriteDescription(LoadableContent content, LoadableContentDescription description)
        {
            using (var stream = content.CreateOutStream(DescriptionPath))
            {
                descriptionSerializer.Serialize(stream, description);
            }
        }

        internal LoadableContentDescription ReadDescriptionFromDirectory(string directory)
        {
            string filePath = Path.Combine(directory, DescriptionPath);
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var descr = descriptionSerializer.Deserialize(stream);
                return descr;
            }
        }

        internal LoadableContentDescription ReadDescriptionFromZip(Stream fStream)
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
                throw new FileNotFoundException(string.Format("未找到描述文件{1}", DescriptionPath));
            }
        }

        internal LoadableContentDescription ReadDescriptionFromZip(ZipFile zipFile)
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
                throw new FileNotFoundException(string.Format("未找到描述文件{1}", DescriptionPath));
            }
        }

        internal LoadableContentDescription ReadDescription(LoadableContent content)
        {
            using (var stream = content.ConcurrentGetInputStream(DescriptionPath))
            {
                var descr = descriptionSerializer.Deserialize(stream);
                return descr;
            }
        }
    }
}
