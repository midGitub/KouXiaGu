using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Localizations
{

    /// <summary>
    /// 语言文件
    /// </summary>
    public class XmlLanguageFile
    {

        public XmlLanguageFile(string language, string file)
        {
            this.Language = language;
            this.FilePath = file;
        }

        public string Language { get; private set; }

        public string FilePath { get; private set; }

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
            return "[Language:" + Language + ",File:" + FilePath + "]";
        }

    }

}
