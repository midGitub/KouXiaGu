using System.Collections.Generic;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 游戏数据目录文件搜寻器,目录只存在一个资源文件获取;
    /// </summary>
    public class ResourcesSingleSearcher : ResourcesSearcher
    {
        public ResourcesSingleSearcher(string resourceName)
        {
            ResourceName = resourceName;
        }

        public string ResourceName { get; private set; }

        /// <summary>
        /// 获取到对应资源的文件路径;
        /// </summary>
        public override IEnumerable<string> Searche(string fileExtension)
        {
            string filePath = GetFilePath(fileExtension);
            if (File.Exists(filePath))
            {
                yield return filePath;
            }
        }

        /// <summary>
        /// 获取到用于输出的文件路径;
        /// </summary>
        public override string GetWrite(string fileExtension)
        {
            string filePath = GetFilePath(fileExtension);
            return filePath;
        }

        string GetFilePath(string fileExtension)
        {
            string filePath = Path.Combine(Resource.ConfigDirectory, ResourceName + fileExtension);
            return filePath;
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
