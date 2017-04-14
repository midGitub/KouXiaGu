using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Globalization
{

    public class Language
    {
        public Language(Language item)
            : this(item.Name, item.LocName)
        {
        }

        public Language(string name, string locName)
        {
            Name = name;
            LocName = locName;
        }

        public string Name { get; private set; }
        public string LocName { get; private set; }
    }


    public class LanguagePack : Language
    {

        public LanguagePack(string name, string locName)
            : base(name, locName)
        {
            TextDictionary = new Dictionary<string, string>();
        }

        public LanguagePack(Language file, Dictionary<string, string> texts)
            : base(file)
        {
            TextDictionary = texts;
        }

        public LanguagePack(string name, string locName, Dictionary<string, string> texts)
             : base(name, locName)
        {
            TextDictionary = texts;
        }

        public Dictionary<string, string> TextDictionary { get; private set; }
    }
    

    public class LanguageFile : Language
    {
        public LanguageFile(Language file, string path)
            : this(file.Name, file.LocName, path)
        {
        }

        public LanguageFile(string name, string locName, string path)
            : base(name, locName)
        {
            FilePath = path;
        }

        public string FilePath { get; private set; }

        public bool Exists()
        {
            return File.Exists(FilePath);
        }
    }

}
