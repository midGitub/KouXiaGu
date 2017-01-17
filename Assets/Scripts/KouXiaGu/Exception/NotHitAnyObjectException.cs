using System;
using System.Runtime.Serialization;

namespace KouXiaGu
{


    public class NotHitAnyObjectException : Exception
    {
        public NotHitAnyObjectException() : base() { }
        public NotHitAnyObjectException(string message) : base(message) { }
        public NotHitAnyObjectException(string message, Exception innerException) : base(message, innerException) { }
        protected NotHitAnyObjectException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

}
