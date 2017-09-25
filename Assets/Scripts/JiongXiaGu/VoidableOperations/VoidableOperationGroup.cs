using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.VoidableOperations
{

    /// <summary>
    /// 可取消操作组;
    /// </summary>
    public sealed class VoidableOperationGroup<T> : VoidableOperation, IEnumerable<T>
        where T : VoidableOperation
    {
        public VoidableOperationGroup()
        {
            OperationList = new List<T>();
            IsDid = true;
        }

        /// <summary>
        /// 传入 参数T 必须已经是执行过的(IsDid == true),否者会出现未知异常(此方法不进行检查);
        /// </summary>
        public VoidableOperationGroup(params T[] operations)
        {
            OperationList = new List<T>(operations.Length);
            OperationList.AddRange(operations);
            IsDid = true;
        }

        /// <summary>
        /// 传入 参数T 必须已经是执行过的(IsDid == true),否者会出现未知异常(此方法不进行检查);
        /// </summary>
        public VoidableOperationGroup(IEnumerable<T> operations)
        {
            ICollection<T> collection = operations as ICollection<T>;
            if (collection != null)
            {
                OperationList = new List<T>();
            }
            OperationList.AddRange(operations);
            IsDid = true;
        }

        /// <summary>
        /// 那先后顺序排列的操作合集;
        /// </summary>
        public List<T> OperationList { get; private set; }

        /// <summary>
        /// 添加操作(提供lambda表达式使用的方法)
        /// 传入 参数T 必须已经是执行过的(IsDid == true),否者会出现未知异常(此方法不进行检查);
        /// </summary>
        public VoidableOperationGroup<T> Add(T item)
        {
            OperationList.Add(item);
            return this;
        }

        protected override void PerformDo_protected()
        {
            for (int i = 0; i < OperationList.Count; i++)
            {
                var operation = OperationList[i];
                operation.PerformDo();
            }
        }

        protected override void PerformUndo_protected()
        {
            for (int i = OperationList.Count - 1; i >= 0; i--)
            {
                var operation = OperationList[i];
                operation.PerformUndo();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return OperationList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
