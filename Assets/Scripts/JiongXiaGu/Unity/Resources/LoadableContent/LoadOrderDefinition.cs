using System;
using System.Collections;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 内容加载顺序;(可通过XML序列化)
    /// </summary>
    public class LoadOrderDefinition : IEnumerable<string>
    {
        /// <summary>
        /// 资源加载先后顺序,优先级从高到低;
        /// </summary>
        public LinkedList<string> LoadOrder { get; private set; }

        public LoadOrderDefinition()
        {
            LoadOrder = new LinkedList<string>();
        }

        public LoadOrderDefinition(IEnumerable<string> contents)
        {
            LoadOrder = new LinkedList<string>(contents);
        }

        public LoadOrderDefinition(params string[] contents) : this(contents as IEnumerable<string>)
        {
        }

        /// <summary>
        /// 提供序列化使用的Add()操作;
        /// </summary>
        public void Add(string contentID)
        {
            LoadOrder.AddLast(contentID);
        }

        public void Add(object contentID)
        {
            LoadOrder.AddLast(contentID.ToString());
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return LoadOrder.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return LoadOrder.GetEnumerator();
        }
    }
}
