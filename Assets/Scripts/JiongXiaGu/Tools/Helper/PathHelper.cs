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
        /// 例子: absolutePath : C:\112233\, relativeTo: C:\112233\1.txt, return: 1.txt;
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
        /// 路径 child 是在 parent 之下?
        /// </summary>
        public static bool IsCommonRoot(string parent, string child)
        {
            parent = Normalize(parent);
            if (parent.EndsWith(DirectorySeparatorSrting))
            {
                parent = parent.Remove(parent.Length - 1);
            }

            child = Normalize(child);

            string[] parentDirectories = parent.Split(DirectorySeparatorChar);
            string[] childDirectories = child.Split(DirectorySeparatorChar);

            if (parentDirectories.Length > childDirectories.Length)
            {
                return false;
            }

            for (int index = 0; index < parentDirectories.Length; index++)
            {
                if (parentDirectories[index] != childDirectories[index])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 统一目录分隔符 为 "\";
        /// </summary>
        public static string Normalize(string path)
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

        /// <summary>
        /// 确认不表示一条路径;
        /// </summary>
        public static bool NonPath(string fileName)
        {
            int index = fileName.IndexOfAny(DirectorySeparatorChars);
            if (index >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查该字符串是否符合要求,仅支持 "?" 和 "*";
        /// </summary>
        public static bool IsMatch(string input, string pattern)
        {
            input = input.ToLower();
            pattern = pattern.ToLower();

            bool matched = false;
            int inputIndex = 0;
            int patternIndex = 0;

            //无通配符 * 时，比较算法（）
            while (inputIndex < input.Length && patternIndex < pattern.Length && (pattern[patternIndex] != '*'))
            {
                if ((pattern[patternIndex] != '?') && (input[inputIndex] != pattern[patternIndex]))
                {
                    //如果模式字符不是通配符，且输入字符与模式字符不相等，则可判定整个输入字串与模式不匹配
                    return matched;
                }
                patternIndex++;
                inputIndex++;
                if (patternIndex == pattern.Length && inputIndex < input.Length)
                {
                    return matched;
                }
                if (inputIndex == input.Length && patternIndex < pattern.Length)
                {
                    return matched;
                }
                if (patternIndex == pattern.Length && inputIndex == input.Length)
                {
                    matched = true;
                    return matched;
                }
            }

            //有通配符 * 时，比较算法
            int mp = 0;
            int cp = 0;
            while (inputIndex < input.Length)
            {
                if (patternIndex < pattern.Length && pattern[patternIndex] == '*')
                {
                    if (++patternIndex >= pattern.Length)
                    {
                        matched = true;
                        return matched;
                    }
                    mp = patternIndex;
                    cp = inputIndex + 1;
                }
                else if (patternIndex < pattern.Length && ((pattern[patternIndex] == input[inputIndex]) || (pattern[patternIndex] == '?')))
                {
                    patternIndex++;
                    inputIndex++;
                }
                else
                {
                    patternIndex = mp;
                    inputIndex = cp++;
                }
            }

            //当输入字符为空且模式为*时
            while (patternIndex < pattern.Length && pattern[patternIndex] == '*')
            {
                patternIndex++;
            }

            return patternIndex >= pattern.Length ? true : false;
        }
    }
}
