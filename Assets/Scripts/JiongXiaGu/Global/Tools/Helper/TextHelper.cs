using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Collections;

namespace JiongXiaGu
{

    /// <summary>
    /// 用于文本生成的拓展方法;
    /// </summary>
    public static class TextHelper
    {
        public const string NullString = "Null";
        private const bool defaultAddNewLine = true;

        /// <summary>
        /// 返回所有元素内容的 StringBuilder;
        /// </summary>
        public static StringBuilder ToText<T>(this IEnumerable<T> collection, bool addNewLine = defaultAddNewLine)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            Func<T, string> getString = item => item != null ? item.ToString() : NullString;
            StringBuilder stringBuilder = new StringBuilder();
            return ToText_internal(collection, stringBuilder, getString, addNewLine);
        }

        /// <summary>
        /// 返回所有元素内容的 StringBuilder;
        /// </summary>
        public static StringBuilder ToText<T>(this IEnumerable<T> collection, Func<T, string> getString, bool addNewLine = defaultAddNewLine)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (getString == null)
                throw new ArgumentNullException(nameof(getString));

            StringBuilder stringBuilder = new StringBuilder();
            return ToText_internal(collection, stringBuilder, getString, addNewLine);
        }

        /// <summary>
        /// 将所有元素内容按顺序加入到 StringBuilder;
        /// </summary>
        public static StringBuilder ToText<T>(this IEnumerable<T> collection, StringBuilder stringBuilder, Func<T, string> getString, bool addNewLine = defaultAddNewLine)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (stringBuilder == null)
                throw new ArgumentNullException(nameof(stringBuilder));
            if (getString == null)
                throw new ArgumentNullException(nameof(getString));

            return ToText_internal(collection, stringBuilder, getString, addNewLine);
        }

        /// <summary>
        /// 将所有元素内容按顺序加入到 StringBuilder;
        /// </summary>
        private static StringBuilder ToText_internal<T>(IEnumerable<T> collection, StringBuilder stringBuilder, Func<T, string> getString, bool addNewLine)
        {
            int index = 0;
            foreach (var item in collection)
            {
                string itemString;
                try
                {
                    itemString = getString(item);
                }
                catch (Exception ex)
                {
                    itemString = "Error:" + ex.ToString();
                }
                stringBuilder.AppendFormat("[{0}]{1};", index, itemString);

                if (addNewLine)
                {
                    stringBuilder.AppendLine();
                }

                index++;
            }
            return stringBuilder;
        }
    }
}
