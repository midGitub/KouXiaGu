using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Concurrent
{

    public interface IRequest
    {
        bool OnQueue { get; set; }
        void Execute();
    }

}
