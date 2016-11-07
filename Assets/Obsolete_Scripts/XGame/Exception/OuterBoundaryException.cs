using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame
{

    /// <summary>
    /// 超出地图边界;
    /// </summary>
    public class OuterBoundaryException : Exception
    {

        public OuterBoundaryException() : base("超出地图边界;") { }
        public OuterBoundaryException(string message) : base(message) { }
        public OuterBoundaryException(string message, Exception innerException) : base(message, innerException) { }

    }

}
