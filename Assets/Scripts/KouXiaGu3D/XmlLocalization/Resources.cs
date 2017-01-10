using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.XmlLocalization
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
        /// 获取到这个语言包的读取接口;
        /// </summary>
        public static ITextReader GetReader(XmlLanguageFile pack)
        {
            return new XmlTextReader(pack);
        }


        /// <summary>
        /// 获取到定义的所有语言包;
        /// </summary>
        public static IEnumerable<XmlLanguageFile> FindLanguageFiles()
        {
            return FindLanguageFiles(ResDirectoryPath, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// 获取到文件夹之下的所有语言文件;
        /// </summary>
        public static IEnumerable<XmlLanguageFile> FindLanguageFiles(string directoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return XmlFile.GetPacks(ResDirectoryPath, searchOption);
        }

    }

}
