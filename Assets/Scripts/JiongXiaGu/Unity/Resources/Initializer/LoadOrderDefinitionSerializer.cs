namespace JiongXiaGu.Unity.Resources
{
    public class LoadOrderDefinitionSerializer : ContentSerializer<LoadOrderDefinition>
    {
        [PathDefinition(PathDefinitionType.Config, "地形资源定义")]
        public override string RelativePath
        {
            get { return "Configs/ContentLoadOrder"; }
        }

        private readonly XmlSerializer<LoadOrderDefinition> xmlSerializer = new XmlSerializer<LoadOrderDefinition>();

        public override ISerializer<LoadOrderDefinition> Serializer
        {
            get { return xmlSerializer; }
        }
    }
}
