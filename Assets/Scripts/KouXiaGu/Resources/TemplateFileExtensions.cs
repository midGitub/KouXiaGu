using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 转换模版文件路径;
    /// </summary>
    public class TemplateFilePath : ISingleFilePath
    {
        public TemplateFilePath(ISingleFilePath file)
        {
            File = file;
        }

        public ISingleFilePath File { get; private set; }

        public string GetFullPath()
        {
            string path = File.GetFullPath();
            path = TemplateFileExtensions.ChangePath(path);
            return path;
        }
    }

    /// <summary>
    /// 转换模版文件路径;
    /// </summary>
    public class TemplateFilePaths : IMultipleFilePath
    {
        public TemplateFilePaths(IMultipleFilePath file)
        {
            File = file;
        }

        public IMultipleFilePath File { get; private set; }

        public string CreateFilePath(string name)
        {
            string path = File.CreateFilePath(name);
            path = TemplateFileExtensions.ChangePath(path);
            return path;
        }

        public IEnumerable<string> GetExistentPaths()
        {
            IEnumerable<string> paths = File.GetExistentPaths();
            return paths.Select(path => TemplateFileExtensions.ChangePath(path));
        }
    }


    public static class TemplateFileExtensions
    {
        const string TemplatePrefix = "_Template_";

        public static string ChangePath(string path)
        {
            string fileName = Path.GetFileName(path);
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            path = Path.Combine(directoryName, TemplatePrefix + fileName);
            return path;
        }

        /// <summary>
        /// 将原本实例转换成模版输出接口;
        /// </summary>
        public static IWriter<T> ToTemplateWriter<T>(this IFileReaderWriter<T> serializer)
        {
            ISingleFilePath originalFile = serializer.FilePath;
            serializer.FilePath = new TemplateFilePath(originalFile);
            return serializer;
        }

        /// <summary>
        /// 将原本实例转换成模版输出接口;
        /// </summary>
        public static IWriter<T> ToTemplateWriter<T>(this IFilesReaderWriter<T> serializer)
        {
            IMultipleFilePath originalFile = serializer.FilePath;
            serializer.FilePath = new TemplateFilePaths(originalFile);
            return serializer;
        }
    }
}
