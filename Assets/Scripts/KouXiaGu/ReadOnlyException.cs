using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public class ReadOnlyException : Exception
    {
        public ReadOnlyException() : base(defaultMessage) { }
        public ReadOnlyException(string message) : base(message) { }
        public ReadOnlyException(string message, Exception innerException) : base(message, innerException) { }

        private const string defaultMessage = "仅只读操作!";

    }

}
