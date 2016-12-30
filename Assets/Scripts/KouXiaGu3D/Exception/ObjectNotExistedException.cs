using System;
using System.Runtime.Serialization;

namespace KouXiaGu
{


    public class ObjectNotExistedException : Exception
    {
        public ObjectNotExistedException() : base() { }
        public ObjectNotExistedException(string message) : base(message) { }
        public ObjectNotExistedException(string message, Exception innerException) : base(message, innerException) { }
        protected ObjectNotExistedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

}
