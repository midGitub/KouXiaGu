using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace KouXiaGu.Localizations
{


    public class XmlFileReader : XmlFile, ITextReader
    {

        public XmlFileReader(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            this.FilePath = filePath;
        }

        public string FilePath { get; private set; }

        public IEnumerable<TextPack> ReadTexts()
        {
            using (XmlReader reader = XmlReader.Create(FilePath, xmlReaderSettings))
            {
                return ReadTexts(reader);
            }
        }

    }

}
