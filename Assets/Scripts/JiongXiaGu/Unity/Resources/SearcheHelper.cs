using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{


    public class SearcheHelper
    {
        /// <summary>
        /// 忽略符,置于名称前缀,用于忽略某文件/文件夹;
        /// </summary>
        public const string IgnoreSymbol = "#ignore_";

        /// <summary>
        /// 该名称是否符合忽略要求?
        /// </summary>
        public static bool IsIgnore(string name)
        {
            return name.StartsWith(IgnoreSymbol, StringComparison.OrdinalIgnoreCase);
        }
    }
}
