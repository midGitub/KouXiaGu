using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Map
{
    public class BlockNotFoundException : Exception
    {

        public BlockNotFoundException() : base(defaultMessage) { }
        public BlockNotFoundException(string message) : base(message) { }
        public BlockNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        private const string defaultMessage = "块未载入!";
    }
}
