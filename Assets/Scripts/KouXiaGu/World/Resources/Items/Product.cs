using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;
using KouXiaGu.Resources;
using KouXiaGu.World.Commerce;

namespace KouXiaGu.World.Resources
{

    [XmlType("Product")]
    public class ProductInfo : IElement
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("Commerce")]
        public ProductCommerceInfo Commerce { get; set; }
    }

    class ProductFile : MultipleFilePath
    {
        [CustomFilePath("产品类型描述文件;", true)]
        public const string fileName = "World/Resources/Products.xml";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    class ProductInfoXmlSerializer : XmlElementsReaderWriter<ProductInfo>
    {
        public ProductInfoXmlSerializer() : base(new ProductFile())
        {
        }
    }
}
