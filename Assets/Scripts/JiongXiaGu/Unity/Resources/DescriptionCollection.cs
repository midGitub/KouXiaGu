using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 提供序列化的描述合集;
    /// </summary>
    public class DescriptionCollection<T>
    {
        /// <summary>
        /// 版本;
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 描述;
        /// </summary>
        public List<T> Descriptions { get; set; }

        public IEnumerable<T> EnumerateDescription()
        {
            return Descriptions as IEnumerable<T> ?? EmptyCollection<T>.Default;
        }
    }
}
