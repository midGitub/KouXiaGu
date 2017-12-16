using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源类型;
    /// </summary>
    public enum PathDefinitionType
    {
        /// <summary>
        /// 资源数据;
        /// </summary>
        Data,

        /// <summary>
        /// 配置数据;
        /// </summary>
        Config,

        /// <summary>
        /// 用户数据;
        /// </summary>
        UserConfig,

        /// <summary>
        /// 用户存档数据;
        /// </summary>
        Archive,

        /// <summary>
        /// 资源数据目录;
        /// </summary>
        DataDirectory,

        /// <summary>
        /// 用户数据目录;
        /// </summary>
        UserConfigDirectory,

        /// <summary>
        /// 用户存档数据目录;
        /// </summary>
        ArchiveDirectory,
    }

    /// <summary>
    /// 在程序中定义的文件路径;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,  Inherited = false, AllowMultiple = false)]
    public sealed class PathDefinitionAttribute : Attribute
    {
        /// <summary>
        /// 资源类型;
        /// </summary>
        public PathDefinitionType ResourceTypes { get; set; }

        /// <summary>
        /// 简短描述;
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 预留信息;
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 构造;
        /// </summary>
        /// <param name="resourceTypes">资源类型</param>
        /// <param name="name">简短描述</param>
        public PathDefinitionAttribute(PathDefinitionType resourceTypes, string name) : this(resourceTypes, name, string.Empty)
        {
        }

        /// <summary>
        /// 构造;
        /// </summary>
        /// <param name="resourceTypes">资源类型</param>
        /// <param name="name">简短描述</param>
        /// <param name="message">详细描述</param>
        public PathDefinitionAttribute(PathDefinitionType resourceTypes, string name, string message)
        {
            ResourceTypes = resourceTypes;
            Name = name;
            Message = message;
        }
    }
}
