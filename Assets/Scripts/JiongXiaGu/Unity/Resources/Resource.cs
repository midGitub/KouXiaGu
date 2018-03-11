using System;
using System.IO;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源路径定义;
    /// </summary>
    public static class Resource
    {

        public static string StreamingAssetsPath => streamingAssetsPath.Value;

        private static readonly Lazy<string> streamingAssetsPath = new Lazy<string>(delegate ()
        {
            return Application.streamingAssetsPath;
        }, true);



        /// <summary>
        /// 存放配置文件的文件夹;
        /// </summary>
        public static string ConfigDirectory => configDirectory.Value;

        /// <summary>
        /// 存放配置文件的文件夹;
        /// </summary>
        public static DirectoryContent ConfigContent => configContent.Value;

        private static readonly Lazy<string> configDirectory = new Lazy<string>(delegate ()
        {
            string directory = Path.Combine(StreamingAssetsPath, "Configs");
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException();
            }
            return directory;
        }, true);

        private static readonly Lazy<DirectoryContent> configContent = new Lazy<DirectoryContent>(() => new DirectoryContent(ConfigDirectory), true);



        /// <summary>
        /// 存放用户配置的文件夹;
        /// </summary>
        public static string UserConfigDirectory => userConfigDirectory.Value;

        /// <summary>
        /// 存放用户配置的文件夹;
        /// </summary>
        public static DirectoryContent UserConfigContent => userConfigContent.Value;

        private static readonly Lazy<string> userConfigDirectory = new Lazy<string>(delegate ()
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", Application.productName);
            Directory.CreateDirectory(directory);
            return directory;
        }, true);

        private static readonly Lazy<DirectoryContent> userConfigContent = new Lazy<DirectoryContent>(() => new DirectoryContent(UserConfigDirectory), true);



        /// <summary>
        /// 存放存档的文件夹路径;
        /// </summary>
        public static string ArchiveDirectory => archiveDirectory.Value;

        private static readonly Lazy<string> archiveDirectory = new Lazy<string>(delegate ()
        {
            string directory = Path.Combine(UserConfigDirectory, "Save");
            Directory.CreateDirectory(directory);
            return directory;
        }, true);



        /// <summary>
        /// 用户存放模组的文件夹;
        /// </summary>
        public static string UserModDirectory => userModDirectory.Value;

        private static readonly Lazy<string> userModDirectory = new Lazy<string>(delegate ()
        {
            string directory = Path.Combine(UserConfigDirectory, "MOD");
            Directory.CreateDirectory(directory);
            return directory;
        });



        /// <summary>
        /// 存放模组的文件夹;
        /// </summary>
        public static string ModDirectory => modDirectory.Value;

        private static Lazy<string> modDirectory = new Lazy<string>(delegate ()
        {
            string directory = Path.Combine(StreamingAssetsPath, "MOD");
            Directory.CreateDirectory(directory);
            return directory;
        }, true);



        /// <summary>
        /// 初始化路径信息(仅在Unity线程调用);
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeOnLoad()
        {
            streamingAssetsPath.Create();
            configContent.Create();
            userConfigContent.Create();
            archiveDirectory.Create();
            userModDirectory.Create();
            modDirectory.Create();
        }
    }
}
