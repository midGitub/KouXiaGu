using System.IO;
using KouXiaGu.Resources;

namespace KouXiaGu.Globalization
{

    public class LanguagePackFilePath : SingleFilePath
    {
        public LanguagePackFilePath(LanguagePack pack) : this(pack.Name)
        {
        }

        public LanguagePackFilePath(string packName)
        {
            fileName = "Localization/language_" + packName + ".xml";
        }

        string fileName;

        public override string FileName
        {
            get { return fileName; }
        }

        public Stream LoadStream()
        {
            string filePath = GetFullPath();
            return new FileStream(filePath, FileMode.Create);
        }
    }
}
