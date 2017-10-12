using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 数据顺序记录;
    /// </summary>
    public class DataOrderRecord : IEnumerable<string>
    {
        readonly List<string> dataOrder;

        public DataOrderRecord()
        {
            dataOrder = new List<string>();
        }

        public DataOrderRecord(IEnumerable<DataDirectoryInfo> dataDirectoryInfos)
        {
            dataOrder = new List<string>();
            foreach (var dataDirectoryInfo in dataDirectoryInfos)
            {
                if (dataDirectoryInfo.Enabled)
                {
                    dataOrder.Add(dataDirectoryInfo.Name);
                }
            }
        }

        public void Add(string name)
        {
            dataOrder.Add(name);
        }

        /// <summary>
        /// 对数据文件夹进行优先级排序,返回一个新的合集;
        /// </summary>
        public List<DataDirectoryInfo> Sort(IReadOnlyCollection<DataDirectoryInfo> dataDirectoryInfos)
        {
            DataDirectoryInfo[] list = new DataDirectoryInfo[dataOrder.Count];
            foreach(var dataDirectoryInfo in dataDirectoryInfos)
            {
                int index = dataOrder.FindIndex(item => item == dataDirectoryInfo.Name);
                if (index >= 0)
                {
                    list[index] = dataDirectoryInfo;
                }
            }
            return list.Where(item => item != null).ToList();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return dataOrder.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
