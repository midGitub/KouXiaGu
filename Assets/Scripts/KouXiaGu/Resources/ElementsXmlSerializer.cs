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

    /// <summary>
    /// 将数组序列化;
    /// </summary>
    public class ElementsXmlSerializer<T> : FilesSerializer<T[], Dictionary<int, T>>
        where T : IElement
    {
        public ElementsXmlSerializer(IFilePath file) : base(file, new XmlFileSerializer<T[]>(), defaultCombiner)
        {
        }

        static readonly FileCombiner defaultCombiner = new FileCombiner();

        class FileCombiner : ICombiner<T[], Dictionary<int, T>>
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

            public T[] Separate(Dictionary<int, T> item)
            {
                T[] itemArray = item.Values.ToArray();
                return itemArray;
            }
        }
    }
}
