﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Resources
{

    public interface IFilePath
    {
        /// <summary>
        /// 主要存放的文件路径,若需要输出的路径;
        /// </summary>
        string GetMainPath();

        /// <summary>
        /// 获取到所有文件路径;
        /// </summary>
        IEnumerable<string> GetExistentPaths();
    }

    /// <summary>
    /// 单个文件路径;
    /// </summary>
    public abstract class SingleFilePath : IFilePath
    {
        /// <summary>
        /// 文件的位于配置目录下的文件名;
        /// </summary>
        public abstract string FileName { get; }

        /// <summary>
        /// 获取到文件路径;
        /// </summary>
        public string GetMainPath()
        {
            return Path.Combine(Resource.ConfigDirectoryPath, FileName);
        }

        public virtual IEnumerable<string> GetExistentPaths()
        {
            string filePath = GetMainPath();
            if (File.Exists(filePath))
            {
                yield return filePath;
            }
        }
    }

    /// <summary>
    /// 表示存在多个文件,搜索路径下"定义文件名 + 零个或多个字符 + 拓展名",
    /// 所以定义的时候要确保目录下不存在其它类型,相同命名方式的其它文件;
    /// </summary>
    public abstract class MultipleFilePath : SingleFilePath, IFilePath
    {
        public override IEnumerable<string> GetExistentPaths()
        {
            string fullPath = GetMainPath();
            string directory = Path.GetDirectoryName(fullPath);

            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            string fileExtension = Path.GetExtension(fullPath);
            string searchPattern = fileName + "*" + fileExtension;
            string[] paths = Directory.GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);

            return paths;
        }
    }
}
