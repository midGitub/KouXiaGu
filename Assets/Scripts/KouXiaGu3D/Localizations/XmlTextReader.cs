using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Localizations
{

    public class XmlTextReader : ITextReader
    {

        public XmlTextReader(XmlLanguagePack pack)
        {
            this.pack = pack;
        }

        XmlLanguagePack pack;

        public IEnumerable<TextItem> ReadTexts()
        {
            return XmlFiler.ReadTexts(pack.FilePath);
        }

    }

}
