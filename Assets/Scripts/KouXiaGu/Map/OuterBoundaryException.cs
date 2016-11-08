using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 超出地图边界;
    /// </summary>
    public class OuterBoundaryException : Exception
    {

        public OuterBoundaryException() : base(defaultMessage) { }
        public OuterBoundaryException(string message) : base(message) { }
        public OuterBoundaryException(string message, Exception innerException) : base(message, innerException) { }

        private const string defaultMessage = "超出地图边界!";
    }

}
