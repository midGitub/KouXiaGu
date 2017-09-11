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
    /// <typeparam name="TSource">序列化得到的内容;</typeparam>
    /// <typeparam name="TResult">转换到的内容;</typeparam>
    public abstract class ResourceSerializer<TSource, TResult>
    {
        public ResourceSerializer(ISerializer<TSource> serializer, ResourceSearcher resourceSearcher)
        {
            Serializer = serializer;
            ResourceSearcher = resourceSearcher;
        }

        public ISerializer<TSource> Serializer { get; set; }
        public ResourceSearcher ResourceSearcher { get; set; }

        protected abstract TResult Convert(IEnumerable<TSource> sources);
        protected abstract TSource Convert(TResult result);

        public void Serialize(TResult result)
        {
            TSource source = Convert(result);
            using (Stream stream = ResourceSearcher.GetWrite(Serializer))
            {
                Serializer.Serialize(source, stream);
            }
        }

        public TResult Deserialize()
        {
            IEnumerable<TSource> sources = DeserializeSources();
            TResult result = Convert(sources);
            return result;
        }

        IEnumerable<TSource> DeserializeSources()
        {
            IEnumerable<Stream> streams = ResourceSearcher.Searche(Serializer);
            foreach (var stream in streams)
            {
                using (stream)
                {
                    TSource source = Serializer.Deserialize(stream);
                    yield return source;
                }
            }
        }
    }
}
