using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 读取资源压缩包;
    /// </summary>
    public class LoadableZipFileReader
    {
        /// <summary>
        /// 读取模组信息,若不存在则返回异常;
        /// </summary>
        public LoadableZipFile Create(string filePath, LoadableContentType type)
        {
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            ZipFile zipFile = new ZipFile(stream);
            LoadableContentDescription description = ReadDescr(zipFile);

            LoadableZipFile info = new LoadableZipFile(zipFile, stream, description, type);
            return info;
        }

        private const string DescriptionFileName = "Description";
        private readonly XmlSerializer<LoadableContentDescription> descriptionSerializer = new XmlSerializer<LoadableContentDescription>();

        /// <summary>
        /// 读取到资源的描述;
        /// </summary>
        public LoadableContentDescription ReadDescr(ZipFile zipFile)
        {
            string descriptionFileName = DescriptionFileName + descriptionSerializer.FileExtension;
            ZipEntry zipEntry = zipFile.GetEntry(descriptionFileName);
            if (zipEntry != null)
            {
                using (var stream = zipFile.GetInputStream(zipEntry))
                {
                    LoadableContentDescription description = descriptionSerializer.Deserialize(stream);
                    return description;
                }
            }
            else
            {
                throw new FileNotFoundException("未找到描述文件;");
            }
        }


        //    private const string TempDirectoryDescriptionFileName = "Description";
        //    private readonly XmlSerializer<ZipTempDirectoryDescription> tempDescrXmlSerializer = new XmlSerializer<ZipTempDirectoryDescription>();

        //    /// <summary>
        //    /// 读取目录的描述;
        //    /// </summary>
        //    private ZipTempDirectoryDescription ReadTempDescr(string tempDirectory)
        //    {
        //        string filePath = GetTempDescrPath(tempDirectory);
        //        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //        {
        //            var descr = tempDescrXmlSerializer.Deserialize(stream);
        //            return descr;
        //        }
        //    }

        //    /// <summary>
        //    /// 输出描述文件到目录;
        //    /// </summary>
        //    private void WriteTempDescr(string tempDirectory, ZipTempDirectoryDescription descr)
        //    {
        //        string filePath = GetTempDescrPath(tempDirectory);
        //        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        //        {
        //            tempDescrXmlSerializer.Serialize(stream, descr);
        //        }
        //    }

        //    private string GetTempDescrPath(string tempDirectory)
        //    {
        //        string descriptionFileName = TempDirectoryDescriptionFileName + tempDescrXmlSerializer.FileExtension;
        //        string filePath = Path.Combine(tempDirectory, descriptionFileName);
        //        return filePath;
        //    }
        //}


        ///// <summary>
        ///// 临时文件夹描述;
        ///// </summary>
        //public struct ZipTempDirectoryDescription
        //{
        //    /// <summary>
        //    /// 资源文件的DM5值;
        //    /// </summary>
        //    public string MD5 { get; set; }
        //}
    }
}