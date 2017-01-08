using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.Localizations
{

    /// <summary>
    /// 负责提供语言读取方式和;
    /// </summary>
    [DisallowMultipleComponent]
    public static class Resources
    {

        /// <summary>
        /// 语言包存放的文件夹;
        /// </summary>
        const string RES_DIRECTORY = "Localization";

        /// <summary>
        /// 主语言包们存放的文件夹;
        /// </summary>
        public static string ResDirectoryPath
        {
            get { return Path.Combine(ResourcePath.ConfigurationDirectoryPath, RES_DIRECTORY); }
        }


        /// <summary>
        /// 获取到主目录下优先级最高的语言读取接口;
        /// </summary>
        public static ITextReader GetReader(params string[] languages)
        {
            ITextReader reader;
            if (TryGetReader(ResDirectoryPath, out reader, languages))
            {
                return reader;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        /// <summary>
        /// 获取到目录下的优先级最高的语言读取接口;
        /// </summary>
        public static IEnumerable<ITextReader> GetReader(IEnumerable<string> directorys, params string[] languages)
        {
            ITextReader reader;
            foreach (var directory in directorys)
            {
                if (TryGetReader(directory, out reader, languages))
                    yield return reader;
                else
                    Debug.LogWarning("无法找到目录之下的语言包:" + directory + "\n请求:" + languages.ToLog());
            }
        }

        /// <summary>
        /// 获取到目录下的优先级最高的语言读取接口;
        /// </summary>
        public static bool TryGetReader(string directoryPath, out ITextReader reader, params string[] languages)
        {
            int level = int.MaxValue;
            XmlLanguageFile result = null;
            IEnumerable<XmlLanguageFile> packs = GetLanguagePacks(directoryPath);

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
                return false;
            }
            else
            {
                reader = GetReader(result);
                return true;
            }
        }


        /// <summary>
        /// 获取到文件夹之下的所有语言文件;
        /// </summary>
        public static IEnumerable<XmlLanguageFile> GetLanguagePacks(string directoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return XmlFiler.GetPacks(ResDirectoryPath, searchOption);
        }

        /// <summary>
        /// 获取到这个语言包的读取接口;
        /// </summary>
        public static ITextReader GetReader(XmlLanguageFile pack)
        {
            return new XmlTextReader(pack);
        }

    }

}
