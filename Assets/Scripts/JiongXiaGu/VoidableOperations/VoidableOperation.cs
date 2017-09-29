using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.VoidableOperations
{

    /// <summary>
    /// 抽象类 可进行撤销和重做的动作;
    /// </summary>
    public abstract class VoidableOperation
    {
        /// <summary>
        /// 是否执行了这个操作?
        /// </summary>
        public bool IsDid { get; protected set; }

        /// <summary>
        /// 改操作是否被取消了?在 IsDid == true 时,该变量为false;
        /// </summary>
        public bool IsCanceled { get; private set; }

        /// <summary>
        /// 执行重做;
        /// </summary>
        protected abstract void PerformDo_protected();

        /// <summary>
        /// 执行撤销;
        /// </summary>
        protected abstract void PerformUndo_protected();

        /// <summary>
        /// 执行重做;
        /// </summary>
        /// <returns>返回本身</returns>
        public VoidableOperation PerformDo()
        {
            ThrowIfOperationWasCanceled();
            if (!IsDid)
            {
                PerformDo_protected();
                IsDid = true;
            }
            return this;
        }

        /// <summary>
        /// 执行撤销;
        /// </summary>
        public VoidableOperation PerformUndo()
        {
            ThrowIfOperationWasCanceled();
            if (IsDid)
            {
                PerformUndo_protected();
                IsDid = false;
            }
            return this;
        }

        /// <summary>
        /// 若改实例已经被取消,则抛出异常;
        /// </summary>
        protected void ThrowIfOperationWasCanceled()
        {
            if (IsCanceled)
            {
                throw new OperationCanceledException("该操作已经被取消;");
            }
        }

        /// <summary>
        /// 对操作进行取消操作,仅在未进行操作时有效;
        /// </summary>
        public virtual void SetCanceleState(bool isCancel)
        {
            if (isCancel && IsDid)
                throw new ArgumentException("不能取消已经进行的操作;");

            IsCanceled = isCancel;
        }
    }
}
