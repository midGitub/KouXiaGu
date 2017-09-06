using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.Concurrent
{

    public class Awaiter : INotifyCompletion
    {
        public Awaiter()
        {
        }

        public bool IsCompleted { get; private set; }

        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        public void GetResult()
        {
            throw new NotImplementedException();
        }
    }
}
