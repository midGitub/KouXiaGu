using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Globalization
{


    public static class Localization
    {

        public static string Language { get; private set; }
        public static IDictionary<string, string> TextDictionary { get; private set; }

    }
}
