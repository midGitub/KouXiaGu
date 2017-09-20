using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Resources
{

    public interface ISingleFilePath
    {
        /// <summary>
        /// 主要存放的文件路径,若需要输出的路径;
        /// </summary>
        string GetFullPath();
    }

    /// <summary>
    /// 单个文件路径;
    /// </summary>
    public abstract class SingleFilePath : ISingleFilePath
    {
        public SingleFilePath() : this(Resource.DataDirectoryPath)
        {
        }

        public SingleFilePath(string directoryPath)
        {
            DirectoryPath = directoryPath;
        }

        public string DirectoryPath { get; set; }

        /// <summary>
        /// 文件的位于配置目录下的文件名;
        /// </summary>
        public abstract string FileName { get; }

        /// <summary>
        /// 获取到文件路径;
        /// </summary>
        public virtual string GetFullPath()
        {
            string fullPath = Path.Combine(DirectoryPath, FileName);
            string directoryName = Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(directoryName);
            return fullPath;
        }
    }


    public interface IMultipleFilePath
    {
        /// <summary>
        /// 获取到所有文件路径;
        /// </summary>
        IEnumerable<string> GetExistentFilePaths();

        /// <summary>
        /// 根据 name,返回一个唯一的路径;
        /// </summary>
        string GetFilePath(string name);
    }

    /// <summary>
    /// 表示存在多个文件,搜索路径下"定义文件名 + 零个或多个字符 + 拓展名",
    /// 所以定义的时候要确保目录下不存在其它类型,相同命名方式的其它文件;
    /// </summary>
    public abstract class MultipleFilePath : IMultipleFilePath
    {
        public MultipleFilePath() : this(Resource.DataDirectoryPath)
        {
        }

        public MultipleFilePath(string directoryPath)
        {
            DirectoryPath = directoryPath;
            string fullPath = GetFullPath();
            string directoryName = Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(directoryName);
        }

        public string DirectoryPath { get; set; }

        /// <summary>
        /// 文件的位于配置目录下的文件名;
        /// </summary>
        public abstract string FileName { get; }

        /// <summary>
        /// 获取到文件路径;
        /// </summary>
        public string GetFullPath()
        {
            return Path.Combine(DirectoryPath, FileName);
        }

        /// <summary>
        /// 获取到所有文件路径;
        /// </summary>
        public IEnumerable<string> GetExistentFilePaths()
        {
            string fullPath = GetFullPath();
            string directory = Path.GetDirectoryName(fullPath);
            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            string fileExtension = Path.GetExtension(fullPath);
            string searchPattern = fileName + "*" + fileExtension;
            string[] paths = Directory.GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);
            return paths;
        }

        /// <summary>
        /// 根据 name,返回一个唯一的路径;
        /// </summary>
        public string GetFilePath(string name)
        {
            string fullPath = GetFullPath();
            if (string.IsNullOrEmpty(name))
            {
                return fullPath;
            }
            else
            {
                string directory = Path.GetDirectoryName(fullPath);
                string fileName = Path.GetFileNameWithoutExtension(fullPath);
                string fileExtension = Path.GetExtension(fullPath);
                string path = Path.Combine(directory, fileName + name + fileExtension);
                return path;
            }
        }
    }
}
