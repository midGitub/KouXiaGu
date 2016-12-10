using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UniRx;

namespace KouXiaGu
{

    public static class FileHelper
    {

        /// <summary>
        /// 将资源文件夹内的所有文件移动到目标文件夹内;
        /// </summary>
        public static void CopyDirectory(string sourceDirectoryPath, string destDirectoryPath, string fileSearchPattern, bool overwrite)
        {
            if (!Directory.Exists(sourceDirectoryPath))
                return;
            if (!Directory.Exists(destDirectoryPath))
                Directory.CreateDirectory(destDirectoryPath);

            string[] filePaths = Directory.GetFileSystemEntries(sourceDirectoryPath, fileSearchPattern);

            foreach (var filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                string sourceFilePath = Path.Combine(sourceDirectoryPath, fileName);
                string destFilePath = Path.Combine(destDirectoryPath, fileName);

                File.Copy(sourceFilePath, destFilePath, overwrite);
            }
        }

        /// <summary>
        /// 将资源文件夹内的所有文件移动到目标文件夹内;
        /// </summary>
        public static void CopyDirectory(ICancelable cancelable, string sourceDirectoryPath, string destDirectoryPath,
         string fileSearchPattern, bool overwrite)
        {
            if (!Directory.Exists(sourceDirectoryPath))
                return;
            if (!Directory.Exists(destDirectoryPath))
                Directory.CreateDirectory(destDirectoryPath);

            string[] filePaths = Directory.GetFileSystemEntries(sourceDirectoryPath, fileSearchPattern);

            foreach (var filePath in filePaths)
            {
                if (cancelable.IsDisposed)
                    return;

                string fileName = Path.GetFileName(filePath);
                string sourceFilePath = Path.Combine(sourceDirectoryPath, fileName);
                string destFilePath = Path.Combine(destDirectoryPath, fileName);

                File.Copy(sourceFilePath, destFilePath, overwrite);
            }
        }

        /// <summary>
        ///  删除文件夹内的部分文件;
        /// </summary>
        /// <param name="fullDirectoryPath"></param>
        /// <param name="fileSearchPattern">要与 path 中的文件和目录的名称匹配的搜索字符串</param>
        public static void DeleteFileInDirectory(string fullDirectoryPath, string fileSearchPattern)
        {
            if (!Directory.Exists(fullDirectoryPath))
                return;

            string[] filePaths = Directory.GetFileSystemEntries(fullDirectoryPath, fileSearchPattern);
            foreach (var mapFilePath in filePaths)
            {
                File.Delete(mapFilePath);
            }
        }

    }

}
