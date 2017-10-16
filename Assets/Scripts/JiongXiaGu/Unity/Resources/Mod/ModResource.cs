using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组资源对应的游戏组件的
    /// </summary>
    public class ComponentModResource
    {

        public enum Status
        {
            /// <summary>
            /// 未找到对应的资源信息;
            /// </summary>
            NotFound,

            /// <summary>
            /// 所有资源信息读取完毕;
            /// </summary>
            Finished,
        }
    }

    /// <summary>
    /// 模组资源;
    /// </summary>
    public struct ModResource
    {
        /// <summary>
        /// 模组信息;
        /// </summary>
        public ModInfo ModInfo { get; private set; }

        /// <summary>
        /// 资源合集;
        /// </summary>
        public ComponentCollection<ComponentModResource> ModComponentResources { get; private set; }

        public ModResource(ModInfo modInfo)
        {
            if (modInfo == null)
                throw new ArgumentNullException(nameof(modInfo));

            ModInfo = modInfo;
            ModComponentResources = new ComponentCollection<ComponentModResource>();
        }

        public ModResource(ModInfo modInfo, ComponentCollection<ComponentModResource> resource)
        {
            if (modInfo == null)
                throw new ArgumentNullException(nameof(modInfo));
            if (resource == null)
                throw new ArgumentNullException(nameof(resource));

            ModInfo = modInfo;
            ModComponentResources = resource;
        }
    }
}
