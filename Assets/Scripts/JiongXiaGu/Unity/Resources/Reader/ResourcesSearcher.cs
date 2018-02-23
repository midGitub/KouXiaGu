using System.Collections.Generic;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源文件搜寻器抽象类;
    /// </summary>
    public abstract class ResourcesSearcher
    {
        /// <summary>
        /// 获取到对应资源的文件路径(可能返回多个或者一个);
        /// </summary>
        public abstract IEnumerable<string> Searche(string fileExtension);

        /// <summary>
        /// 获取到对应资源的文件路径(可能返回多个或者一个);
        /// </summary>
        public IEnumerable<string> Searche<T>(ISerializer<T> serializer)
        {
            return Searche(".xml");
        }

        /// <summary>
        /// 获取到用于输出的文件路径;
        /// </summary>
        public abstract string GetWrite(string fileExtension);

        /// <summary>
        /// 获取到用于输出的文件路径;
        /// </summary>
        public string GetWrite<T>(ISerializer<T> serializer)
        {
            return GetWrite(".xml");
        }

        /// <summary>
        /// 提供一个模版文件搜寻器(若不存在则返回null);
        /// </summary>
        public virtual ResourcesSearcher AsTemplateResourceSearcher()
        {
            return null;
        }
    }
}
