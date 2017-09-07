using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Resources
{
    
    public interface IElement
    {
        int ID { get; }
    }

    public class ElementsReaderWriter<T> : FilesReaderWriter<T[], Dictionary<int, T>>
         where T : IElement
    {
        public ElementsReaderWriter(IMultipleFilePath multipleFile, IOFileSerializer<T[]> serializer, ICombiner<T[], Dictionary<int, T>> combiner) : base(multipleFile, serializer, combiner)
        {
        }

        public ElementsReaderWriter(IMultipleFilePath multipleFile, IOFileSerializer<T[]> serializer) : base(multipleFile, serializer, defaultCombiner)
        {
        }

        static readonly DataCombiner defaultCombiner = new DataCombiner();

        protected class DataCombiner : ICombiner<T[], Dictionary<int, T>>
        {
            public Dictionary<int, T> Combine(IEnumerable<T[]> itemArrays)
            {
                Dictionary<int, T> completed = new Dictionary<int, T>();
                string errorStr = string.Empty;

                foreach (T[] items in itemArrays)
                {
                    foreach (var item in items)
                    {
                        if (completed.ContainsKey(item.ID))
                        {
                            errorStr += "\n相同的ID:" + item.ID;
                        }
                        else
                        {
                            completed.Add(item.ID, item);
                        }
                    }
                }

                if (errorStr != string.Empty)
                {
                    Debug.LogWarning(ToString() + errorStr);
                }
                return completed;
            }

            IEnumerable<WriteInfo<T[]>> ICombiner<T[], Dictionary<int, T>>.Separate(Dictionary<int, T> item)
            {
                T[] itemArray = item.Values.ToArray();
                yield return new WriteInfo<T[]>(string.Empty, itemArray);
            }
        }
    }

    /// <summary>
    /// 将数组序列化;
    /// </summary>
    public class XmlElementsReaderWriter<T> : ElementsReaderWriter<T>
        where T : IElement
    {
        public XmlElementsReaderWriter(IMultipleFilePath file) : base(file, new XmlFileSerializer<T[]>())
        {
        }
    }
}
