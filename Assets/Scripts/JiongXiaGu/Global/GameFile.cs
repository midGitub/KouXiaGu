using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu
{


    public static class GameFile
    {
        /// <summary>
        /// 游戏文件路径;
        /// </summary>
        public static List<string> GameDirectorys = new List<string>()
        {
            Resource.DataDirectoryPath,
        };
        
        public static string MainDirectory
        {
            get { return Resource.DataDirectoryPath; }
        }
    }


    /// <summary>
    /// 游戏路径
    /// </summary>
    public abstract class GameDirectoryPath
    {
        public GameDirectoryPath()
        {
        }

        public abstract string DirectoryName { get; }

        public string MainDirectoryPath
        {
            get { return Path.Combine(GameFile.MainDirectory, DirectoryName); }
        }
    }


    public abstract class ArchiveFilePath
    {
        public ArchiveFilePath(string fileExtension)
        {
            FileExtension = fileExtension;
        }

        public abstract string FileName { get; }
        public string FileExtension { get; private set; }

        public string GetFilePath(string archiveDir)
        {
            string path = Path.Combine(archiveDir, FileName);
            path = Path.ChangeExtension(path, FileExtension);
            return path;
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
        public virtual IEnumerable<string> GetFilePaths()
        {
            foreach (var dir in GameFile.GameDirectorys)
            {
                yield return Path.Combine(dir, FileName);
            }
        }
    }

}
