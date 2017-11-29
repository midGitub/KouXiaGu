using JiongXiaGu.Unity.Resources;
using System.IO;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 本地化配置文件;
    /// </summary>
    public class LocalizationConfigFileReader : ConfigFileReader<LocalizationConfig>
    {

        [PathDefinition(PathDefinition.UserConfig, "本地化组件信息配置")]
        internal const string FileName = "Configs/LocalizationConfig";

        public LocalizationConfigFileReader() : base(new XmlSerializer<LocalizationConfig>())
        {
        }

        public override string GetFilePathWithoutExtension()
        {
            string path = Path.Combine(ResourcePath.UserConfigDirectory, FileName);
            return path;
        }
    }
}
