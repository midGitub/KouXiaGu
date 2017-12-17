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

        private const bool DefaultIsCoreContent = false;

        /// <summary>
        /// 创建可读内容,若目录已经存在则返回异常;
        /// </summary>
        public LoadableContent CreateNew(string directory, LoadableContentDescription description, bool isCore = DefaultIsCoreContent)
        {
            if (Directory.Exists(directory))
                throw new IOException(string.Format("目录[{0}]已经存在;", directory));

            Directory.CreateDirectory(directory);
            ContentDirectory contentDirectory = new ContentDirectory(directory);
            return CreateNew(contentDirectory, description, isCore);
        }

        /// <summary>
        /// 创建可读内容,若文件已经存在则返回异常;
        /// </summary>
        public LoadableContent CreateNewZip(string file, LoadableContentDescription description, bool isCore = DefaultIsCoreContent)
        {
            Stream stream = new FileStream(file, FileMode.CreateNew, FileAccess.ReadWrite);
            ZipFile zipFile = ZipFile.Create(stream);
            ContentZip contentZip = new ContentZip(file, stream, zipFile);
            return CreateNew(contentZip, description, isCore);
        }

        /// <summary>
        /// 创建一个新的可读内容类型;
        /// </summary>
        public LoadableContent CreateNew(Content content, LoadableContentDescription description, bool isCore = DefaultIsCoreContent)
        {
            WriteDescription(content, description);
            LoadableContent loadableDirectory = new LoadableContent(new ConcurrentContent(content), description);
            loadableDirectory.IsCoreContent = isCore;
            return loadableDirectory;
        }

        /// <summary>
        /// 读取内容,若目录不存在,或者不是定义的可读内容则返回异常;
        /// </summary>
        public LoadableContent Read(string directory, bool isCore = false)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            ContentDirectory contentDirectory = new ContentDirectory(directory);
            return Read(contentDirectory, isCore);
        }

        /// <summary>
        /// 读取内容,若文件不存在,或者不是定义的可读内容则返回异常;
        /// </summary>
        public LoadableContent ReadZip(string file, bool isCore = false)
        {
            Stream stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
            ZipFile zipFile = new ZipFile(stream);
            ContentZip contentZip = new ContentZip(file, stream, zipFile);
            return Read(contentZip, isCore);
        }

        public LoadableContent Read(Content content, bool isCore = false)
        {
            LoadableContentDescription description = ReadDescription(content);
            LoadableContent loadableContent = new LoadableContent(new ConcurrentContent(content), description);
            loadableContent.IsCoreContent = isCore;
            return loadableContent;
        }

        /// <summary>
        /// 从内容读取到描述,并且更新实例;
        /// </summary>
        public void UpdateDescription(LoadableContent loadableContent)
        {
            LoadableContentDescription description = ReadDescription(loadableContent.Content);
            loadableContent.Description = description;
        }




        private const string DescriptionFileName = "ContentDescription";
        private readonly XmlSerializer<LoadableContentDescription> descriptionSerializer = new XmlSerializer<LoadableContentDescription>();
        private string descriptionPath;

        private string DescriptionPath
        {
            get { return descriptionPath ?? (descriptionPath = DescriptionFileName + descriptionSerializer.FileExtension); }
        }

        /// <summary>
        /// 输出新的描述到;
        /// </summary>
        private void WriteDescription(Content content, LoadableContentDescription description)
        {
            using (content.BeginUpdate())
            {
                InternalWriteDescription(content, description);
            }
        }

        /// <summary>
        /// 读取到描述;
        /// </summary>
        private LoadableContentDescription ReadDescription(Content content)
        {
            using (var stream = content.GetInputStream(DescriptionPath))
            {
                var descr = descriptionSerializer.Deserialize(stream);
                return descr;
            }
        }

        /// <summary>
        /// 输出新的描述到;
        /// </summary>
        private void InternalWriteDescription(Content content, LoadableContentDescription description)
        {
            using (var stream = content.CreateOutStream(DescriptionPath))
            {
                descriptionSerializer.Serialize(stream, description);
            }
        }
    }
}
