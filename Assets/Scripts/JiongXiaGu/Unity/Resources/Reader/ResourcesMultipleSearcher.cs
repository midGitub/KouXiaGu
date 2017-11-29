using System.Collections.Generic;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 游戏数据目录文件搜寻器,相同目录多个不同命名的资源文件获取;
    /// </summary>
    public class ResourcesMultipleSearcher : ResourcesSearcher
    {
        public ResourcesMultipleSearcher(string resourceName)
        {
            ResourceName = resourceName;
            SearchOption = SearchOption.TopDirectoryOnly;
        }

        public string ResourceName { get; private set; }
        public SearchOption SearchOption { get; private set; }

        /// <summary>
        /// 获取到对应资源的文件路径;
        /// </summary>
        public override IEnumerable<string> Searche(string fileExtension)
        {
            return Searche(fileExtension, ResourcePath.CoreDirectory);
        }

        /// <summary>
        /// 获取到对应资源的文件路径;
        /// </summary>
        IEnumerable<string> Searche(string fileExtension, string directory)
        {
            string fullPath = Path.Combine(directory, ResourceName);
            string directoryPath = Path.GetDirectoryName(fullPath);
            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            string searchPattern = fileName + "*" + fileExtension;
            return Directory.EnumerateFiles(directoryPath, searchPattern, SearchOption);
        }

        /// <summary>
        /// 获取到用于输出的文件路径(注意!此方法每次都返回新的未创建的文件路径);
        /// </summary>
        public override string GetWrite(string fileExtension)
        {
            return GetWrite(fileExtension, ResourcePath.CoreDirectory);
        }

        string GetWrite(string fileExtension, string directory)
        {
            string path = GetWritePath(fileExtension, directory);
            return path;
        }

        string GetWritePath(string fileExtension, string directory)
        {
            string path = Path.Combine(directory, ResourceName + fileExtension);
            int i = 0;
            while (File.Exists(path))
            {
                path = Path.Combine(directory, ResourceName + "_" + i.ToString() + fileExtension);
                i++;
            }
            return path;
        }

        /// <summary>
        /// 转换为模版输出;
        /// </summary>
        public override ResourcesSearcher AsTemplateResourceSearcher()
        {
            const string template = "_Template";
            if (!ResourceName.EndsWith(template))
            {
                string resName = ResourceName + template;
                return new ResourcesSingleSearcher(resName);
            }
            return this;
        }
    }
}
