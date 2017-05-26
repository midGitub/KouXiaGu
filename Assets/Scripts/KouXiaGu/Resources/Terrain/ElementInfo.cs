using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Resources
{

    public abstract class ElementInfo
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return "[ID:" + ID + ",Name:" + Name + "]"; 
        }
    }

}
