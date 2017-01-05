using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Localizations
{

    /// <summary>
    /// 负责语言包的加载内容;
    /// </summary>
    [DisallowMultipleComponent]
    public class LocalizationRes : UnitySington<LocalizationRes>
    {
        LocalizationRes() { }

        /// <summary>
        /// 资源存放的文件夹;
        /// </summary>
        public const string ResDirectory = "Localization";

        public static string resPath
        {
            get { return Path.Combine(ResourcePath.ConfigurationDirectoryPath, ResDirectory); }
        }


    }

}
