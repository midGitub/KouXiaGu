using System;
using System.Collections.Generic;
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
    public class ElementXmlSerializer<T> : ICombiner<T[], Dictionary<int, T>>, IReader<Dictionary<int, T>>
        where T : IElement
    {
        public ElementXmlSerializer(IFilePath file)
        {
            fileSerializer = new FilesXmlSerializer<T[], Dictionary<int, T>>(file, this);
        }

        FilesXmlSerializer<T[], Dictionary<int, T>> fileSerializer;

        public Dictionary<int, T> Read()
        {
            return fileSerializer.Read();
        }

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
    }
}
