using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 游戏模组信息;
    /// </summary>
    public class Mods
    {
        private readonly ModSearcher modSearcher = new ModSearcher();
        private readonly ModOrderReader modOrderReader = new ModOrderReader();
        internal bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// 所有模组信息(不包括核心数据);
        /// </summary>
        private List<ModInfo> modInfos;

        /// <summary>
        /// 根据读取优先顺序排序的模组信息(不包含核心数据);
        /// </summary>
        private List<ModInfo> orderedModInfosWithoutCore;

        /// <summary>
        /// 根据读取优先顺序排序的模组信息(包含核心数据);
        /// </summary>
        private List<ModInfo> orderedModInfos;

        /// <summary>
        /// 所有模组信息;
        /// </summary>
        public IReadOnlyCollection<ModInfo> ModInfos
        {
            get { return modInfos; }
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

        internal Mods()
        {
            if (!IsInitialized)
            {
                var modInfos = modSearcher.EnumerateModInfos();
                try
                {
                    ModOrder modOrder = modOrderReader.Read();
                    Initialize(modInfos, modOrder);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(string.Format("读取资源排列顺序文件失败:{0}", ex));
                    Initialize(modInfos);
                }
                IsInitialized = true;
            }
        }

        private void Initialize(IEnumerable<ModInfo> mods)
        {
            modInfos = new List<ModInfo>(mods);
            orderedModInfosWithoutCore = new List<ModInfo>();
            orderedModInfos = new List<ModInfo>();
            orderedModInfos.Add(Resource.CoreDirectoryInfo);
        }

        private void Initialize(IEnumerable<ModInfo> mods, ModOrder modOrder)
        {
            modInfos = new List<ModInfo>(mods);
            orderedModInfosWithoutCore = new List<ModInfo>();
            modOrder.Sort(modInfos, orderedModInfosWithoutCore);

            orderedModInfos = new List<ModInfo>();
            orderedModInfos.Add(Resource.CoreDirectoryInfo);
            orderedModInfos.AddRange(orderedModInfosWithoutCore);
        }

        /// <summary>
        /// 设置模组读取顺序;
        /// </summary>
        public void SetModOrder(ModOrder modOrder)
        {
            if (modOrder == null)
                throw new ArgumentNullException(nameof(modOrder));

            orderedModInfosWithoutCore.Clear();
            modOrder.Sort(modInfos, orderedModInfosWithoutCore);

            orderedModInfos.Clear();
            orderedModInfos.Add(Resource.CoreDirectoryInfo);
            orderedModInfos.AddRange(orderedModInfosWithoutCore);
        }
    }
}
