using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 无法编辑;
    /// </summary>
    public class CanNotEditException : Exception
    {
        public CanNotEditException() : base() { }
        public CanNotEditException(string message) : base(message) { }
        public CanNotEditException(string message, Exception innerException) : base(message, innerException) { }
        protected CanNotEditException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

}
