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

    public static class StateExtensions
    {
        /// <summary>
        /// 仅返回成功的状态;
        /// </summary>
        public static IEnumerable<State<T>> WhereCompleted<T>(this IEnumerable<State<T>> collection)
        {
            return collection.Where(state => state.IsCompleted);
        }

        /// <summary>
        /// 仅返回成功的内容;
        /// </summary>
        public static IEnumerable<T> WhereCompletedValue<T>(this IEnumerable<State<T>> collection)
        {
            return collection.Where(state => state.IsCompleted).Select(state => state.Value);
        }

        /// <summary>
        /// 仅返回失败的状态;
        /// </summary>
        public static IEnumerable<State<T>> WhereFaulted<T>(this IEnumerable<State<T>> collection)
        {
            return collection.Where(state => state.IsFaulted);
        }
    }
}
