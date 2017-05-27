using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 单个文件路径;
    /// </summary>
    public abstract class SingleFilePath
    {
        /// <summary>
        /// 文件的位于配置目录下的文件名;
        /// </summary>
        public abstract string FileName { get; }

        /// <summary>
        /// 获取到文件路径;
        /// </summary>
        public string GetFullPath()
        {
            return Path.Combine(Resource.ConfigDirectoryPath, FileName);
        }
    }

    /// <summary>
    /// 表示存在多个文件,以编号区分;
    /// </summary>
    public abstract class MultipleFilePath : SingleFilePath
    {
        public IEnumerable<string> FindFiles()
        {
            string fullPath = GetFullPath();
            string directory = Path.GetDirectoryName(fullPath);

            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            string fileExtension = Path.GetExtension(fullPath);
            string searchPattern = fileName + "*" + fileExtension;
            string[] paths = Directory.GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);

            return paths;
        }
    }
}
