using JiongXiaGu.Collections;
using JiongXiaGu.Unity.Resources;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JiongXiaGu.Unity.KeyInputs
{


    public class KeyMapFactroy
    {
        private const string ConfigFileName = "Configs/KeyboardInput.xml";
        private readonly XmlSerializer<List<KeyInfo>> serializer = new XmlSerializer<List<KeyInfo>>();

        public Dictionary<string, KeyCode> ReadKeyMap()
        {
            List<KeyInfo> keyInfos = Read();
            Dictionary<string, KeyCode> dictionary = new Dictionary<string, KeyCode>();

            foreach (var keyInfo in keyInfos)
            {
                dictionary.AddOrUpdate(keyInfo.Name, keyInfo.Key);
            }

            return dictionary;
        }

        /// <summary>
        /// 获取到当前使用的按键映射;
        /// </summary>
        public List<KeyInfo> Read()
        {
            try
            {
                return ReadUserKey();
            }
            catch
            {
                return ReadDefaultKey();
            }
        }

        public List<KeyInfo> ReadDefaultKey()
        {
            string path = Path.Combine(Resource.ConfigDirectory, ConfigFileName);

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return serializer.Deserialize(stream);
            }
        }

        public List<KeyInfo> ReadUserKey()
        {
            string path = Path.Combine(Resource.UserConfigDirectory, ConfigFileName);

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return serializer.Deserialize(stream);
            }
        }
    }
}
