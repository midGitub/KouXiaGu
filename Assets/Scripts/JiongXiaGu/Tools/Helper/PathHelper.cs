using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu
{


    public static class PathHelper
    {

        /// <summary>
        /// 路径字符串中的分隔符;
        /// </summary>
        public const char DirectorySeparatorChar = '\\';

        /// <summary>
        /// 路径字符串中的分隔符;
        /// </summary>
        public const string DirectorySeparatorSrting = "\\";

        /// <summary>
        /// 路径字符串中的分隔符(只读);
        /// </summary>
        internal static char[] DirectorySeparatorChars = new char[] { '/', '\\' };

        /// <summary>
        /// 获取到相对路径;参考 Uri.MakeRelativeUri 方法;
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <param name="relativeTo">要对比的路径</param>
        public static string GetRelativePath(string absolutePath, string relativeTo)
        {
            if (string.IsNullOrWhiteSpace(absolutePath))
                throw new ArgumentNullException(nameof(absolutePath));
            if (string.IsNullOrWhiteSpace(relativeTo))
                throw new ArgumentNullException(nameof(relativeTo));

            absolutePath = Normalize(absolutePath);
            if (absolutePath.EndsWith(DirectorySeparatorSrting))
            {
                absolutePath = absolutePath.Remove(absolutePath.Length - 1);
            }

            relativeTo = Normalize(relativeTo);

            string[] absoluteDirectories = absolutePath.Split(DirectorySeparatorChar);
            string[] relativeDirectories = relativeTo.Split(DirectorySeparatorChar);

            if (absoluteDirectories.Length > relativeDirectories.Length)
            {
                throw new ArgumentException("路径不为同一个目录");
            }

            //检查路径是否在同一个目录;
            for (int index = 0; index < absoluteDirectories.Length; index++)
            {
                if (absoluteDirectories[index] != relativeDirectories[index])
                {
                    throw new ArgumentException("路径不为同一个目录");
                }
            }

            if (absoluteDirectories.Length == relativeDirectories.Length)
            {
                return string.Empty;
            }

            StringBuilder relativePath = new StringBuilder();

            for (int index = absoluteDirectories.Length; index < relativeDirectories.Length - 1; index++)
            {
                relativePath.Append(relativeDirectories[index] + DirectorySeparatorSrting);
            }

            relativePath.Append(relativeDirectories[relativeDirectories.Length - 1]);

            return relativePath.ToString();
        }

        /// <summary>
        /// 统一目录分隔符 为 "\";
        /// </summary>
        private static string Normalize(string path)
        {
            path = path.Replace('/', DirectorySeparatorChar);
            return path;
        }

        /// <summary>
        /// 获取到文件名;若路径以目录分隔符结尾则返回异常;
        /// 例: path : "12345/jjjwww\\2.txt, return : 2.txt;"
        /// 例: path : "12345/jjjwww\\2.txt\\/, throw : ArgumentException;"
        /// </summary>
        public static string GetFileName(string path)
        {
            int i = path.LastIndexOfAny(DirectorySeparatorChars);
            if (i == path.Length - 1)
            {
                throw new ArgumentException(string.Format("路径[{0}]不为文件路径", path));
            }
            else
            {
                path = path.Remove(0, i + 1);
                return path;
            }
        }
    }
}
