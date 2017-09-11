using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 读取资源;
    /// </summary>
    /// <typeparam name="T">读取到T;</typeparam>
    public interface IResourceSerializer<T>
    {
        void Serialize(T result);
        T Deserialize();
    }

    /// <summary>
    /// 读取资源;
    /// </summary>
    /// <typeparam name="TSource">序列化得到的内容;</typeparam>
    /// <typeparam name="TResult">转换到的内容;</typeparam>
    public abstract class ResourceSerializer<TSource, TResult> : IResourceSerializer<TResult>
    {
        protected ResourceSerializer()
        {
        }

        public ResourceSerializer(ISerializer<TSource> serializer, ResourceSearcher resourceSearcher)
        {
            Serializer = serializer;
            ResourceSearcher = resourceSearcher;
        }

        public ISerializer<TSource> Serializer { get; set; }
        public ResourceSearcher ResourceSearcher { get; set; }

        /// <summary>
        /// 将多个TSource转换成一个TResult;
        /// </summary>
        protected abstract TResult Combine(List<TSource> sources);

        /// <summary>
        /// 将 TResult 转换为 TSource;
        /// </summary>
        protected abstract TSource Convert(TResult result);

        public void Serialize(TResult result)
        {
            TSource source = Convert(result);
            string path = ResourceSearcher.GetWrite(Serializer);
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                Serializer.Serialize(source, stream);
            }
        }

        public TResult Deserialize()
        {
            var sources = DeserializeSources();
            TResult result = Combine(sources);
            return result;
        }

        List<TSource> DeserializeSources()
        {
            List<TSource> sources = new List<TSource>();
            IEnumerable<string> paths = ResourceSearcher.Searche(Serializer);
            foreach (var path in paths)
            {
                using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    TSource source = Serializer.Deserialize(stream);
                    sources.Add(source);
                }
            }
            return sources;
        }
    }
}
