using JiongXiaGu.Unity.Resources;
using System.IO;

namespace JiongXiaGu.Unity.KeyInputs
{
    /// <summary>
    /// 默认按键配置文件读写器;
    /// </summary>
    public class DefaultKeyConfigReader : ConfigFileReader<KeyInfo[]>
    {
        const string ConfigFileName = "Configs/KeyboardInput";

        public DefaultKeyConfigReader() : base(new XmlSerializer<KeyInfo[]>())
        {
        }

        public override string GetFilePathWithoutExtension()
        {
            string path = Path.Combine(Resource.ConfigDirectory, ConfigFileName);
            return path;
        }
    }
}
