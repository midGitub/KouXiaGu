//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace KouXiaGu.Resources
//{


//    /// <summary>
//    /// 表示单个文件;
//    /// </summary>
//    public struct SingleConfigFileName
//    {
//        public SingleConfigFileName(string fileName)
//        {
//            FileName = fileName;
//        }

//        public string FileName { get; private set; }

//        /// <summary>
//        /// 获取到完成目录;
//        /// </summary>
//        public string GetFullPath(string directory)
//        {
//            string fullPath = Path.Combine(directory, FileName);
//            return fullPath;
//        }

//        /// <summary>
//        /// 确认是否存在该文件;
//        /// </summary>
//        public bool Exists(string directory, string extension)
//        {
//            string fullPath = Path.Combine(directory, FileName);
//            bool existse = File.Exists(fullPath);
//            return existse;
//        }

//        public static implicit operator SingleConfigFileName(string fileName)
//        {
//            return new SingleConfigFileName(fileName);
//        }

//        public static implicit operator string(SingleConfigFileName fileName)
//        {
//            return fileName.FileName;
//        }
//    }


//    /// <summary>
//    /// 表示多个文件;
//    /// </summary>
//    public struct MultipleConfigFileName
//    {
//        public MultipleConfigFileName(string fileName)
//        {
//            FileName = fileName;
//        }

//        public string FileName { get; private set; }

//        /// <summary>
//        /// 枚举存在的文件;
//        /// </summary>
//        public IEnumerable<string> EnumerateFiles(string directory, string extension, SearchOption searchOption)
//        {
//            string fullPath = Path.Combine(directory, FileName);
//            string directoryPath = Path.GetDirectoryName(fullPath);
//            string fileName = Path.GetFileNameWithoutExtension(fullPath);
//            string searchPattern = fileName + "*" + extension;
//            return Directory.EnumerateFiles(directory, searchPattern, searchOption);
//        }

//        public static implicit operator MultipleConfigFileName(string fileName)
//        {
//            return new MultipleConfigFileName(fileName);
//        }

//        public static implicit operator string(MultipleConfigFileName fileName)
//        {
//            return fileName.FileName;
//        }
//    }
//}
