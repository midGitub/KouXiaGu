using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace KouXiaGu.XmlLocalization
{


    public class Language
    {

        public Language(string name, string languageTag)
        {
            this.Name = name;
            this.LanguageTag = languageTag;
            CultureInfo = new CultureInfo(languageTag);
        }

        public string Name { get; private set; }
        public string LanguageTag { get; private set; }

        public CultureInfo CultureInfo { get; private set; }

    }

}
