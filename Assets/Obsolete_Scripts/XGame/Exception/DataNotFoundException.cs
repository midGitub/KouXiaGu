using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame
{


    public class DataNotFoundException : Exception
    {
        public DataNotFoundException() : base("缺少数据;") { }
        public DataNotFoundException(string message) : base(message) { }
        public DataNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

}
