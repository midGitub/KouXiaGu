using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Localizations
{


    public class Resources
    {

        /// <summary>
        /// 语言包存放的文件夹;
        /// </summary>
        const string RES_DIRECTORY = "Localization";

        const string DESCRIPTION_NAME = "Localization.xml";

        public static string ResPath
        {
            get { return Path.Combine(ResourcePath.ConfigurationDirectoryPath, RES_DIRECTORY); }
        }

        public static string ConfigFilePath
        {
            get { return Path.Combine(ResPath, DESCRIPTION_NAME); }
        }

        public static LocalizationConfig ReadConfig()
        {
            var descr = (LocalizationConfig)LocalizationConfig.Serializer.DeserializeXiaGu(ConfigFilePath);
            return descr;
        }

        public static void WriteConfig(LocalizationConfig config)
        {
            LocalizationConfig.Serializer.SerializeXiaGu(ConfigFilePath, config);
        }


        /// <summary>
        /// 按数组从 0 到 max 的顺序获取到对应的文本读取接口;
        /// </summary>
        public static IEnumerable<ITextReader> GetReader(IEnumerable<string> languages)
        {
            foreach (var directory in SearchDirectorys())
            {
                ITextReader reader;
                if (TryGetReader(directory, out reader, languages.ToArray()))
                    yield return reader;
            }
        }

        public static bool TryGetReader(string directoryPath, out ITextReader reader, params string[] languages)
        {
            int level = int.MaxValue;
            XmlLanguagePack result = null;
            IEnumerable<XmlLanguagePack> packs = GetLanguagePacks(directoryPath);

            foreach (var pack in packs)
            {
                for (int i = 0; i < languages.Length; i++)
                {
                    if (level > i && languages[i] == pack.Language)
                    {
                        result = pack;
                        level = i;
                        break;
                    }
                }
            }

            if (result == null)
            {
                reader = null;
                Debug.LogWarning("无法找到目录之下的语言包:" + directoryPath + "\n请求:" + languages.ToLog());
                return false;
            }
            else
            {
                reader = GetReader(result);
                return true;
            }
        }


        /// <summary>
        /// 获取到需要读取语言文件的路径;
        /// </summary>
        static IEnumerable<string> SearchDirectorys()
        {
            yield return ResPath;
        }

        /// <summary>
        /// 获取到文件夹之下的所有语言文件;
        /// </summary>
        public static IEnumerable<XmlLanguagePack> GetLanguagePacks(string directoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return XmlFiler.GetPacks(ResPath, searchOption);
        }

        static ITextReader GetReader(XmlLanguagePack pack)
        {
            return new XmlTextReader(pack);
        }

    }

}
