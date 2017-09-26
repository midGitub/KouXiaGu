using JiongXiaGu.Collections;
using JiongXiaGu.Resources;
using System.IO;
using System.Linq;

namespace JiongXiaGu.Unity.KeyInputs
{

    /// <summary>
    /// 按键配置读写器;
    /// </summary>
    public class KeyMapReader : IResourceReader<KeyMap>
    {
        public KeyMapReader()
        {
            DefaultKeyConfigReader = new DefaultKeyConfigReader();
            UserKeyConfigReader = new UserKeyConfigReader();
        }

        public DefaultKeyConfigReader DefaultKeyConfigReader { get; private set; }
        public UserKeyConfigReader UserKeyConfigReader { get; private set; }

        /// <summary>
        /// 读取到按键映射;
        /// </summary>
        public KeyMap Deserialize()
        {
            KeyInfo[] defaultKeys = DefaultKeyConfigReader.Deserialize();
            var keyMap = new KeyMap();

            try
            {
                KeyInfo[] userKeys = UserKeyConfigReader.Deserialize();
                keyMap.AddOrUpdate(defaultKeys);
                keyMap.AddOrUpdate(userKeys);
            }
            catch
            {
                keyMap.AddOrUpdate(defaultKeys);
            }

            return keyMap;
        }

        /// <summary>
        /// 合并两个信息,若定义的按键存在重复的Name,则只保留最后一个信息;
        /// </summary>
        KeyMap Combine(KeyInfo[] defaultKeys, KeyInfo[] userKeys)
        {
            var dictionary = new KeyMap();

            foreach (var info in defaultKeys)
            {
                dictionary.AddOrUpdate(info.Name, info.Key);
            }

            foreach (var info in userKeys)
            {
                dictionary.AddOrUpdate(info.Name, info.Key);
            }

            return dictionary;
        }

        /// <summary>
        /// 默认输出到用户配置文件;
        /// </summary>
        public void Serialize(KeyMap keyMap)
        {
            var keyInfos = keyMap.ToArray(pair => new KeyInfo(pair.Key, pair.Value));
            UserKeyConfigReader.Serialize(keyInfos);
        }
    }

    /// <summary>
    /// 默认按键配置文件读写器;
    /// </summary>
    public class DefaultKeyConfigReader : FileReader<KeyInfo[]>
    {
        const string ConfigFileName = "Configs/KeyboardInput";

        public DefaultKeyConfigReader() : base(new XmlFileSerializer<KeyInfo[]>())
        {
        }

        public override string GetFilePath()
        {
            string path = Path.Combine(Resource.DataDirectoryPath, ConfigFileName + FileExtension);
            return path;
        }
    }

    /// <summary>
    /// 用户按键配置文件读写器;
    /// </summary>
    public class UserKeyConfigReader : FileReader<KeyInfo[]>
    {
        const string ConfigFileName = "Configs/KeyboardInput";

        public UserKeyConfigReader() : base(new XmlFileSerializer<KeyInfo[]>())
        {
        }

        public override string GetFilePath()
        {
            string path = Path.Combine(Resource.UserConfigDirectoryPath, ConfigFileName + FileExtension);
            return path;
        }
    }
}
