using ICSharpCode.SharpZipLib.Zip;
using JiongXiaGu.Unity.Resources;
using System.Collections.Generic;
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
        public LanguagePack Deserialize(Stream stream)
        {
            LanguagePackDescription? description = null;
            LanguageDictionary dictionary = null;

            using (ZipInputStream zipInputStream = new ZipInputStream(stream))
            {
                zipInputStream.IsStreamOwner = false;

                ZipEntry entry;
                while ((entry = zipInputStream.GetNextEntry()) != null)
                {
                    if (!description.HasValue && entry.Name == descriptionFileName)
                    {
                        description = descriptionSerializer.Deserialize(zipInputStream);
                    }
                    else if (dictionary == null && entry.Name == dictionaryFileName)
                    {
                        dictionary = dictionarySerializer.Deserialize(zipInputStream);
                    }
                }

                if (description.HasValue && dictionary != null)
                    return new LanguagePack(description.Value, dictionary);
                else
                    throw new FileNotFoundException();
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
