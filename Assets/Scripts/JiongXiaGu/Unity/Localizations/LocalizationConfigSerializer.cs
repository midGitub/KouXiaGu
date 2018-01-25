using JiongXiaGu.Unity.Resources;
using System.IO;

namespace JiongXiaGu.Unity.Localizations
{


    public class LocalizationConfigSerializer : ContentSerializer<LocalizationConfig>
    {
        [PathDefinition(PathDefinitionType.UserConfig, "本地化组件信息配置")]
        public override string RelativePath
        {
            get { return "Configs/LocalizationConfig.xml"; }
        }

        private readonly XmlSerializer<LocalizationConfig> xmlSerializer = new XmlSerializer<LocalizationConfig>();
        public override ISerializer<LocalizationConfig> Serializer => xmlSerializer;
    }


    ///// <summary>
    ///// 本地化配置文件;
    ///// </summary>
    //public class LocalizationConfigFileReader : ConfigFileReader<LocalizationConfig>
    //{

    //    [PathDefinition(PathDefinitionType.UserConfig, "本地化组件信息配置")]
    //    internal const string FileName = "Configs/LocalizationConfig";

    //    public LocalizationConfigFileReader() : base(new XmlSerializer<LocalizationConfig>())
    //    {
    //    }

    //    public override string GetFilePathWithoutExtension()
    //    {
    //        string path = Path.Combine(Resource.UserConfigDirectory, FileName);
    //        return path;
    //    }
    //}
}
