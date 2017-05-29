using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 转换模版文件路径;
    /// </summary>
    public class TemplateFilePath : IFilePath
    {
        public TemplateFilePath(IFilePath filePath)
        {
            File = filePath;
        }

        const string TemplatePrefix = "_Template_";
        public IFilePath File { get; private set; }

        public string GetMainPath()
        {
            string path = File.GetMainPath();
            path = ChangePath(path);
            return path;
        }

        public IEnumerable<string> GetExistentPaths()
        {
            IEnumerable<string> paths = File.GetExistentPaths();
            foreach (var path in paths)
            {
                yield return ChangePath(path);
            }
        }

        string ChangePath(string path)
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
    }


    public static class TemplateFileWriter
    {
        /// <summary>
        /// 转换成模版输出
        /// </summary>
        public static IWriter<T> ToTemplateWriter<T>(this ISerializer<T> serializer)
        {
            IFilePath originalFile = serializer.File;
            serializer.File = new TemplateFilePath(originalFile);
            return serializer;
        }
    }
}
