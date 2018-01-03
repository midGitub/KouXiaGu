using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 内容序列化器;
    /// </summary>
    public abstract class ContentSerializer<T>
    {
        /// <summary>
        /// 返回资源的完整相对路径,需要带上后缀名;
        /// </summary>
        public abstract string RelativePath { get; }

        /// <summary>
        /// 序列化方法;
        /// </summary>
        public abstract ISerializer<T> Serializer { get; }

        /// <summary>
        /// 添加对应资源,若已经存在则覆盖;
        /// </summary>
        public virtual void Serialize(Content content, T item)
        {
            using (var stream = content.GetOutputStream(RelativePath))
            {
                Serializer.Serialize(stream, item);
            }
        }

        /// <summary>
        /// 获取到对应资源,若未能获取到则返回异常;
        /// </summary>
        public virtual T Deserialize(Content content)
        {
            using (var stream = content.GetInputStream(RelativePath))
            {
                var item = Serializer.Deserialize(stream);
                return item;
            }
        }

        /// <summary>
        /// 获取到对应资源,若未能获取到则返回false;
        /// </summary>
        public bool TryDeserialize(Content content, out T item)
        {
            try
            {
                using (var stream = content.GetInputStream(RelativePath))
                {
                    item = Serializer.Deserialize(stream);
                    return true;
                }
            }
            catch
            {
                item = default(T);
                return false;
            }
        }
    }
}
