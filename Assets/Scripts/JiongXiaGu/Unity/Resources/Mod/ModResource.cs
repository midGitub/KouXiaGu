using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组资源;
    /// </summary>
    public class ModResource : IDisposable
    {
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 所有模组信息(不包括核心数据);
        /// </summary>
        private List<ModInfo> modInfos;

        /// <summary>
        /// 根据读取优先顺序排序的模组信息(不包含核心数据);
        /// </summary>
        private List<ModInfo> orderedModInfosWithoutCore;

        /// <summary>
        /// 根据读取优先顺序排序的模组信息;
        /// </summary>
        private List<ModInfo> orderedModInfos;

        /// <summary>
        /// 是否只读?
        /// </summary>
        public bool IsReadOnly { get; internal set; }

        internal ModResource(IEnumerable<ModInfo> mods)
        {
            if (mods == null)
                throw new ArgumentNullException(nameof(mods));

            modInfos = new List<ModInfo>(mods);
            orderedModInfosWithoutCore = new List<ModInfo>();
            orderedModInfos = new List<ModInfo>();
            orderedModInfos.Add(Resource.CoreDirectoryInfo);
        }

        internal ModResource(IEnumerable<ModInfo> mods, ModOrder modOrder)
        {
            if (mods == null)
                throw new ArgumentNullException(nameof(mods));
            if (modOrder == null)
                throw new ArgumentNullException(nameof(modOrder));

            modInfos = new List<ModInfo>(mods);
            orderedModInfosWithoutCore = new List<ModInfo>();
            modOrder.Sort(modInfos, orderedModInfosWithoutCore);

            orderedModInfos = new List<ModInfo>();
            orderedModInfos.Add(Resource.CoreDirectoryInfo);
            orderedModInfos.AddRange(orderedModInfosWithoutCore);
        }

        /// <summary>
        /// 按读取优先顺序排序好的模组合集;
        /// </summary>
        public IReadOnlyCollection<ModInfo> OrderedModInfos
        {
            get { return orderedModInfos; }
        }

        /// <summary>
        /// 按读取优先顺序排序好的模组合集;
        /// </summary>
        public IReadOnlyCollection<ModInfo> OrderedModInfosWithoutCore
        {
            get { return orderedModInfosWithoutCore; }
        }

        /// <summary>
        /// 所有模组信息;
        /// </summary>
        public IReadOnlyCollection<ModInfo> ModInfos
        {
            get { return modInfos; }
        }

        /// <summary>
        /// 设置模组读取顺序;
        /// </summary>
        public void SetModOrder(ModOrder modOrder)
        {
            if (modOrder == null)
                throw new ArgumentNullException(nameof(modOrder));
            if (IsReadOnly)
                throw new InvalidOperationException(string.Format("[{0}]当前为只读状态;", nameof(ModResource)));

            orderedModInfosWithoutCore.Clear();
            modOrder.Sort(modInfos, orderedModInfosWithoutCore);

            orderedModInfos.Clear();
            orderedModInfos.Add(Resource.CoreDirectoryInfo);
            orderedModInfos.AddRange(orderedModInfosWithoutCore);
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                foreach (var modInfo in modInfos)
                {
                    modInfo.Dispose();
                }
                IsDisposed = true;
            }
        }
    }
}