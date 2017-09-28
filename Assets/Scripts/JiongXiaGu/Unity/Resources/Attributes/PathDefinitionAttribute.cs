using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 在程序中定义的文件路径,需要放在 internal 访问级别的静态变量上;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class PathDefinitionAttribute : Attribute
    {
        /// <summary>
        /// 资源类型;
        /// </summary>
        public ResourceTypes ResourceTypes { get; set; }

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
        public PathDefinitionAttribute(ResourceTypes resourceTypes, string name) : this(resourceTypes, name, string.Empty)
        {
        }

        /// <summary>
        /// 构造;
        /// </summary>
        /// <param name="resourceTypes">资源类型</param>
        /// <param name="name">简短描述</param>
        /// <param name="message">详细描述</param>
        public PathDefinitionAttribute(ResourceTypes resourceTypes, string name, string message)
        {
            ResourceTypes = resourceTypes;
            Name = name;
            Message = message;
        }
    }
}
