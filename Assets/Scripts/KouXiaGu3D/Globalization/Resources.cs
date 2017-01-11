using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.Globalization
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
        public static string DirectoryPath
        {
            get { return Path.Combine(ResourcePath.ConfigurationDirectoryPath, RES_DIRECTORY); }
        }

        /// <summary>
        /// 获取到定义的所有语言包;
        /// </summary>
        public static IEnumerable<LanguageFile> FindLanguageFiles()
        {
            return FindLanguageFiles(DirectoryPath, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// 获取到文件夹之下的所有语言文件;
        /// </summary>
        public static IEnumerable<LanguageFile> FindLanguageFiles(string directoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return XmlFile.GetPacks(DirectoryPath, searchOption);
        }

    }

}
