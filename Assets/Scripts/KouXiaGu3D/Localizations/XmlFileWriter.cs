using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace KouXiaGu.Localizations
{


    public class XmlFileWriter : XmlFile, ITextWriter
    {
        public XmlFileWriter(string filePath)
        {
            this.FilePath = filePath;
            writer = XmlWriter.Create(FilePath, xmlWriterSettings);
        }

        public string FilePath { get; private set; }

        XmlWriter writer;

        public void WriteTexts(string language, IEnumerable<TextPack> texts)
        {
            WriteTexts(writer, language, texts);
        }
    }

}
