using ICSharpCode.SharpZipLib.Zip;
using JiongXiaGu.Unity.Resources;
using System.IO;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 语言包读写器;
    /// </summary>
    public class LanguagePackSerializer
    {
        private const string descriptionFileName = "Description.xml";
        private const string dictionaryFileName = "Dictionary.xml";
        private readonly XmlSerializer<LanguagePackDescription> descriptionSerializer;
        private readonly XmlSerializer<LanguageDictionary> dictionarySerializer;

        public LanguagePackSerializer()
        {
            descriptionSerializer = new XmlSerializer<LanguagePackDescription>();
            dictionarySerializer = new XmlSerializer<LanguageDictionary>();
        }

        /// <summary>
        /// 读取语言包;
        /// </summary>
        public LanguagePack Deserialize(LoadableContent loadableContent, ILoadableEntry entry)
        {
            using (var stream = loadableContent.GetStream(entry))
            {
                return Deserialize(stream);
            }
        }

        /// <summary>
        /// 读取语言包;
        /// </summary>
        public LanguagePack Deserialize(Stream stream)
        {
            LanguagePackDescription? description = null;
            LanguageDictionary dictionary = null;

            using (ZipInputStream zipInputStream = new ZipInputStream(stream))
            {
                ZipEntry entry;
                zipInputStream.IsStreamOwner = false;
                while ((entry = zipInputStream.GetNextEntry()) != null)
                {
                    if (description == null && entry.Name == descriptionFileName)
                    {
                        description = descriptionSerializer.Deserialize(zipInputStream);
                    }
                    else if (dictionary == null && entry.Name == dictionaryFileName)
                    {
                        dictionary = dictionarySerializer.Deserialize(zipInputStream);
                    }
                }

                if (description != null && dictionary != null)
                    return new LanguagePack(description.Value, dictionary);
                else
                    throw new FileNotFoundException();
            }

            //using (ZipFile zipFile = new ZipFile(stream))
            //{
            //    zipFile.IsStreamOwner = false;
            //    ZipContent zipContent = Check(zipFile);

            //    var descStream = zipFile.GetInputStream(zipContent.Description);
            //    description = descriptionSerializer.Deserialize(descStream);

            //    var dictionarystream = zipFile.GetInputStream(zipContent.Dictionary);
            //    dictionary = dictionarySerializer.Deserialize(dictionarystream);
            //}
            //return new LanguagePack(description, dictionary);
        }

        public LanguagePackDescription DeserializeDesc(LoadableContent loadableContent, ILoadableEntry entry)
        {
            using (var stream = loadableContent.GetStream(entry))
            {
                return DeserializeDesc(stream);
            }
        }

        /// <summary>
        /// 读取到描述文件;
        /// </summary>
        public LanguagePackDescription DeserializeDesc(Stream stream)
        {
            using (ZipInputStream zipInputStream = new ZipInputStream(stream))
            {
                zipInputStream.IsStreamOwner = false;
                ZipEntry entry;
                while ((entry = zipInputStream.GetNextEntry()) != null)
                {
                    if (entry.Name == descriptionFileName)
                    {
                        var description = descriptionSerializer.Deserialize(zipInputStream);
                        return description;
                    }
                }
                throw new FileNotFoundException();
            }

            //using (ZipFile zipFile = new ZipFile(stream))
            //{
            //    zipFile.IsStreamOwner = false;
            //    ZipContent zipContent = Check(zipFile);
            //    var descStream = zipFile.GetInputStream(zipContent.Description);
            //    return descriptionSerializer.Deserialize(descStream);
            //}
        }

        /// <summary>
        /// 检查压缩包完整性;
        /// </summary>
        private ZipContent Check(ZipFile zipFile)
        {
            ZipContent zipContent = new ZipContent();
            zipContent.ZipFile = zipFile;

            zipContent.Description = zipFile.GetEntry(descriptionFileName);
            if (zipContent.Description == null)
            {
                throw new FileNotFoundException(string.Format("未找到对应文件:{0}", descriptionFileName));
            }

            zipContent.Dictionary = zipFile.GetEntry(dictionaryFileName);
            if (zipContent.Dictionary == null)
            {
                throw new FileNotFoundException(string.Format("未找到对应文件:{0}", dictionaryFileName));
            }

            return zipContent;
        }

        /// <summary>
        /// 输出语言包;
        /// </summary>
        public void Serialize(string file, LanguagePack pack)
        {
            using (var fStream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite))
            {
                Serialize(fStream, pack);
            }
        }

        /// <summary>
        /// 输出语言包;
        /// </summary>
        public void Serialize(Stream stream, LanguagePack pack)
        {
            using (ZipOutputStream zipOutputStream = new ZipOutputStream(stream))
            {
                zipOutputStream.IsStreamOwner = false;

                ZipEntry descZipEntry = new ZipEntry(descriptionFileName);
                zipOutputStream.PutNextEntry(descZipEntry);
                descriptionSerializer.Serialize(zipOutputStream, pack.Description);

                ZipEntry dictionaryZipEntry = new ZipEntry(dictionaryFileName);
                zipOutputStream.PutNextEntry(dictionaryZipEntry);
                dictionarySerializer.Serialize(zipOutputStream, pack.LanguageDictionary);
            }
        }

        /// <summary>
        /// 表示压缩包内的文件;
        /// </summary>
        private struct ZipContent
        {
            public ZipFile ZipFile { get; set; }
            public ZipEntry Description { get; set; }
            public ZipEntry Dictionary { get; set; }
        }
    }
}
