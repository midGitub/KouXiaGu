using System;
using System.Runtime.Serialization;

namespace KouXiaGu
{

    /// <summary>
    /// 缺少资源;
    /// </summary>
    public class LackOfResourcesException : Exception
    {
        public LackOfResourcesException() : base() { }
        public LackOfResourcesException(string message) : base(message) { }
        public LackOfResourcesException(string message, Exception innerException) : base(message, innerException) { }
        protected LackOfResourcesException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

}
