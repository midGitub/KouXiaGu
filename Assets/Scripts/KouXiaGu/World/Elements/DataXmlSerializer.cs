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
    public abstract class DataXmlSerializer<T> : DataReader<Dictionary<int, T>, IEnumerable<T>>
        where T : IMarked
    {
        protected static readonly XmlSerializer serializer = new XmlSerializer(typeof(T[]));

        public override string FileExtension
        {
            get { return ".xml"; }
        }

        public override Dictionary<int, T> Read(IEnumerable<string> filePaths)
        {
            Dictionary<int, T> dictionary = new Dictionary<int, T>();

            foreach (var filePath in filePaths)
            {
                var infos = Read(filePath);
                AddOrUpdate(dictionary, infos);
            }

            return dictionary;
        }

        T[] Read(string filePath)
        {
            var item = (T[])serializer.DeserializeXiaGu(filePath);
            return item;
        }

        void AddOrUpdate(Dictionary<int, T> dictionary, IEnumerable<T> infos)
        {
            foreach (var info in infos)
            {
                dictionary.AddOrUpdate(info.ID, info);
            }
        }

        public override void Write(IEnumerable<T> item, string filePath)
        {
            T[] array = item.ToArray();
            serializer.SerializeXiaGu(filePath, array);
        }
    }


}
