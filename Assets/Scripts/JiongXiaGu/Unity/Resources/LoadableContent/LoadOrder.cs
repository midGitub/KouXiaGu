using System;
using System.Collections;
using System.Collections.Generic;
using JiongXiaGu.Collections;
using System.Linq;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源加载顺序;(可通过XML序列化)
    /// </summary>
    public class LoadOrder : IEnumerable<LoadOrder.LoadableContentXmlContent>, IEquatable<LoadOrder>
    {
        public System.Collections.Generic.LinkedList<LoadableContentInfo> Order { get; private set; }

        public LoadOrder()
        {
            Order = new System.Collections.Generic.LinkedList<LoadableContentInfo>();
        }

        public LoadOrder(LoadOrder loadOrder)
        {
            Order = new System.Collections.Generic.LinkedList<LoadableContentInfo>(loadOrder.Order);
        }

        /// <summary>
        /// 添加可读资源到最后,若未能找到对应资源,则返回异常;
        /// </summary>
        public void Add(LoadableContentXmlContent xmlContent)
        {
            LoadableContentInfo info = ToContentInfo(xmlContent);
            Order.AddLast(info);
        }

        /// <summary>
        /// 转换成可读内容;若未找到对应的可读内容则返回异常;
        /// </summary>
        public LoadableContentInfo ToContentInfo(LoadableContentXmlContent xmlContent)
        {
            switch (xmlContent.Type)
            {
                case LoadableContentType.Core:
                    return Resource.Core;

                case LoadableContentType.DLC:
                    return Find(Resource.Dlc, xmlContent);

                case LoadableContentType.MOD:
                    return Find(Resource.Mod, xmlContent);

                default:
                    throw LoadableContentInfoNotFoundException(xmlContent);
            }
        }

        /// <summary>
        /// 寻找到对应的可读内容实例;若未找到对应的可读内容则返回异常;
        /// </summary>
        private LoadableContentInfo Find(IReadOnlyCollection<LoadableContentInfo> infos, LoadableContentXmlContent xmlContent)
        {
            foreach (var info in infos)
            {
                if (xmlContent == info)
                {
                    return info;
                }
            }
            throw LoadableContentInfoNotFoundException(xmlContent);
        }

        /// <summary>
        /// 返回未找到可读内容异常;
        /// </summary>
        private Exception LoadableContentInfoNotFoundException(LoadableContentXmlContent xmlContent)
        {
            return new KeyNotFoundException(string.Format("未找到类型为 {0} ,ID为 {1} 的资源;", xmlContent.Type, xmlContent.ID));
        }

        public IEnumerator<LoadableContentXmlContent> GetEnumerator()
        {
            return Order.Select(info => new LoadableContentXmlContent(info)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LoadOrder);
        }

        public bool Equals(LoadOrder other)
        {
            return other != null &&
                Order.IsSame(other.Order);
        }

        public override int GetHashCode()
        {
            return -390870225 + EqualityComparer<System.Collections.Generic.LinkedList<LoadableContentInfo>>.Default.GetHashCode(Order);
        }

        public static bool operator ==(LoadOrder order1, LoadOrder order2)
        {
            return EqualityComparer<LoadOrder>.Default.Equals(order1, order2);
        }

        public static bool operator !=(LoadOrder order1, LoadOrder order2)
        {
            return !(order1 == order2);
        }

        /// <summary>
        /// 提供XML序列化的结构;
        /// </summary>
        public struct LoadableContentXmlContent : IEquatable<LoadableContentInfo>, IEquatable<LoadableContentXmlContent>
        {
            public string ID { get; set; }
            public LoadableContentType Type { get; set; }

            public LoadableContentXmlContent(string id, LoadableContentType type)
            {
                ID = id;
                Type = type;
            }

            public LoadableContentXmlContent(LoadableContentInfo info) : this(info.Description.ID, info.Type)
            {
            }

            public override bool Equals(object obj)
            {
                return obj is LoadableContentXmlContent && Equals((LoadableContentXmlContent)obj);
            }

            public bool Equals(LoadableContentXmlContent other)
            {
                return ID == other.ID &&
                       Type == other.Type;
            }

            public bool Equals(LoadableContentInfo other)
            {
                return Type == other.Type && ID == other.Description.ID;
            }

            public override int GetHashCode()
            {
                var hashCode = 430596813;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
                hashCode = hashCode * -1521134295 + Type.GetHashCode();
                return hashCode;
            }

            public static bool operator ==(LoadableContentXmlContent content, LoadableContentInfo info)
            {
                return content.Equals(info);
            }

            public static bool operator !=(LoadableContentXmlContent content, LoadableContentInfo info)
            {
                return !(content == info);
            }

            public static bool operator ==(LoadableContentXmlContent content1, LoadableContentXmlContent content2)
            {
                return content1.Equals(content2);
            }

            public static bool operator !=(LoadableContentXmlContent content1, LoadableContentXmlContent content2)
            {
                return !(content1 == content2);
            }
        }
    }
}
