using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{


    public abstract class ContentSerializer<T>
    {
        public abstract string RelativePath { get; }

        public void Serialize(LoadableContent content, T item)
        {
            using (var stream = content.CreateOutStream(RelativePath))
            {
                Serialize(stream, item);
            }
        }

        public T Deserialize(LoadableContent content)
        {
            using (var stream = content.GetInputStream(RelativePath))
            {
                var item = Deserialize(stream);
                return item;
            }
        }

        /// <summary>
        /// 序列化内容;
        /// </summary>
        public abstract void Serialize(Stream stream, T item);

        /// <summary>
        /// 反序列化内容;
        /// </summary>
        public abstract T Deserialize(Stream stream);
    }
}
