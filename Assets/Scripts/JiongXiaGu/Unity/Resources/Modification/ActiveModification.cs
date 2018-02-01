using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源读取顺序(不包括核心资源);
    /// </summary>
    public struct ActiveModification
    {
        [XmlArrayItem("ID")]
        public List<string> IDList { get; set; }

        public ActiveModification(IEnumerable<string> contents)
        {
            IDList = new List<string>(contents);
        }
    }

    public class ActiveModificationSerializer : ContentSerializer<ActiveModification>
    {
        [PathDefinition(PathDefinitionType.Config, "资源读取顺序定义")]
        public override string RelativePath
        {
            get { return "ActiveModification.xml"; }
        }

        private readonly XmlSerializer<ActiveModification> xmlSerializer = new XmlSerializer<ActiveModification>();

        public override ISerializer<ActiveModification> Serializer
        {
            get { return xmlSerializer; }
        }

        /// <summary>
        /// 分别从用户配置和数据配置获取到模组读取顺序;
        /// </summary>
        /// <exception cref="FileNotFoundException">未找到对应文件</exception>
        public ActiveModification Deserialize()
        {
            ActiveModification activeModification;
            if (TryDeserialize(Resource.UserConfigContent, out activeModification))
            {
                return activeModification;
            }

            if (TryDeserialize(Resource.ConfigContent, out activeModification))
            {
                return activeModification;
            }

            throw new FileNotFoundException();
        }
    }
}
