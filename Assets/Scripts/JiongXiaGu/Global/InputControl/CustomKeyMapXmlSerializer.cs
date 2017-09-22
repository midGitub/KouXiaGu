using JiongXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace JiongXiaGu.InputControl
{


    public class CustomKeyMapXmlSerializer : FileReaderWriter<CustomKeyMap>
    {
        public CustomKeyMapXmlSerializer() : base(new CustomKeyMapFilePath(), new XmlFileSerializer<CustomKeyMap>())
        {
        }

        public CustomKeyMapXmlSerializer(ISingleFilePath file, IOFileSerializer<CustomKeyMap> serializer) : base(file, serializer)
        {
        }
    }

    public class CustomKeyMapFilePath : SingleFilePath
    {
        public override string FileName
        {
            get { return "Input\\Keyboard.xml"; }
        }
    }
}
