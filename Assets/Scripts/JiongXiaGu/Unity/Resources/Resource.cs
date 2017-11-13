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
    public class Resource
    {
        private static LoadableContentInfo[] dlc;
        private static LoadableContentInfo[] mod;

        /// <summary>
        /// 核心资源;
        /// </summary>
        public static LoadableContentInfo Core { get; private set; }

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
        /// 初始化资源信息;
        /// </summary>
        internal static void Initialize()
        {
            LoadableContentReader modInfoReader = new LoadableContentReader();
            Core = new LoadableContentInfo(ResourcePath.CoreDirectory, new LoadableContentDescription("Core"), LoadableContentType.Core);
            dlc = modInfoReader.EnumerateModInfos(ResourcePath.DlcDirectory.FullName, LoadableContentType.DLC).ToArray();
            mod = modInfoReader.EnumerateModInfos(ResourcePath.ModDirectory.FullName, LoadableContentType.MOD).ToArray();
        }
    }
}
