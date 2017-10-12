using JiongXiaGu.Unity.Resources;
using System.IO;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 本地化配置文件;
    /// </summary>
    public class LocalizationConfigReader : ConfigFileReader<LocalizationConfig>
    {

        [PathDefinition(ResourceTypes.UserConfig, "本地化组件信息配置")]
        internal const string FileName = "Configs/LocalizationConfig";

        public LocalizationConfigReader() : base(new XmlFileSerializer<LocalizationConfig>())
        {
        }

        public override string GetFilePathWithoutExtension()
        {
            string path = Path.Combine(Resource.UserConfigDirectory, FileName);
            return path;
        }
    }
}
