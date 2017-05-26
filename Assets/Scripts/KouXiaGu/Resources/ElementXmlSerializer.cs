using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Resources
{

    public interface IElement
    {
        int ID { get; }
    }

    /// <summary>
    /// 将数组序列化;
    /// </summary>
    public class ElementXmlSerializer<T> : FileXmlSerializer<T[]>, IReader<List<T>>, IReader<Dictionary<int, T>>
        where T : IElement
    {
        public ElementXmlSerializer(IFilePath file) : base(file)
        {
        }

        /// <summary>
        /// 读取到文件,若遇到异常则输出到日志;
        /// </summary>
        List<T> IReader<List<T>>.Read()
        {
            IReader<List<T[]>> arrayReader = this;
            List<T[]> items = arrayReader.Read();
            List<T> completed = new List<T>();
            foreach (var item in items)
            {
                completed.AddRange(item);
            }
            return completed;
        }

        /// <summary>
        /// 读取到文件,若遇到异常则输出到日志;
        /// </summary>
        Dictionary<int, T> IReader<Dictionary<int, T>>.Read()
        {
            IReader<List<T[]>> arrayReader = this;
            List<T[]> itemArrays = arrayReader.Read();
            Dictionary<int, T> completed = new Dictionary<int, T>();
            string errorStr = ToString();

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
            return completed;
        }
    }
}
