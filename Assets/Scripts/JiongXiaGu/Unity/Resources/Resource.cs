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
        /// <summary>
        /// 核心资源;
        /// </summary>
        public static ILoadableResource Core { get; private set; }

        /// <summary>
        /// 所有拓展资源;
        /// </summary>
        public static IReadOnlyCollection<ModInfo> Dlc
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 所有模组资源;
        /// </summary>
        public static IReadOnlyCollection<ModInfo> Mod
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 初始化资源信息;
        /// </summary>
        internal static void Initialize()
        {
            Core = new CoreResourceInfo(ResourcePath.CoreDirectory);
        }
    }
}
