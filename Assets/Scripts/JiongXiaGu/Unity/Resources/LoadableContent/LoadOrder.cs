using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源加载顺序;(可通过XML序列化)
    /// </summary>
    public class LoadOrder : IEnumerable<LoadOrder.LoadableContentXmlContent>
    {
        private LinkedList<LoadableContentInfo> order;

        /// <summary>
        /// 是否只读?
        /// </summary>
        public bool IsReadOnly { get; private set; } = false;

        public LoadOrder()
        {
            order = new LinkedList<LoadableContentInfo>();
        }

        public void Add(LoadableContentXmlContent xmlContent)
        {

        }

        public IEnumerator<LoadableContentXmlContent> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public struct LoadableContentXmlContent
        {
            public LoadableContentType Type { get; set; }
            public string ID { get; set; }
        }
    }
}
