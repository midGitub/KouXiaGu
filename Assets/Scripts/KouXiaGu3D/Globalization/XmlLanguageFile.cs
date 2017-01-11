using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 语言文件
    /// </summary>
    public class XmlLanguageFile
    {

        public XmlLanguageFile(Language language, string file)
        {
            this.Language = language;
            this.FilePath = file;
        }

        public Language Language { get; private set; }

        public string FilePath { get; private set; }


        /// <summary>
        /// 读取所有文本;
        /// </summary>
        public TextDictionary ReadTexts()
        {
            return new TextDictionary(XmlFile.ReadTexts(FilePath));
        }

        /// <summary>
        /// 创建一个新的文件,并且写入;
        /// </summary>
        /// <param name="textItems"></param>
        public void CreateTexts(IEnumerable<TextItem> textItems)
        {
            XmlFile.CreateTexts(FilePath, Language, textItems);
        }

        /// <summary>
        /// 加入到已存在的文本中;
        /// </summary>
        public void AppendTexts(IEnumerable<TextItem> textItems)
        {
            XmlFile.AppendTexts(FilePath, textItems);
        }


        public override bool Equals(object obj)
        {
            if (!(obj is XmlLanguageFile))
                return false;
            return ((XmlLanguageFile)obj).FilePath == FilePath;
        }

        public override int GetHashCode()
        {
            return FilePath.GetHashCode();
        }

        public override string ToString()
        {
            return "[Language:" + Language.ToString() + ",File:" + FilePath + "]";
        }

    }

}
