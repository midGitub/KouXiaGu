using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JiongXiaGu.Resources
{


    /// <summary>
    /// 资源文件获取;
    /// </summary>
    public class ResourceSearcher
    {
        public ResourceSearcher(string resourceName)
        {
            ResourceName = resourceName;
        }

        public string ResourceName { get; private set; }

        /// <summary>
        /// 获取到对应资源的文件路径;
        /// </summary>
        public virtual IEnumerable<string> Searche<T>(ISerializer<T> serializer)
        {
            string filePath = GetFilePath(serializer);
            if (File.Exists(filePath))
            {
                yield return filePath;
            }
        }

        /// <summary>
        /// 获取到用于输出的文件路径;
        /// </summary>
        public virtual string GetWrite<T>(ISerializer<T> serializer)
        {
            string filePath = GetFilePath(serializer);
            return filePath;
        }

        string GetFilePath<T>(ISerializer<T> serializer)
        {
            string filePath = Path.Combine(Resource.DataDirectoryPath, ResourceName + serializer.Extension);
            return filePath;
        }

        /// <summary>
        /// 转换为模版输出;
        /// </summary>
        public virtual ResourceSearcher AsTemplateResourceSearcher()
        {
            const string template = "_Template";
            if (!ResourceName.EndsWith(template))
            {
                string resName = ResourceName + template;
                return new ResourceSearcher(resName);
            }
            return this;
        }

        public override bool Equals(object obj)
        {
            ResourceSearcher i = obj as ResourceSearcher;

            if (i == null)
                return false;

            return i.ResourceName == ResourceName;
        }

        public override int GetHashCode()
        {
            return ResourceName.GetHashCode();
        }

        public static implicit operator ResourceSearcher(string resourceName)
        {
            return new ResourceSearcher(resourceName);
        }

        public static implicit operator string(ResourceSearcher searcher)
        {
            return searcher.ResourceName;
        }
    }


    /// <summary>
    /// 相同目录多个不同命名的资源文件获取;
    /// </summary>
    public class MultipleResourceSearcher : ResourceSearcher
    {
        public MultipleResourceSearcher(string resourceName) : base(resourceName)
        {
            SearchOption = SearchOption.TopDirectoryOnly;
        }

        public SearchOption SearchOption { get; private set; }

        public override IEnumerable<string> Searche<T>(ISerializer<T> serializer)
        {
            return Searche(serializer, Resource.DataDirectoryPath);
        }

        public IEnumerable<string> Searche<T>(ISerializer<T> serializer, string directory)
        {
            string fullPath = Path.Combine(directory, ResourceName);
            string directoryPath = Path.GetDirectoryName(fullPath);
            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            string searchPattern = fileName + "*" + serializer.Extension;
            return Directory.EnumerateFiles(directoryPath, searchPattern, SearchOption);
        }

        public override string GetWrite<T>(ISerializer<T> serializer)
        {
            return GetWrite(serializer, Resource.DataDirectoryPath);
        }

        string GetWrite<T>(ISerializer<T> serializer, string directory)
        {
            string path = GetWritePath(serializer, directory);
            return path;
        }

        string GetWritePath<T>(ISerializer<T> serializer, string directory)
        {
            string path = Path.Combine(directory, ResourceName + serializer.Extension);
            int i = 0;
            while (File.Exists(path))
            {
                path = Path.Combine(directory, ResourceName + "_" + i.ToString() + serializer.Extension);
                i++;
            }
            return path;
        }
    }
}
