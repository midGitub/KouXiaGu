using System;
using System.Runtime.Serialization;

namespace KouXiaGu
{

    /// <summary>
    /// 前提不满足;
    /// </summary>
    public class PremiseNotInvalidException : Exception
    {
        public PremiseNotInvalidException() : base() { }
        public PremiseNotInvalidException(string message) : base(message) { }
        public PremiseNotInvalidException(string message, Exception innerException) : base(message, innerException) { }
        protected PremiseNotInvalidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

}
