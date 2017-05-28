using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Resources
{

    [XmlType("Product")]
    public class ProductElementInfo : ElementInfo
    {

    }

    /// <summary>
    /// 地形信息文件路径;
    /// </summary>
    public class ProductInfosFilePath : CustomFilePath
    {
        public ProductInfosFilePath(string fileExtension) : base(fileExtension)
        {
        }

        public override string FileName
        {
            get { return "World/Terrain/Product"; }
        }
    }

    /// <summary>
    /// 地形信息读取;
    /// </summary>
    public class ProductInfosXmlSerializer : DataDictionaryXmlReader<ProductElementInfo>
    {
        public ProductInfosXmlSerializer()
        {
            file = new ProductInfosFilePath(FileExtension);
        }

        ProductInfosFilePath file;

        public override CustomFilePath File
        {
            get { return file; }
        }
    }

}
