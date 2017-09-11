using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KouXiaGu.Resources
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

        public virtual IEnumerable<Stream> Searche<T>(ISerializer<T> serializer)
        {
            string filePath = GetFilePath(serializer);
            if (File.Exists(filePath))
            {
                Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                yield return stream;
            }
        }

        public virtual Stream GetWrite<T>(ISerializer<T> serializer)
        {
            string filePath = GetFilePath(serializer);
            Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            return stream;
        }

        string GetFilePath<T>(ISerializer<T> serializer)
        {
            string filePath = Path.Combine(Resource.ConfigDirectoryPath, ResourceName + serializer.Extension);
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

        public override IEnumerable<Stream> Searche<T>(ISerializer<T> serializer)
        {
            return Searche(serializer, Resource.ConfigDirectoryPath);
        }

        IEnumerable<Stream> Searche<T>(ISerializer<T> serializer, string directory)
        {
            IEnumerable<string> paths = SearchePath(serializer, directory);
            foreach (var path in paths)
            {
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                yield return stream;
            }
        }

        public IEnumerable<string> SearchePath<T>(ISerializer<T> serializer, string directory)
        {
            string fullPath = Path.Combine(directory, ResourceName);
            string directoryPath = Path.GetDirectoryName(fullPath);
            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            string searchPattern = fileName + "*" + serializer.Extension;
            return Directory.EnumerateFiles(directoryPath, searchPattern, SearchOption);
        }

        public override Stream GetWrite<T>(ISerializer<T> serializer)
        {
            return GetWrite(serializer, Resource.ConfigDirectoryPath);
        }

        Stream GetWrite<T>(ISerializer<T> serializer, string directory)
        {
            string path = GetWritePath(serializer, directory);
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            return stream;
        }

        string GetWritePath<T>(ISerializer<T> serializer, string directory, int max = 100)
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
