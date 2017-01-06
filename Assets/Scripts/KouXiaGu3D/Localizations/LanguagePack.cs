using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Localizations
{


    public class LanguagePack : ITextReader, ITextWriter
    {

        /// <summary>
        /// 语言文件匹配的搜索字符串;
        /// </summary>
        const string LANGUAGE_PACK_SEARCH_PATTERN = "*" + XmlFile.FILE_EXTENSION;

        /// <summary>
        /// 搜索并获取到所有语言包;
        /// </summary>
        public static IEnumerable<LanguagePack> Find(string directoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var paths = Directory.GetFiles(directoryPath, LANGUAGE_PACK_SEARCH_PATTERN, searchOption);

            foreach (var path in paths)
            {
                LanguagePack pack;
                if (TryLoad(path, out pack))
                    yield return pack;
            }
        }

        public static bool TryLoad(string filePath, out LanguagePack pack)
        {
            try
            {
                pack = Load(filePath);
                return true;
            }
            catch
            {
                pack = default(LanguagePack);
                return false;
            }
        }

        public static LanguagePack Load(string filePath)
        {
            string language = XmlFile.GetLanguage(filePath);
            return new LanguagePack(language, filePath);
        }


        public LanguagePack(string language, string file)
        {
            this.Language = language;
            this.FilePath = file;
        }


        public string Language { get; private set; }

        public string FilePath { get; private set; }


        public IEnumerable<TextPack> ReadTexts()
        {
            return XmlFile.ReadTexts(FilePath);
        }

        public void WriteTexts(IEnumerable<TextPack> texts)
        {
            XmlFile.WriteTexts(FilePath, Language, texts);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LanguagePack))
                return false;
            return ((LanguagePack)obj).FilePath == FilePath;
        }

        public override int GetHashCode()
        {
            return FilePath.GetHashCode();
        }

        public override string ToString()
        {
            return "[Language:" + Language + ",File:" + FilePath + "]";
        }

    }

}
