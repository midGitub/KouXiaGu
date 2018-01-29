using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{


    public class ModificationFactory
    {
        /// <summary>
        /// 创建可读内容,若目录已经存在则返回异常;
        /// </summary>
        public ModificationContent CreateNew(string directory, ModificationDescription description)
        {
            Content content = new DirectoryContent(directory);
            return CreateNew(content, description);
        }

        /// <summary>
        /// 创建可读内容,若文件已经存在则返回异常;
        /// </summary>
        public ModificationContent CreateNewZip(string file, ModificationDescription description)
        {
            Stream stream = new FileStream(file, FileMode.CreateNew, FileAccess.ReadWrite);
            ZipFile zipFile = ZipFile.Create(stream);
            ZipContent contentZip = new ZipContent(file, stream, zipFile);
            return CreateNew(contentZip, description);
        }

        /// <summary>
        /// 创建一个新的可读内容类型;
        /// </summary>
        public ModificationContent CreateNew(Content content, ModificationDescription description)
        {
            WriteDescription(content, description);
            ModificationContent loadableDirectory = new ModificationContent(content, description);
            return loadableDirectory;
        }


        /// <summary>
        /// 读取内容,若目录不存在,或者不是定义的可读内容则返回异常;
        /// </summary>
        public ModificationContent Read(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            Content content = new DirectoryContent(directory);
            return Read(content);
        }

        /// <summary>
        /// 读取内容,若文件不存在,或者不是定义的可读内容则返回异常;
        /// </summary>
        public ModificationContent ReadZip(string file)
        {
            Content content = new ZipContent(file);
            return Read(content);
        }

        /// <summary>
        /// 创建为可读内容,若未能创建则返回异常;
        /// </summary>
        public ModificationContent Read(Content content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            ModificationDescription description = ReadDescription(content);
            ModificationContent loadableContent = new ModificationContent(content, description);
            return loadableContent;
        }

        public ModificationContent Read(ModificationInfo info)
        {
            if (info.ContentInfo == null)
                throw new ArgumentNullException(nameof(info.ContentInfo));

            Content content = info.ContentInfo.GetContent();
            return Read(content);
        }



        public ModificationInfo ReadInfo(string directory)
        {
            DirectoryContentInfo info = new DirectoryContentInfo(directory);
            return ReadInfo(info);
        }

        public ModificationInfo ReadZipInfo(string file)
        {
            ZipContentInfo info = new ZipContentInfo(file);
            return ReadInfo(info);
        }

        public ModificationInfo ReadInfo(IContentInfo contentInfo)
        {
            if (contentInfo == null)
                throw new ArgumentNullException(nameof(contentInfo));

            using (var content = contentInfo.GetContent())
            {
                ModificationDescription description = ReadDescription(content);
                ModificationInfo info = new ModificationInfo(contentInfo, description);
                return info;
            }
        }



        /// <summary>
        /// 从内容读取到描述,并且更新实例;
        /// </summary>
        public void UpdateDescription(ModificationContent loadableContent)
        {
            lock (loadableContent.AsyncLock)
            {
                ModificationDescription description = ReadDescription(loadableContent);
                loadableContent.Description = description;
            }
        }

        /// <summary>
        /// 写入资源描述;
        /// </summary>
        public void UpdateDescription(ModificationContent loadableContent, ModificationDescription description)
        {
            lock (loadableContent.AsyncLock)
            {
                WriteDescription(loadableContent, description);
                loadableContent.Description = description;
            }
        }



        private const string DescriptionFileName = "ModDescription";
        private readonly XmlSerializer<ModificationDescription> descriptionSerializer = new XmlSerializer<ModificationDescription>();
        private string descriptionPath;

        private string DescriptionPath
        {
            get { return descriptionPath ?? (descriptionPath = DescriptionFileName + descriptionSerializer.FileExtension); }
        }

        /// <summary>
        /// 输出新的描述到;
        /// </summary>
        private void WriteDescription(Content content, ModificationDescription description)
        {
            using (content.BeginUpdate())
            {
                using (var stream = content.GetOutputStream(DescriptionPath))
                {
                    descriptionSerializer.Serialize(stream, description);
                }
            }
        }

        /// <summary>
        /// 读取到描述;
        /// </summary>
        private ModificationDescription ReadDescription(Content content)
        {
            using (var stream = content.GetInputStream(DescriptionPath))
            {
                var descr = descriptionSerializer.Deserialize(stream);
                return descr;
            }
        }
    }
}
