using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 产品信息;
    /// </summary>
    [XmlType("Commerce")]
    public class CommerceProduct
    {
        /// <summary>
        /// 产品价值;
        /// </summary>
        [XmlElement("Worth")]
        public int Worth { get; set; }
    }
}
