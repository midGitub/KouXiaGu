using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组资源;
    /// </summary>
    public class ModResource : IReadOnlyCollection<ModInfo>
    {
        /// <summary>
        /// 所有模组信息;
        /// </summary>
        public static IEnumerable<ModInfo> ModInfos { get; private set; }

        /// <summary>
        /// 根据读取优先顺序排序的模组信息;
        /// </summary>
        private List<ModInfo> orderedModInfos;

        /// <summary>
        /// 是否只读?
        /// </summary>
        public bool IsReadOnly { get; private set; }

        public ModResource()
        {
            orderedModInfos = new List<ModInfo>();
        }

        public int Count
        {
            get { return orderedModInfos.Count; }
        }

        public IEnumerator<ModInfo> GetEnumerator()
        {
            return orderedModInfos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
