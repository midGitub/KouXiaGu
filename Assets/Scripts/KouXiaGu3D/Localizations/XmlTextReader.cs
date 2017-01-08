using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Localizations
{

    public class XmlTextReader : ITextReader
    {

        public XmlTextReader(XmlLanguageFile pack)
        {
            this.pack = pack;
        }

        XmlLanguageFile pack;

        public IEnumerable<TextItem> ReadTexts()
        {
            return XmlFiler.ReadTexts(pack.FilePath);
        }

        public override string ToString()
        {
            return pack.ToString();
        }

    }

}
