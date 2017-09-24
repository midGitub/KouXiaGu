using JiongXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Inputs
{

    /// <summary>
    /// 按键映射文件获取器;
    /// </summary>
    public class KeyMapFileSearcher : FileSearcher
    {
        public const string UserKeyConfigName = "Configs/KeyboardInput";
        public const string DefaultKeyConfigName = "Configs/DefaultKeyboardInput";

        /// <summary>
        /// 获取到按键配置文件路径,优先从用户配置文件夹内获取,若不存在则使用默认按键映射,并且拷贝一份到用户配置文件内;
        /// </summary>
        public override IEnumerable<string> Searche(string fileExtension)
        {
            string userKeyConfigPath = GetUserKeyConfigPath(fileExtension);
            if (File.Exists(userKeyConfigPath))
            {
                yield return userKeyConfigPath;
                yield break;
            }

            string defaultKeyConfigPath = GetDefaultKeyConfigPath(fileExtension);
            if (File.Exists(defaultKeyConfigPath))
            {
                File.Copy(defaultKeyConfigPath, userKeyConfigPath);
                yield return defaultKeyConfigPath;
                yield break;
            }
        }

        /// <summary>
        /// 返回用户配置文件路径;
        /// </summary>
        public string GetUserKeyConfigPath(string fileExtension)
        {
            return Path.Combine(Resource.UserConfigDirectoryPath, UserKeyConfigName, fileExtension);
        }

        /// <summary>
        /// 预定义的默认按键路径;
        /// </summary>
        public string GetDefaultKeyConfigPath(string fileExtension)
        {
            return Path.Combine(Resource.UserConfigDirectoryPath, UserKeyConfigName, fileExtension);
        }

        /// <summary>
        /// 总是返回用户配置文件路径;
        /// </summary>
        public override string GetWrite(string fileExtension)
        {
            return GetUserKeyConfigPath(fileExtension);
        }
    }

    /// <summary>
    /// 按键映射序列化;
    /// </summary>
    public class KeyMapSerializer : ResourceSerializer<CustomKey[], KeyMap>
    {
        public KeyMapSerializer(ISerializer<CustomKey[]> serializer, KeyMapFileSearcher resourceSearcher) : base(serializer, resourceSearcher)
        {
        }

        public KeyMapFileSearcher Searcher { get; private set; }

        protected override KeyMap Combine(List<CustomKey[]> sources)
        {
            throw new NotImplementedException();
        }

        protected override CustomKey[] Convert(KeyMap result)
        {
            throw new NotImplementedException();
        }
    }
}
