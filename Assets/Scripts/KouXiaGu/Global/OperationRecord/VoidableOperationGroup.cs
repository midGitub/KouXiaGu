using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.OperationRecord
{

    /// <summary>
    /// 可取消操作组;
    /// </summary>
    public class VoidableOperationGroup : IVoidableOperation
    {
        public VoidableOperationGroup()
        {
            Operations = new List<IVoidableOperation>();
        }

        public List<IVoidableOperation> Operations { get; private set; }

        void IVoidableOperation.Redo()
        {
            for (int i = 0; i < Operations.Count; i++)
            {
                var operation = Operations[i];
                operation.Redo();
            }
        }

        void IVoidableOperation.Undo()
        {
            for (int i = Operations.Count - 1; i >= 0; i--)
            {
                var operation = Operations[i];
                operation.Undo();
            }
        }
    }
}
