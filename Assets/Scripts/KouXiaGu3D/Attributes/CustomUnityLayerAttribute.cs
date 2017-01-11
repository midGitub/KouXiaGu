using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 在代码中定义的 Layer;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class CustomUnityLayerAttribute : Attribute
    {
        public CustomUnityLayerAttribute() { }
        public CustomUnityLayerAttribute(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }

    }

}
