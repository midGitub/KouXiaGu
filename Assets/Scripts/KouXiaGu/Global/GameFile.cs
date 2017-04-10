using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KouXiaGu
{


    static class GameFile
    {

        /// <summary>
        /// 游戏文件路径;
        /// </summary>
        public static List<string> GameDirectorys = new List<string>()
        {
            ResourcePath.ConfigDirectoryPath,
        };
        
    }

    public interface ICustomFile
    {
        IEnumerable<string> GetFilePaths();
    }

    abstract class CustomFile : ICustomFile
    {
        public abstract string FileName { get; }

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
