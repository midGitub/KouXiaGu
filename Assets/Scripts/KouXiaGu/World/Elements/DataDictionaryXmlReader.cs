using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KouXiaGu.Collections;

namespace KouXiaGu.World
{

    /// <summary>
    /// 通过序列化读取的资源;
    /// </summary>
    public abstract class DataDictionaryXmlReader<T> : DataDictionaryReader<T, IEnumerable<T>>
        where T : ElementInfo
    {
        protected static readonly XmlSerializer serializer = new XmlSerializer(typeof(T[]));

        public override string FileExtension
        {
            get { return ".xml"; }
        }

        protected override IEnumerable<T> Read(string filePath)
        {
            var item = (T[])serializer.DeserializeXiaGu(filePath);
            return item;
        }

        public override void Write(IEnumerable<T> item, string filePath)
        {
            T[] array = item.ToArray();
            serializer.SerializeXiaGu(filePath, array);
        }
    }


}
