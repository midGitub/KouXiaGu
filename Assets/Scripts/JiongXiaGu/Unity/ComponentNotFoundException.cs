using System;
using System.Runtime.Serialization;

namespace JiongXiaGu.Unity
{
    /// <summary>
    /// 当无法找到指定 Component 所引发的异常;
    /// </summary>
    public class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException()
        {
        }

        public ComponentNotFoundException(string message) : base(message)
        {
        }

        public ComponentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ComponentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
