using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KouXiaGu
{


    public static class GameFile
    {

        /// <summary>
        /// 游戏文件路径;
        /// </summary>
        public static List<string> GameDirectorys = new List<string>()
        {
            ResourcePath.ConfigDirectoryPath,
        };
        
        public static string MainDirectory
        {
            get { return ResourcePath.ConfigDirectoryPath; }
        }

    }


    public abstract class FilePath
    {
        public FilePath(string fileExtension)
        {
            FileExtension = fileExtension;
        }

        public abstract string FileName { get; }
        public string FileExtension { get; private set; }

        public string MainFilePath
        {
            get
            {
                string path = Path.Combine(MainDirPath, FileName);
                path = Path.ChangeExtension(path, FileExtension);
                return path;
            }
        }

        public string MainDirPath
        {
            get { return GameFile.MainDirectory; }
        }

        public string Combine(string dirPath)
        {
            return Path.Combine(dirPath, FileName);
        }

        public bool Exists(string dirPath)
        {
            return File.Exists(MainFilePath);
        }

    }

    public abstract class CustomFilePath : FilePath
    {
        public CustomFilePath(string fileExtension) : base(fileExtension)
        {
        }

        public IEnumerable<string> GetDirPaths()
        {
            return GameFile.GameDirectorys;
        }

        /// <summary>
        /// 获取到文件路径;
        /// </summary>
        public IEnumerable<string> GetFilePaths()
        {
            foreach (var dir in GameFile.GameDirectorys)
            {
                yield return Path.Combine(dir, FileName);
            }
        }
    }

}
