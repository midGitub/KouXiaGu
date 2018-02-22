using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu
{


    public static class TagHelper
    {
        /// <summary>
        /// 标签分隔符;
        /// </summary>
        public const char TagSeparatorChar = ',';
        public const string TagSeparatorString = ", ";
        internal static readonly char[] TagSeparatorCharArray = new char[] { TagSeparatorChar };

        /// <summary>
        /// 连接多个标签;
        /// </summary>
        public static string Combine(params string[] tags)
        {
            return Combine(tags as IEnumerable<string>);
        }

        /// <summary>
        /// 连接多个标签;
        /// </summary>
        public static string Combine(IEnumerable<string> tags)
        {
            string value = string.Join(TagSeparatorString, tags);
            return value;
        }

        /// <summary>
        /// 将标签语句分离为多个标签;
        /// </summary>
        public static string[] GetTags(string tags)
        {
            return tags.Split(TagSeparatorCharArray, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
