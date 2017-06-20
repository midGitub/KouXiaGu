using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World.Resources
{

    /// <summary>
    /// 城镇预定义信息;
    /// </summary>
    [XmlType("Town")]
    public class TownInfo : IElement
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }
    }

    class TownInfoFile : MultipleFilePath
    {
        [CustomFilePath("城镇描述文件;", true)]
        public const string fileName = "World/Resources/Towns.xml";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    class TownInfoXmlSerializer : XmlElementsReaderWriter<TownInfo>
    {
        public TownInfoXmlSerializer() : base(new TownInfoFile())
        {
        }
    }
}
