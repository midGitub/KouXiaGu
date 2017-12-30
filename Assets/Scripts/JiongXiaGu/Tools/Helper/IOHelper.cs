using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu
{

    /// <summary>
    /// 对命名空间 System.IO 方法的拓展;
    /// </summary>
    public static class IOHelper
    {

        /// <summary>
        /// 尝试删除目录,若目录存在并且删除成功则返回true,否则返回false;
        /// </summary>
        public static bool TryDeleteDirectory(string directory)
        {
            try
            {
                Directory.Delete(directory, true);
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// 尝试删除文件,若文件存在且删除成功则返回true,否则返回false;
        /// </summary>
        public static bool TryDeleteFile(string file)
        {
            try
            {
                File.Delete(file);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// 若文件不存在,则弹出异常 DirectoryNotFoundException;
        /// </summary>
        public static void ThrowIfFileNotExisted(this FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException(string.Format("文件不存在[{0}]", fileInfo.FullName));
            }
        }

        /// <summary>
        /// 若目录不存在,则弹出异常 DirectoryNotFoundException;
        /// </summary>
        public static void ThrowIfDirectoryNotExisted(this DirectoryInfo directoryInfo)
        {
            if (!directoryInfo.Exists)
            {
                throw new DirectoryNotFoundException(string.Format("目录不存在[{0}]", directoryInfo.FullName));
            }
        }
    }
}
