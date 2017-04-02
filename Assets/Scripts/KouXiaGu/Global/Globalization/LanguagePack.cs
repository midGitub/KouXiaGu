using System.IO;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 语言包信息;
    /// </summary>
    public struct LanguagePack
    {
        public LanguagePack(string name, string locName, string path)
        {
            Name = name;
            LocName = locName;
            FilePath = path;
        }

        public string Name { get; private set; }
        public string LocName { get; private set; }
        public string FilePath { get; private set; }

        public bool Exists()
        {
            return File.Exists(FilePath);
        }

    }

}
