using System.Collections.Generic;

namespace KouXiaGu.Localizations
{


    /// <summary>
    /// 记录缺少的文本;
    /// </summary>
    public static class LackingTextCollecter
    {

        static readonly HashSet<string> lackingKeys = new HashSet<string>();

        public static IEnumerable<string> LackingKeys
        {
            get { return lackingKeys; }
        }

        public static void Collecting(string key)
        {
            lackingKeys.Add(key);
        }

        /// <summary>
        /// 将所有Key输出到文件;
        /// </summary>
        public static void WriteXml(string filePath)
        {

        }

    }

}
