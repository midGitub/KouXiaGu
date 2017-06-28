using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.OperationRecord
{


    public interface IRecorder : IVoidable
    {
        IEnumerable<IVoidable> Operations { get; }
        void Register(IVoidable operation);
        void Clear();
    }
}
