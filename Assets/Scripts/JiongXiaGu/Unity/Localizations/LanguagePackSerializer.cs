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
        public LanguagePack Deserialize(string file)
        {
            using (var fStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                return Deserialize(fStream);
            }
        }

        /// <summary>
        /// 读取语言包;
        /// </summary>
        public LanguagePack Deserialize(Stream stream)
        {
            LanguagePackDescription description = default(LanguagePackDescription);
            LanguageDictionary dictionary = default(LanguageDictionary);

            using (ZipFile zipFile = new ZipFile(stream))
            {
                zipFile.IsStreamOwner = false;

                ZipEntry descZipEntry = zipFile.GetEntry(descriptionFileName);
                if (descZipEntry != null)
                {
                    var descStream = zipFile.GetInputStream(descZipEntry);
                    description = descriptionSerializer.Deserialize(descStream);
                }
                else
                {
                    throw new FileNotFoundException(string.Format("未找到对应文件:{0}", descriptionFileName));
                }

                ZipEntry dictionaryZipEntry = zipFile.GetEntry(dictionaryFileName);
                if (dictionaryZipEntry != null)
                {
                    var dictionarystream = zipFile.GetInputStream(dictionaryZipEntry);
                    dictionary = dictionarySerializer.Deserialize(dictionarystream);
                }
                else
                {
                    throw new FileNotFoundException(string.Format("未找到对应文件:{0}", dictionaryFileName));
                }
            }
            return new LanguagePack(description, dictionary);
        }

        /// <summary>
        /// 读取到描述文件;
        /// </summary>
        public LanguagePackDescription DeserializeDesc(string file)
        {
            using (var fStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                return DeserializeDesc(fStream);
            }
        }

        /// <summary>
        /// 读取到描述文件;
        /// </summary>
        public LanguagePackDescription DeserializeDesc(Stream stream)
        {
            using (ZipFile zipFile = new ZipFile(stream))
            {
                zipFile.IsStreamOwner = false;

                ZipEntry descZipEntry = zipFile.GetEntry(descriptionFileName);
                if (descZipEntry != null)
                {
                    var descStream = zipFile.GetInputStream(descZipEntry);
                    return descriptionSerializer.Deserialize(descStream);
                }
                else
                {
                    throw new FileNotFoundException(string.Format("未找到对应文件:{0}", descriptionFileName));
                }
            }
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
    }
}
