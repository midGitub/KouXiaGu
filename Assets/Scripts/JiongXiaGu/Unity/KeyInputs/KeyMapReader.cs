using System;
using JiongXiaGu.Collections;
using System.Linq;

namespace JiongXiaGu.Unity.KeyInputs
{

    /// <summary>
    /// 按键配置读写器;
    /// </summary>
    public class KeyMapReader
    {
        public DefaultKeyConfigReader DefaultKeyConfigReader { get; private set; }
        public UserKeyConfigReader UserKeyConfigReader { get; private set; }

        public KeyMapReader()
        {
            DefaultKeyConfigReader = new DefaultKeyConfigReader();
            UserKeyConfigReader = new UserKeyConfigReader();
        }

        /// <summary>
        /// 读取到按键映射;
        /// </summary>
        public KeyMap Read()
        {
            KeyInfo[] defaultKeys = DefaultKeyConfigReader.Read();
            var keyMap = new KeyMap();

            try
            {
                KeyInfo[] userKeys = UserKeyConfigReader.Read();
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
        private KeyMap Combine(KeyInfo[] defaultKeys, KeyInfo[] userKeys)
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
        public void Write(KeyMap keyMap)
        {
            var keyInfos = keyMap.ToArray(pair => new KeyInfo(pair.Key, pair.Value));
            UserKeyConfigReader.Write(keyInfos);
        }
    }
}
