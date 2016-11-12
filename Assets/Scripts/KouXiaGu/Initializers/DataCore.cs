using System;
using System.IO;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏
    /// </summary>
    [Serializable]
    public class DataCore : ICoreDataResource
    {
        private DataCore() { }

        [SerializeField, Tooltip("核心文件目录")]
        private string coreDataDirectory;

        public string GetCoreDataDirectoryPath()
        {
            string coreDataDirectoryPath = Path.Combine(Application.dataPath, coreDataDirectory);
            return coreDataDirectoryPath;
        }

    }

    /// <summary>
    /// 新游戏创建资源;
    /// </summary>
    public interface ICoreDataResource
    {
        /// <summary>
        /// 存放游戏核心内容的目录;
        /// </summary>
        string GetCoreDataDirectoryPath();
    }

}
