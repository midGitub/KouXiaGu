using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 读取多个可序列化Xml;
    /// </summary>
    public abstract class XmlFilesSerializer<T> : IReader<T[]>, IReader<List<T>>
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(T[]));

        public abstract IEnumerable<string> FilePaths { get; }

        public List<T> Read()
        {
            List<T> items = new List<T>();

            foreach (var path in FilePaths)
            {
                var itemArray = Read(path);
                items.AddRange(itemArray);
            }

            return items;
        }

        T[] IReader<T[]>.Read()
        {
            var list = Read();
            return list.ToArray();
        }

        T[] Read(string filePath)
        {
            if (!File.Exists(filePath))
                return new T[0];

            var item = (T[])serializer.DeserializeXiaGu(filePath);
            return item;
        }

    }

}
