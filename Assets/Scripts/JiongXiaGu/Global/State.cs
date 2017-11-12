using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu
{

    /// <summary>
    /// 表示执行状态;
    /// </summary>
    public struct State<T>
    {
        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public Exception Exception { get; private set; }
        public T Value { get; private set; }

        public State(T value)
        {
            Value = value;
            IsCompleted = true;
            IsFaulted = false;
            Exception = null;
        }

        public State(Exception exception)
        {
            Exception = exception;
            IsCompleted = true;
            IsFaulted = true;
            Value = default(T);
        }
    }
}
