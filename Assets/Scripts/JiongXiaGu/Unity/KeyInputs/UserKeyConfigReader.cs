using JiongXiaGu.Unity.Resources;
using System.IO;

namespace JiongXiaGu.Unity.KeyInputs
{
    /// <summary>
    /// 用户按键配置文件读写器;
    /// </summary>
    public class UserKeyConfigReader : ConfigFileReader<KeyInfo[]>
    {
        const string ConfigFileName = "Configs/KeyboardInput";

        public UserKeyConfigReader() : base(new XmlSerializer<KeyInfo[]>())
        {
        }

        public override string GetFilePathWithoutExtension()
        {
            string path = Path.Combine(Resource.UserConfigDirectory, ConfigFileName);
            return path;
        }
    }
}
