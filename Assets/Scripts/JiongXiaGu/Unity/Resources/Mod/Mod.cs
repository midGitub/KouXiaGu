using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 游戏模组信息;
    /// </summary>
    public static class Mod
    {
        private static readonly ModSearcher modSearcher = new ModSearcher();
        private static readonly ModOrderReader modOrderReader = new ModOrderReader();
        internal static bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// 所有模组信息(不包括核心数据);
        /// </summary>
        private static List<ModInfo> modInfos;

        /// <summary>
        /// 根据读取优先顺序排序的模组信息(不包含核心数据);
        /// </summary>
        private static List<ModInfo> orderedModInfosWithoutCore;

        /// <summary>
        /// 根据读取优先顺序排序的模组信息(包含核心数据);
        /// </summary>
        private static List<ModInfo> orderedModInfos;

        /// <summary>
        /// 所有模组信息;
        /// </summary>
        public static IReadOnlyCollection<ModInfo> ModInfos
        {
            get { return modInfos; }
        }

        /// <summary>
        /// 按读取优先顺序排序好的模组合集;
        /// </summary>
        public static IReadOnlyCollection<ModInfo> OrderedModInfos
        {
            get { return orderedModInfos; }
        }

        /// <summary>
        /// 按读取优先顺序排序好的模组合集;
        /// </summary>
        public static IReadOnlyCollection<ModInfo> OrderedModInfosWithoutCore
        {
            get { return orderedModInfosWithoutCore; }
        }

        internal static void Initialize()
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

        private static void Initialize(IEnumerable<ModInfo> mods)
        {
            modInfos = new List<ModInfo>(mods);
            orderedModInfosWithoutCore = new List<ModInfo>();
            orderedModInfos = new List<ModInfo>();
            orderedModInfos.Add(Resource.CoreDirectoryInfo);
        }

        private static void Initialize(IEnumerable<ModInfo> mods, ModOrder modOrder)
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
        public static void SetModOrder(ModOrder modOrder)
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
