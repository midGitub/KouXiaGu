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
        public LoadableContent CreateNew(string directory, LoadableContentDescription description)
        {
            if (Directory.Exists(directory))
                throw new IOException(string.Format("目录[{0}]已经存在;", directory));

            LoadableDirectory loadableDirectory = new LoadableDirectory(directory, description);
            loadableDirectory.BeginUpdate();
            WriteDescription(loadableDirectory, description);
            loadableDirectory.CommitUpdate();
            return loadableDirectory;
        }

        /// <summary>
        /// 创建可读内容,若文件已经存在则返回异常;
        /// </summary>
        public LoadableContent CreateNewZip(string file, LoadableContentDescription description)
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
        public LoadableContent Read(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            LoadableDirectory loadableDirectory = new LoadableDirectory(directory, default(LoadableContentDescription));
            loadableDirectory.Update(this);
            return loadableDirectory;
        }

        /// <summary>
        /// 读取内容,若文件不存在,或者不是定义的可读内容则返回异常;
        /// </summary>
        public LoadableContent ReadZip(string file)
        {
            Stream stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
            ZipFile zipFile = new ZipFile(stream);
            LoadableZip loadableZip = new LoadableZip(file, stream, zipFile, default(LoadableContentDescription));
            loadableZip.Update(this);
            return loadableZip;
        }


        private const string DescriptionFileName = "LoadableContentDescr";
        private readonly XmlSerializer<LoadableContentDescription> descriptionSerializer = new XmlSerializer<LoadableContentDescription>();

        private string DescriptionPath
        {
            get { return DescriptionFileName + descriptionSerializer.FileExtension; }
        }

        internal void WriteDescription(LoadableContent content, LoadableContentDescription description)
        {
            using (var stream = content.CreateOutStream(DescriptionPath))
            {
                descriptionSerializer.Serialize(stream, description);
            }
        }

        internal LoadableContentDescription ReadDescription(LoadableContent content)
        {
            using (var stream = content.GetInputStream(DescriptionPath))
            {
                var descr = descriptionSerializer.Deserialize(stream);
                return descr;
            }
        }
    }
}
