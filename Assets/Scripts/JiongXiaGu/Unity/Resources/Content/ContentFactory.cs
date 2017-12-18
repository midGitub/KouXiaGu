using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{


    public class ContentFactory
    {

        /// <summary>
        /// 创建可读内容,若目录已经存在则返回异常;
        /// </summary>
        public Content CreateNew(string directory)
        {
            if (Directory.Exists(directory))
                throw new IOException(string.Format("目录[{0}]已经存在;", directory));

            Directory.CreateDirectory(directory);
            ContentDirectory contentDirectory = new ContentDirectory(directory);
            return contentDirectory;
        }

        /// <summary>
        /// 创建可读内容,若文件已经存在则返回异常;
        /// </summary>
        public Content CreateNewZip(string zipFile)
        {
            Stream stream = new FileStream(zipFile, FileMode.CreateNew, FileAccess.ReadWrite);
            ZipFile zip = ZipFile.Create(stream);
            ContentZip contentZip = new ContentZip(zipFile, stream, zip);
            return contentZip;
        }

        /// <summary>
        /// 读取内容,若目录不存在,或者不是定义的可读内容则返回异常;
        /// </summary>
        public Content Read(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            ContentDirectory contentDirectory = new ContentDirectory(directory);
            return contentDirectory;
        }

        /// <summary>
        /// 读取内容,若文件不存在,或者不是定义的可读内容则返回异常;
        /// </summary>
        public Content ReadZip(string file)
        {
            Stream stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
            ZipFile zipFile = new ZipFile(stream);
            ContentZip contentZip = new ContentZip(file, stream, zipFile);
            return contentZip;
        }
    }
}
