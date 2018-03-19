using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组路径内容信息,用于存储数据定义;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ModificationSubpathInfoAttribute : Attribute 
    {
        public string Name { get; private set; }
        public string Message { get; private set; }

        public ModificationSubpathInfoAttribute(string name) : this(name, null)
        {
        }

        public ModificationSubpathInfoAttribute(string name, string message)
        {
            Name = name;
            Message = message;
        }
    }
}
