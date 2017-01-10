using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.xgLocalization
{

    public class XmlTextReader : ITextReader
    {

        public XmlTextReader(XmlLanguageFile pack)
        {
            this.pack = pack;
        }

        XmlLanguageFile pack;

        public Language Language
        {
            get { return pack.Language; }
        }

        public IEnumerable<TextItem> ReadTexts()
        {
            return XmlFile.ReadTexts(pack.FilePath);
        }

        public override string ToString()
        {
            return pack.ToString();
        }

    }

}
