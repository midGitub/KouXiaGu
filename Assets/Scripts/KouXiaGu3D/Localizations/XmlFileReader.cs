using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace KouXiaGu.Localizations
{


    public class XmlFileReader : XmlFile, ITextReader
    {

        public XmlFileReader(string filePath)
        {
            this.FilePath = filePath;
            reader = XmlReader.Create(filePath, xmlReaderSettings);
        }

        public string FilePath { get; private set; }

        XmlReader reader;

        public IEnumerable<TextPack> ReadTexts()
        {
            return ReadTexts(reader);
        }

    }

}
