using System.IO;


namespace XGame
{

    /// <summary>
    /// 资源信息;
    /// </summary>
    public class ModInfo
    {

        public ModInfo(string name, string path)
        {
            this.Name = name;
            this.ModPath = path;
        }

        /// <summary>
        /// Mod名;
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Mod目录;
        /// </summary>
        public string ModPath { get; private set; }

        /// <summary>
        /// 获取到完整的路径;
        /// </summary>
        /// <param name="afterModPath">在模组路径之后的路径;</param>
        /// <param name="fullPath">返回完整路径;</param>
        /// <returns>若路径存在则返回true,否则返回false;</returns>
        public bool PathCombine(string afterModPath, out string fullPath)
        {
            fullPath = Path.Combine(ModPath, afterModPath);
            return File.Exists(fullPath);
        }

    }


}
