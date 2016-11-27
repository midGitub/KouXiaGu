using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// 当受困于一个无法行走的节点返回的异常;
    /// </summary>
    public class TrappedException : Exception
    {
        public TrappedException() : base() { }
        public TrappedException(string message) : base(message) { }
        public TrappedException(string message, Exception innerException) : base(message, innerException) { }
    }

}
