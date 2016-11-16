using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// 无法到达目的地,路径不存在;
    /// </summary>
    public class PathNotFoundException : Exception
    {
        public PathNotFoundException() : base(defaultMessage) { }
        public PathNotFoundException(string message) : base(message) { }
        public PathNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        private const string defaultMessage = "无法找到有效的路径!";

    }

}
