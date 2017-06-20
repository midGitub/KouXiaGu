using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 建筑商业信息;
    /// </summary>
    [XmlType("BuildingCommerce")]
    public class BuildingCommerceInfo
    {
        /// <summary>
        /// 建筑价值;
        /// </summary>
        [XmlElement("Worth")]
        public int Worth { get; set; }

        [XmlArrayItem("ProductionContent")]
        public List<ProductionContent> ProductionContents { get; internal set; }

        [XmlIgnore]
        public int CurrentProductionContentIndex { get; set; }

        /// <summary>
        /// 当前生产内容;
        /// </summary>
        public ProductionContent CurrentProductionContent
        {
            get { return ProductionContents[CurrentProductionContentIndex]; }
        }

        /// <summary>
        /// 根据名字指定生产内容;
        /// </summary>
        public void SetProduction(string name)
        {
            int index = ProductionContents.FindIndex(item => item.Name == name);
            if (index >= 0)
            {
                CurrentProductionContentIndex = index;
            }
            else
            {
                throw new KeyNotFoundException(name);
            }
        }
    }

    /// <summary>
    /// 生产内容;
    /// </summary>
    [XmlType("ProductionContent")]
    public class ProductionContent
    {
        ProductionContent()
        {
        }

        public ProductionContent(string name, IEnumerable<ProductProduction> products)
        {
            Name = name;
            Products = new List<ProductProduction>(products);
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Production")]
        public List<ProductProduction> Products { get; set; }
    }

    /// <summary>
    /// 产品生产信息;
    /// </summary>
    [XmlType("ProductProduction")]
    public class ProductProduction
    {
        ProductProduction()
        {
        }

        public ProductProduction(int productType, int dailyProductionCount)
        {
            ProductType = productType;
            DailyProductionCount = dailyProductionCount;
        }

        [XmlAttribute("productType")]
        public int ProductType { get; set; }

        [XmlAttribute("dailyProductionCount")]
        public int DailyProductionCount { get; set; }
    }
}
