namespace JiongXiaGu.Unity.Resources
{
    public class LoadOrderDefinitionSerializer : ContentSerializer<LoadOrderDefinition>
    {
        [PathDefinition(PathDefinitionType.Config, "资源读取顺序定义")]
        public override string RelativePath
        {
            get { return "ContentLoadOrder"; }
        }

        private readonly XmlSerializer<LoadOrderDefinition> xmlSerializer = new XmlSerializer<LoadOrderDefinition>();

        public override ISerializer<LoadOrderDefinition> Serializer
        {
            get { return xmlSerializer; }
        }
    }
}
