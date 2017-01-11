using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 在代码中定义的 Tag;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CustomUnityTagAttribute : Attribute
    {
        public CustomUnityTagAttribute() { }
        public CustomUnityTagAttribute(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }

    }

}
