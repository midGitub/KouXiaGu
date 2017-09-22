using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace JiongXiaGu.World.Commerce
{

    /// <summary>
    /// 产品信息;
    /// </summary>
    [XmlType("Commerce")]
    public class ProductCommerceInfo
    {
        /// <summary>
        /// 价值;
        /// </summary>
        [XmlElement("Worth")]
        public int Worth { get; set; }

        /// <summary>
        /// 需求度;
        /// </summary>
        [XmlElement("Demand")]
        public int Demand { get; set; }
    }
}
