using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Globalization
{

    public class LanguagePackStream : LanguagePack
    {
        public LanguagePackStream(string name, string locName, Stream stream) : base(name, locName)
        {
            Stream = stream;
        }

        public Stream Stream { get; private set; }

        /// <summary>
        /// 结束流;
        /// </summary>
        public void Close()
        {
            Stream.Dispose();
        }
    }
}
