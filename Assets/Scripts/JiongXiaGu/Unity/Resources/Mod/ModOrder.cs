using System.Collections;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组顺序记录;
    /// </summary>
    public class ModOrder : IEnumerable<string>
    {
        readonly List<string> modNames;

        public ModOrder()
        {
            modNames = new List<string>();
        }

        public ModOrder(IEnumerable<ModInfo> modOrder)
        {
            this.modNames = new List<string>();
            foreach (var dataDirectoryInfo in modOrder)
            {
                this.modNames.Add(dataDirectoryInfo.Name);
            }
        }

        public IReadOnlyList<string> ModNames
        {
            get { return modNames; }
        }

        /// <summary>
        /// 将新的模组名添加到最后(提供序列化使用);
        /// </summary>
        public void Add(string name)
        {
            modNames.Add(name);
        }

        /// <summary>
        /// 获取到模组读取顺序;
        /// </summary>
        public void Sort(IEnumerable<ModInfo> modInfos, ICollection<ModInfo> emptyCollection)
        {
            ModInfo[] list = new ModInfo[modNames.Count];
            foreach (var dataDirectoryInfo in modInfos)
            {
                int index = modNames.FindIndex(item => item == dataDirectoryInfo.Name);
                if (index >= 0)
                {
                    list[index] = dataDirectoryInfo;
                }
            }

            foreach (var item in list)
            {
                if (item != null)
                {
                    emptyCollection.Add(item);
                }
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return modNames.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
