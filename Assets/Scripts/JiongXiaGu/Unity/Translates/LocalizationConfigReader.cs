using JiongXiaGu.Resources;
using JiongXiaGu.Unity.Resources;
using System.IO;

namespace JiongXiaGu.Unity.Translates
{

    /// <summary>
    /// 本地化配置文件;
    /// </summary>
    public class LocalizationConfigReader : FileReader<LocalizationConfig>
    {

        [FileDefinition(ResourceTypes.UserConfig, "本地化组件信息配置")]
        public const string FileName = "Configs/LocalizationConfig";

        public LocalizationConfigReader() : base(new XmlFileSerializer<LocalizationConfig>())
        {
        }

        public override string GetFilePath()
        {
            string path = Path.Combine(Resource.UserConfigDirectoryPath, FileName + FileExtension);
            return path;
        }
    }
}
