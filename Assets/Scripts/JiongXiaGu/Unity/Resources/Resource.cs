using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源定义;
    /// </summary>
    public static class Resource
    {
        private static LoadableContentInfo core;
        private static List<LoadableContentInfo> dlc;
        private static List<LoadableContentInfo> mod;
        private static List<LoadableContentInfo> all;

        /// <summary>
        /// 核心资源;
        /// </summary>
        public static LoadableContentInfo Core
        {
            get { return core; }
        }

        /// <summary>
        /// 所有拓展资源;
        /// </summary>
        public static IReadOnlyCollection<LoadableContentInfo> Dlc
        {
            get { return dlc; }
        }

        /// <summary>
        /// 所有模组资源;
        /// </summary>
        public static IReadOnlyCollection<LoadableContentInfo> Mod
        {
            get { return mod; }
        }

        /// <summary>
        /// 所有资源;
        /// </summary>
        public static IReadOnlyCollection<LoadableContentInfo> All
        {
            get { return all; }
        }

        /// <summary>
        /// 初始化资源信息;
        /// </summary>
        internal static void Initialize()
        {
            LoadableDirectoryReader contentReader = new LoadableDirectoryReader();
            core = GetCore();
            dlc = GetDlc(contentReader);
            mod = GetMod(contentReader);
            all = GetAll();
        }

        private static LoadableContentInfo GetCore()
        {
            return new LoadableContentInfo(new LoadableDirectory(ResourcePath.CoreDirectory), new LoadableContentDescription("0", "Core"), LoadableContentType.Core);
        }

        private static List<LoadableContentInfo> GetDlc(LoadableDirectoryReader contentReader)
        {
            return contentReader.EnumerateModInfos(ResourcePath.DlcDirectory.FullName, LoadableContentType.DLC).ToList();
        }

        private static List<LoadableContentInfo> GetMod(LoadableDirectoryReader contentReader)
        {
            return contentReader.EnumerateModInfos(ResourcePath.ModDirectory.FullName, LoadableContentType.MOD).ToList();
        }

        private static List<LoadableContentInfo> GetAll()
        {
            int capacity = 1 + dlc.Count + mod.Count;
            var all = new List<LoadableContentInfo>(capacity);
            all.Add(Core);
            all.AddRange(dlc);
            all.AddRange(mod);
            return all;
        }
    }
}
