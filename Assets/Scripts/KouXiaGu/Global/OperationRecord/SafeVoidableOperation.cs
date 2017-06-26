using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.OperationRecord
{

    /// <summary>
    /// 带撤销,重做检查的抽象类;
    /// </summary>
    public abstract class SafeVoidableOperation : IVoidableOperation
    {
        bool isUndo;

        public abstract void Redo();
        public abstract void Undo();

        void IVoidableOperation.Redo()
        {
            if (isUndo)
            {
                Redo();
                isUndo = false;
            }
            else
            {
                throw new InvalidOperationException("在没撤销时进行重做;");
            }
        }

        void IVoidableOperation.Undo()
        {
            if (isUndo)
            {
                throw new InvalidOperationException("已经进行了撤销操作;");
            }
            else
            {
                Undo();
                isUndo = true;
            }
        }
    }
}
