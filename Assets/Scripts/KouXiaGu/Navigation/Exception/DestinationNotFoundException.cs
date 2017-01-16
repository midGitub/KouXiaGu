using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// 无法找到目的地;
    /// </summary>
    public class DestinationNotFoundException : Exception
    {
        public DestinationNotFoundException() : base(defaultMessage) { }
        public DestinationNotFoundException(string message) : base(message) { }
        public DestinationNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        private const string defaultMessage = "无法找到有效的路径!";

    }

}
