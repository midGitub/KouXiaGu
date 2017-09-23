using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Operations
{

    /// <summary>
    /// 可进行撤销和重做的动作;
    /// </summary>
    public abstract class VoidableOperation
    {
        /// <summary>
        /// 是否执行了这个操作?
        /// </summary>
        public bool IsDid { get; protected set; }

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
            if (IsDid)
            {
                PerformUndo_protected();
                IsDid = false;
            }
            return this;
        }
    }
}
