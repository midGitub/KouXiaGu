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
        private const char directorySeparatorChar = '\\';

        /// <summary>
        /// 路径字符串中的分隔符;
        /// </summary>
        private const string directorySeparatorSrting = "\\";

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
            if (absolutePath.EndsWith(directorySeparatorSrting))
            {
                absolutePath = absolutePath.Remove(absolutePath.Length - 1);
            }

            relativeTo = Normalize(relativeTo);

            string[] absoluteDirectories = absolutePath.Split(directorySeparatorChar);
            string[] relativeDirectories = relativeTo.Split(directorySeparatorChar);

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
                relativePath.Append(relativeDirectories[index] + directorySeparatorSrting);
            }

            relativePath.Append(relativeDirectories[relativeDirectories.Length - 1]);

            return relativePath.ToString();
        }

        /// <summary>
        /// 统一目录分隔符 为 "\";
        /// </summary>
        private static string Normalize(string path)
        {
            path = path.Replace('/', directorySeparatorChar);
            return path;
        }

        ///// <summary>
        ///// 获取到相对路径;
        ///// </summary>
        ///// <param name="absolutePath">绝对路径</param>
        ///// <param name="relativeTo">要对比的路径</param>
        //public static string GetRelativePath(string absolutePath, string relativeTo)
        //{
        //    string[] absoluteDirectories = absolutePath.Split('\\');
        //    string[] relativeDirectories = relativeTo.Split('\\');

        //    //Get the shortest of the two paths
        //    int length = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length : relativeDirectories.Length;

        //    //Use to determine where in the loop we exited
        //    int lastCommonRoot = -1;
        //    int index;

        //    //Find common root
        //    for (index = 0; index < length; index++)
        //        if (absoluteDirectories[index] == relativeDirectories[index])
        //            lastCommonRoot = index;
        //        else
        //            break;

        //    //If we didn't find a common prefix then throw
        //    if (lastCommonRoot == -1)
        //        throw new ArgumentException("Paths do not have a common base");

        //    //Build up the relative path
        //    StringBuilder relativePath = new StringBuilder();

        //    //Add on the ..
        //    for (index = lastCommonRoot + 1; index < absoluteDirectories.Length; index++)
        //        if (absoluteDirectories[index].Length > 0)
        //            relativePath.Append("..\\");

        //    //Add on the folders
        //    for (index = lastCommonRoot + 1; index < relativeDirectories.Length - 1; index++)
        //        relativePath.Append(relativeDirectories[index] + "\\");
        //    relativePath.Append(relativeDirectories[relativeDirectories.Length - 1]);

        //    return relativePath.ToString();
        //}
    }
}
