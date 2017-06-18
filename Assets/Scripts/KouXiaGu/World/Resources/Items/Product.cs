using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;
using KouXiaGu.Resources;

namespace KouXiaGu.World.Commerce
{

    [XmlType("Product")]
    public class ProductInfo : IElement
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// 产品价值;
        /// </summary>
        [XmlElement("Worth")]
        public int Worth { get; set; }



        /// <summary>
        /// 图标
        /// </summary>
        [XmlIgnore]
        public Texture Icon { get; set; }
    }
}
